package cs309.selectunes.services

import android.content.Intent
import android.widget.ListView
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.StringRequest
import com.android.volley.toolbox.Volley
import cs309.selectunes.R
import cs309.selectunes.activities.GuestListActivity
import cs309.selectunes.activities.HostMenuActivity
import cs309.selectunes.adapter.GuestAdapter
import cs309.selectunes.utils.HttpUtils
import org.json.JSONArray
import java.nio.charset.StandardCharsets

class PartyServiceImpl : PartyService {

    override fun createParty(auth: String, activity: AppCompatActivity) {
        val url = "https://coms-309-jr-2.cs.iastate.edu/api/Auth/Callback?Code=$auth"
        val jsonObjectRequest = object : JsonObjectRequest(Method.GET, url, null,
                Response.Listener {
                    val settings = activity.getSharedPreferences("PartyInfo", 0)
                    val editor = settings.edit()
                    editor.putString("join_code", it.getString("joinCode"))
                    editor.apply()
                    activity.startActivity(Intent(activity, HostMenuActivity::class.java))
                },
                Response.ErrorListener {
                    println("Error adding spotify login and creating party: ${it.networkResponse.statusCode}")
                    println(it.networkResponse.data.toString(StandardCharsets.UTF_8))
                }) {
            override fun getParams(): Map<String, String> {
                val params: MutableMap<String, String> = HashMap()
                params["Code"] = auth
                return params
            }
        }
        val requestQueue = Volley.newRequestQueue(activity, HttpUtils.createAuthCookie(activity))
        requestQueue.add(jsonObjectRequest)
    }

    override fun kickGuest(givenEmail: String, activity: GuestListActivity) {
        val jsonObjectRequest = object : StringRequest(Method.POST, "https://coms-309-jr-2.cs.iastate.edu/api/Auth/Kick",
                Response.Listener {
                    this.getGuestList(activity, false)
                },
                Response.ErrorListener {
                    println("Error kicking user: ${it.networkResponse.statusCode}")
                    println(it.networkResponse.data.toString(StandardCharsets.UTF_8))
                }) {
            override fun getHeaders(): Map<String, String> {
                val headers: MutableMap<String, String> = java.util.HashMap()
                headers["Content-Type"] = "application/x-www-form-urlencoded"
                headers["Accept"] = "application/json, text/json"
                return headers
            }
            override fun getParams(): Map<String, String> {
                val params: MutableMap<String, String> = HashMap()
                params["email"] = givenEmail
                return params
            }
        }
        val requestQueue = Volley.newRequestQueue(activity, HttpUtils.createAuthCookie(activity))
        requestQueue.add(jsonObjectRequest)
    }

    override fun getGuestList(activity: GuestListActivity, isGuest: Boolean) {
        val jsonObjectRequest = object : StringRequest(Method.GET, "https://coms-309-jr-2.cs.iastate.edu/api/Party/Members",
                Response.Listener {
                    val array = JSONArray(it)
                    val guests = activity.parseGuests(array)
                    activity.setContentView(R.layout.guest_list_menu)
                    val listView = activity.findViewById<ListView>(R.id.guests)
                    val adapter = GuestAdapter(activity, guests, activity, isGuest)
                    listView.adapter = adapter
                },
                Response.ErrorListener {
                    println("Error getting the memberList...")
                }) {
            override fun getHeaders(): Map<String, String> {
                val headers: MutableMap<String, String> = java.util.HashMap()
                headers["Content-Type"] = "application/json"
                headers["Accept"] = "application/json, text/json, text/plain"
                return headers
            }
        }
        val requestQueue = Volley.newRequestQueue(activity, HttpUtils.createAuthCookie(activity))
        requestQueue.add(jsonObjectRequest)
    }

    override fun endParty(activity: AppCompatActivity, switchedActivity: Class<out AppCompatActivity>?) {
        val stringRequest = StringRequest(Request.Method.DELETE,
                "https://coms-309-jr-2.cs.iastate.edu/api/Party/DisbandParty",
                Response.Listener {
                    if (switchedActivity != null) {
                        activity.startActivity(Intent(activity, switchedActivity))
                    }
                },
                Response.ErrorListener {
                    println("There was an error with the response. Code: ${it.networkResponse.statusCode}")
                    println("Response: ${it.networkResponse.data.toString(StandardCharsets.UTF_8)}")
                })
        val requestQueue = Volley.newRequestQueue(activity, HttpUtils.createAuthCookie(activity))
        requestQueue.add(stringRequest)
    }

    override fun leaveParty(activity: AppCompatActivity, switchedActivity: Class<out AppCompatActivity>?) {
        val stringRequest = StringRequest(Request.Method.POST,
                "https://coms-309-jr-2.cs.iastate.edu/api/Party/LeaveParty",
                Response.Listener {
                    if (switchedActivity != null) {
                        activity.startActivity(Intent(activity, switchedActivity))
                    }
                },
                Response.ErrorListener {
                    println("There was an error with the response. Code: ${it.networkResponse.statusCode}")
                    println("Response: ${it.networkResponse.data.toString(StandardCharsets.UTF_8)}")
                })
        val requestQueue = Volley.newRequestQueue(activity, HttpUtils.createAuthCookie(activity))
        requestQueue.add(stringRequest)
    }
}