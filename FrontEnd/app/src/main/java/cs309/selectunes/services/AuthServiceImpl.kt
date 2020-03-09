package cs309.selectunes.services

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.NetworkResponse
import com.android.volley.Response
import com.android.volley.toolbox.StringRequest
import com.android.volley.toolbox.Volley
import com.spotify.sdk.android.authentication.LoginActivity
import cs309.selectunes.activities.ChooseActivity
import cs309.selectunes.utils.JsonUtils

class AuthServiceImpl : AuthService {

    override fun login(email: String, password: String, activity: AppCompatActivity) {
        val stringRequest = object : StringRequest(Method.POST, "https://coms-309-jr-2.cs.iastate.edu/api/auth/login", Response.Listener {
            val success = JsonUtils.parseLoginResponse(activity, it)
            if (success) activity.startActivity(Intent(activity, ChooseActivity::class.java))
        }, Response.ErrorListener {
            println("There was an error with the response. Code: ${it.networkResponse.statusCode}")
        }) {
            override fun getParams(): Map<String, String> {
                val params: MutableMap<String, String> = HashMap()
                params["Email"] = email
                params["Password"] = password
                params["ConfirmPassword"] = password
                return params
            }

            override fun getHeaders(): Map<String, String> {
                val headers: MutableMap<String, String> = HashMap()
                headers["Content-Type"] = "application/x-www-form-urlencoded"
                headers["Accept"] = "application/json, text/json"
                return headers
            }

            override fun parseNetworkResponse(response: NetworkResponse?): Response<String> {
                val responseHeaders = response?.headers
                val rawCookies = responseHeaders?.get("Set-Cookie")
                if (rawCookies != null) {
                    val search = "Holtzmann="
                    val cookie = rawCookies.substring(rawCookies.indexOf(search) + search.length, rawCookies.indexOf(';'))
                    val settings = activity.getSharedPreferences("Cookie", 0)
                    settings.edit().putString("cookie", cookie).apply()
                }
                return super.parseNetworkResponse(response)
            }
        }
        val requestQueue = Volley.newRequestQueue(activity)
        requestQueue.add(stringRequest)
    }

    override fun register(email: String, password: String, passwordConfirmed: String, activity: AppCompatActivity) {
        val stringRequest = object : StringRequest(Method.POST, "https://coms-309-jr-2.cs.iastate.edu/api/auth/register", Response.Listener {
            val success = JsonUtils.parseRegisterResponse(activity, it, 200)
            if (success) activity.startActivity(Intent(activity, LoginActivity::class.java))
        }, Response.ErrorListener {
            JsonUtils.parseRegisterResponse(activity, null, it.networkResponse.statusCode)
            println("There was an error with the response. Code: ${it.networkResponse.statusCode}")
        }) {
            override fun getParams(): Map<String, String> {
                val params: MutableMap<String, String> = HashMap()
                params["Email"] = email
                params["Password"] = password
                params["ConfirmPassword"] = passwordConfirmed
                return params
            }

            override fun getHeaders(): Map<String, String> {
                val headers: MutableMap<String, String> = HashMap()
                headers["Content-Type"] = "application/x-www-form-urlencoded"
                headers["Accept"] = "application/json, text/json"
                return headers
            }
        }
        val requestQueue = Volley.newRequestQueue(activity)
        requestQueue.add(stringRequest)
    }
}