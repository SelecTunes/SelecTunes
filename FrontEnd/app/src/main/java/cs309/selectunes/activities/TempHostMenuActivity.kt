package cs309.selectunes.activities

import android.os.Bundle
import android.view.KeyEvent
import android.view.View
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import cs309.selectunes.R
import cs309.selectunes.models.Song
import cs309.selectunes.utils.HttpUtils
import org.json.JSONObject
import java.nio.charset.StandardCharsets
import java.util.*
import kotlin.collections.ArrayList

class TempHostMenuActivity : AppCompatActivity() {

    var songList = ArrayList<Song>()
    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.host_menu_main)
        val guestList = findViewById<Button>(R.id.guestList_id)
        val songList = findViewById<Button>(R.id.songQueue_id)
        val songSearch = findViewById<TextView>(R.id.song_search_id)
        songSearch.setOnKeyListener(View.OnKeyListener { v, keyCode, event ->
            if (keyCode == KeyEvent.KEYCODE_ENTER && event.action == KeyEvent.ACTION_UP) {
                searchSong(songSearch.text.toString())
                return@OnKeyListener true
            }
            false
        })

        guestList.setOnClickListener {
            //val toGuestList = Intent(this, GuestListActivity::class.java)
            //startActivity(toGuestList)
        }

        songList.setOnClickListener {
            //val toSongList = Intent(this, SongListActivity::class.java)
            //startActivity(toSongList)
        }


    }

    private fun searchSong(songToSearch: String) {
        val json = JSONObject()
        json.put("QueryString", songToSearch)
        val jsonObjectRequest = object : JsonObjectRequest(Method.POST, "https://coms-309-jr-2.cs.iastate.edu/api/Song/SearchBySong", json,
                Response.Listener {
                    parseJson(it)
                },
                Response.ErrorListener {
                    println("Error fetching JSON object: ${it.networkResponse.statusCode}")
                    println(it.networkResponse.data.toString(StandardCharsets.UTF_8))
                }) {
            override fun getParams(): Map<String, String> {
                val params: MutableMap<String, String> = HashMap()
                params["QueryString"] = songToSearch
                return params
            }

            override fun getHeaders(): Map<String, String> {
                val headers: MutableMap<String, String> = HashMap()
                headers["Content-Type"] = "application/json"
                headers["Accept"] = "application/json, text/json"
                return headers
            }
        }
        val requestQueue = Volley.newRequestQueue(this, HttpUtils.createAuthCookie(this))
        requestQueue.add(jsonObjectRequest)
    }

    private fun parseJson(jsonBack: JSONObject) {
        val jsonItems = jsonBack.getJSONObject("tracks").getJSONArray("items")
        for (x in 0 until jsonItems.length()) {
            val jsonSong = jsonItems.getJSONObject(x)
            val song = jsonSong.get("name")
            val songName = song.toString()
            val explicit = jsonSong.getBoolean("explicit")
            val jsonAlbum = jsonSong.getJSONObject("album")
            val artistName = jsonAlbum.getJSONArray("artists").getJSONObject(0).getString("name")
            val albumArt = jsonAlbum.getJSONArray("images")
            val firstSize = albumArt.getJSONObject(0)
            val albumArtSrc = firstSize.getString("url")
            println("Song $x: name: $songName, artist: $artistName, albumSrc: $albumArtSrc, explicit: $explicit")
            songList.add(
                    Song(
                            songName,
                            "",
                            artistName,
                            albumArtSrc,
                            explicit
                    )
            )
        }

    }
}