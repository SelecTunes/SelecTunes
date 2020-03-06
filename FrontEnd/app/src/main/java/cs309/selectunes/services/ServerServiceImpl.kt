package cs309.selectunes.services

import android.content.Intent
import android.widget.ListView
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import cs309.selectunes.R
import cs309.selectunes.activities.HostMenuActivity
import cs309.selectunes.activities.SongSearchActivity
import cs309.selectunes.utils.HttpUtils
import org.json.JSONObject
import java.nio.charset.StandardCharsets

class ServerServiceImpl : ServerService {

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

    override fun searchSong(songToSearch: String, activity: SongSearchActivity) {
        val json = JSONObject()
        json.put("QueryString", songToSearch)
        val jsonObjectRequest = object : JsonObjectRequest(Method.POST, "https://coms-309-jr-2.cs.iastate.edu/api/Song/SearchBySong", json,
                Response.Listener {
                    activity.parseJson(it)
                    val listView = activity.findViewById<ListView>(R.id.song_search_list)
                    val adapter = SongSearchActivity.SongAdapter(activity, activity.songList)
                    listView.adapter = adapter
                },
                Response.ErrorListener {
                    println("Error fetching JSON object: ${it.networkResponse.statusCode}")
                    println(it.networkResponse.data.toString(StandardCharsets.UTF_8))
                }) {
            override fun getParams(): Map<String, String> {
                val params: MutableMap<String, String> = java.util.HashMap()
                params["QueryString"] = songToSearch
                return params
            }

            override fun getHeaders(): Map<String, String> {
                val headers: MutableMap<String, String> = java.util.HashMap()
                headers["Content-Type"] = "application/json"
                headers["Accept"] = "application/json, text/json"
                return headers
            }
        }
        val requestQueue = Volley.newRequestQueue(activity, HttpUtils.createAuthCookie(activity))
        requestQueue.add(jsonObjectRequest)
    }
}