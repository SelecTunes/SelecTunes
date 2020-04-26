package cs309.selectunes.services

import android.widget.Button
import android.widget.ListView
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.StringRequest
import com.android.volley.toolbox.Volley
import com.microsoft.signalr.HubConnection
import cs309.selectunes.R
import cs309.selectunes.activities.SongQueueActivity
import cs309.selectunes.activities.SongSearchActivity
import cs309.selectunes.adapter.QueueAdapter
import cs309.selectunes.adapter.SongAdapter
import cs309.selectunes.models.Song
import cs309.selectunes.utils.HttpUtils
import cs309.selectunes.utils.JsonUtils
import org.json.JSONException
import org.json.JSONObject
import java.nio.charset.StandardCharsets

class SongServiceImpl : SongService {

    override fun makeSongsExplicit(activity: AppCompatActivity) {
        val stringRequest = StringRequest(
                Request.Method.POST,
                "https://coms-309-jr-2.cs.iastate.edu/api/party/explicit",
                Response.Listener {
                    isExplicit(activity)
                },
                Response.ErrorListener {
                    println("There was an error with making the song explicit: ${it.networkResponse.statusCode}")
                    println(it.networkResponse.data)
                })
        val requestQueue = Volley.newRequestQueue(activity, HttpUtils.createAuthCookie(activity))
        requestQueue.add(stringRequest)
    }

    override fun isExplicit(activity: AppCompatActivity) {
        val stringRequest = JsonObjectRequest(
                Request.Method.GET,
                "https://coms-309-jr-2.cs.iastate.edu/api/party/explicit",
                null,
                Response.Listener {
                    if (it != null) {
                        val explicit = activity.findViewById<Button>(R.id.explicit)
                        if (it.getBoolean("allowed")) {
                            explicit.setBackgroundColor(activity.resources.getColor(R.color.colorSecondaryDark))
                        } else {
                            explicit.setBackgroundColor(activity.resources.getColor(R.color.colorBright))
                        }
                    }
                },
                Response.ErrorListener {
                    println("There was an error with making the song explicit: ${it.networkResponse.statusCode}")
                    println(it.networkResponse.data)
                })
        val requestQueue = Volley.newRequestQueue(activity, HttpUtils.createAuthCookie(activity))
        requestQueue.add(stringRequest)
    }

    override fun getSongQueue(
            activity: SongQueueActivity,
            socket: HubConnection?,
            votes: Map<String, Int>?
    ) {
        val url = "https://coms-309-jr-2.cs.iastate.edu/api/Song/Queue"
        val jsonObjectRequest = StringRequest(
                Request.Method.GET, url,
                Response.Listener {
                    println(it)
                    try {
                        val jsonObject = JSONObject(it)
                        val voteableSongArray = jsonObject.getJSONArray("votable")
                        val lockedSongArray = jsonObject.getJSONArray("lockedIn")
                        val voteableSongList = JsonUtils.parseSongQueue(voteableSongArray, true)
                        val lockedSongList = JsonUtils.parseSongQueue(lockedSongArray, false)
                        val allSongs = ArrayList<Song>()
                        allSongs.addAll(lockedSongList)
                        allSongs.addAll(voteableSongList)
                        val listView = activity.findViewById<ListView>(R.id.song_queue_list)
                        val adapter = QueueAdapter(activity, allSongs, socket!!, votes!!)
                        listView.adapter = adapter
                    } catch (e: JSONException) {
                        println("The queue doesn't exist.")
                    }
                },
                Response.ErrorListener {
                    if (it.networkResponse != null) {
                        println("Error getting the song queue: ${it.networkResponse.statusCode}")
                        println(it.networkResponse.data.toString(StandardCharsets.UTF_8))
                    }
                })
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

    override fun removeLockedSong(song: String, activity: AppCompatActivity) {
        val json = JSONObject()
        json.put("id", song)
        val jsonObjectRequest = object : JsonObjectRequest(Method.POST, "https://coms-309-jr-2.cs.iastate.edu/api/Song/ThisSongWasPlayed", json, null,
                Response.ErrorListener {
                    println("Error removing the song $song from queue: ${it.networkResponse.statusCode}")
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
}