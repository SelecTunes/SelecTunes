package cs309.selectunes.activities

import android.os.Bundle
import android.view.KeyEvent
import android.view.View
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.HttpClientStack
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import cs309.selectunes.R
import cs309.selectunes.models.Song
import org.apache.http.impl.client.BasicCookieStore
import org.apache.http.impl.client.DefaultHttpClient
import org.apache.http.impl.cookie.BasicClientCookie
import org.json.JSONObject
import java.nio.charset.StandardCharsets

class TempHostMenuActivity : AppCompatActivity()
{

    var songList = ArrayList<Song>()
    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.host_menu_main)
        val guestList = findViewById<Button>(R.id.guestList_id)
        val songList = findViewById<Button>(R.id.songQueue_id)
        val songSearch = findViewById<TextView>(R.id.song_search_id)

        songSearch.setOnKeyListener(View.OnKeyListener { v, keyCode, event ->
            if (keyCode == KeyEvent.KEYCODE_ENTER && event.action == KeyEvent.ACTION_UP) {
                searchSong("{\"queryString\": \"${songSearch.text}\"}")
                return@OnKeyListener true
            }
            false
        })

        guestList.setOnClickListener{
            //val toGuestList = Intent(this, GuestListActivity::class.java)
            //startActivity(toGuestList)
        }

        songList.setOnClickListener{
            //val toSongList = Intent(this, SongListActivity::class.java)
            //startActivity(toSongList)
        }



    }

    private fun searchSong(songToSearch: String) {
        val partyCodeBox = findViewById<TextView>(R.id.PartyCode)
        val httpclient = DefaultHttpClient()
        val cookieStore = BasicCookieStore()
        val settings = getSharedPreferences("Cookie", 0)
        val cookie = BasicClientCookie("Holtzmann", settings.getString("cookie", ""))
        cookie.domain = "coms-309-jr-2.cs.iastate.edu"
        cookieStore.addCookie(cookie)
        httpclient.cookieStore = cookieStore
        val httpStack = HttpClientStack(httpclient)

        val json = JSONObject(songToSearch)
        val url = "https://coms-309-jr-2.cs.iastate.edu/api/Song/SearchBySong"

        val jsonObjectRequest = object : JsonObjectRequest(Request.Method.GET, url, json,
                Response.Listener {
                    println("Attempting to fetch JSON")
                    parseJson(it)
                },
                Response.ErrorListener {

                    println("Error fetching JSON object")
                    println(it.networkResponse.data.toString(StandardCharsets.UTF_8))

                }) {
            override fun getHeaders(): MutableMap<String, String> {
                val headers = HashMap<String, String>()
                headers["Content-Type"] = "application/json"
                return headers
            }
        }
        val requestQueue = Volley.newRequestQueue(this, httpStack)
        requestQueue.add(jsonObjectRequest)
    }

    private fun parseJson(jsonBack: JSONObject) {
        val jsonItems = jsonBack.getJSONObject("tracks").getJSONArray("items")
        for (x in 0..9) {
            val jsonSong = jsonItems.getJSONObject(x)
            val song = jsonSong.get("name")
            val songName = song.toString()
            val explicit = jsonSong.getBoolean("explicit")
            val artistName = jsonSong.getString("artist")
            val jsonAlbum = jsonSong.getJSONObject("album")
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