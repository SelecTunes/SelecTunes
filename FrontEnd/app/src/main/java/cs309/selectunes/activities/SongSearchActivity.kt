package cs309.selectunes.activities

import android.content.Context
import android.content.Intent
import android.graphics.BitmapFactory
import android.os.Bundle
import android.view.KeyEvent
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import cs309.selectunes.R
import cs309.selectunes.models.Song
import cs309.selectunes.utils.HttpUtils
import org.json.JSONObject
import java.net.URL
import java.nio.charset.StandardCharsets
import java.util.*


class SongSearchActivity : AppCompatActivity() {

    private val songList = mutableListOf<Song>()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_song_search)

        val songSearch = findViewById<TextView>(R.id.search_song_input)
        val backArrow = findViewById<Button>(R.id.back_arrow_song_search)

        songSearch.setOnKeyListener(View.OnKeyListener { v, keyCode, event ->
            if (keyCode == KeyEvent.KEYCODE_ENTER && event.action == KeyEvent.ACTION_UP) {
                searchSong(songSearch.text.toString())
                return@OnKeyListener true
            }
            false
        })

        backArrow.setOnClickListener {
            startActivity(Intent(this, HostMenuActivity::class.java))
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
        songList.clear()
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

        val listView = findViewById<ListView>(R.id.song_search_list)
        val adapter = SongAdapter(this, songList)
        listView.adapter = adapter
    }


    class SongAdapter(private val ctx: Context, private val songList: List<Song>) : ArrayAdapter<String>(ctx, R.layout.activity_song_search, songList as List<String>) {

        override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {
            val layoutInflater = ctx.applicationContext.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater
            val row = layoutInflater.inflate(R.layout.song_row, parent, false)
            val artist = row.findViewById<TextView>(R.id.artistName)
            val song = row.findViewById<TextView>(R.id.songName)
            val image = row.findViewById<ImageView>(R.id.album_cover)
            artist.text = "By: ${songList[position].artistName}"
            song.text = songList[position].songName
            val url = URL(songList[position].albumArt)
            val bmp = BitmapFactory.decodeStream(url.openConnection().getInputStream())
            image.setImageBitmap(bmp)
            return row
        }
    }
}
