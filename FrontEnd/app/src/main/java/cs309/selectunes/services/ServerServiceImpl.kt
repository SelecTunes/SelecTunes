package cs309.selectunes.services

import android.content.Intent
import android.widget.ListView
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import cs309.selectunes.R
import cs309.selectunes.activities.HostMenuActivity
import cs309.selectunes.activities.SongListActivity
import cs309.selectunes.activities.SongSearchActivity
import cs309.selectunes.adapter.QueueAdapter
import cs309.selectunes.adapter.SongAdapter
import cs309.selectunes.models.Song
import cs309.selectunes.utils.HttpUtils
import cs309.selectunes.utils.JsonUtils
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
                    val adapter = SongAdapter(activity, activity.songList)
                    listView.adapter = adapter
                    listView.setOnItemClickListener { parent, view, position, id ->
                        addSongToQueue(activity.songList[position], activity)
                    }
                },
                Response.ErrorListener {
                    println("Error fetching JSON object: ${it.networkResponse.statusCode}")
                    println(it.networkResponse.data.toString(StandardCharsets.UTF_8))
                }) {

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

    override fun addSongToQueue(song: Song, activity: AppCompatActivity) {
        val json = JSONObject()
        json.put("id", song.id)
        json.put("name", song.songName)
        json.put("artistName", song.artistName)
        json.put("albumArt", song.albumArt)
        val jsonObjectRequest = object : JsonObjectRequest(Method.POST, "https://coms-309-jr-2.cs.iastate.edu/api/Song/AddToQueue", json, null,
                Response.ErrorListener {
                    println("Error adding song to queue: ${it.networkResponse.statusCode}")
                    println(it.networkResponse.data.toString(StandardCharsets.UTF_8))
                }) {
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

    override fun getSongQueue(activity: SongListActivity) {
        val url = "https://coms-309-jr-2.cs.iastate.edu/api/Song/Queue"
        val jsonObjectRequest = JsonObjectRequest(Request.Method.GET, url, null,
            Response.Listener {
                val songList = JsonUtils.parseSongQueue(it)
                val listView = activity.findViewById<ListView>(R.id.song_queue_list)
                val adapter = QueueAdapter(activity, activity.songs)
                listView.adapter = adapter
                listView.setOnItemClickListener { parent, view, position, id ->
                    // TODO updoot and downdoot.
                }
            },
            Response.ErrorListener {
                println("Error getting the song queue: ${it.networkResponse.statusCode}")
                println(it.networkResponse.data.toString(StandardCharsets.UTF_8))
            })
        val requestQueue = Volley.newRequestQueue(activity, HttpUtils.createAuthCookie(activity))
        requestQueue.add(jsonObjectRequest)
    }
}