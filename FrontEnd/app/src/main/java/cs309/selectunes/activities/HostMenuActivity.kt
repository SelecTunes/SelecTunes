package cs309.selectunes.activities

import android.content.Intent
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
import cs309.selectunes.utils.HttpUtils
import org.json.JSONObject


class HostMenuActivity : AppCompatActivity() {

    var songArray= ArrayList<Song>()


    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.test_host_menu)


        val guestList = findViewById<Button>(R.id.guest_list)
        val songList = findViewById<Button>(R.id.song_list)
        val backArrow = findViewById<Button>(R.id.back_arrow)
        val endParty = findViewById<Button>(R.id.end_party)
        val moderators = findViewById<Button>(R.id.moderators)
        val explicit = findViewById<Button>(R.id.explicit)
        val searchBar = findViewById<TextView>(R.id.song_search_id)

        guestList.setOnClickListener{
            val toGuestList = Intent(this, GuestListActivity::class.java)
            startActivity(toGuestList)
        }

        songList.setOnClickListener{
            val toSongList = Intent(this, TempHostMenuActivity::class.java)
            startActivity(toSongList)
        }

        backArrow.setOnClickListener{
            HttpUtils.endParty(this)
            startActivity(Intent(this, ChooseActivity::class.java))
        }

        endParty.setOnClickListener{
            HttpUtils.endParty(this)
            startActivity(Intent(this, LoginActivity::class.java))
        }

        searchBar.setOnKeyListener(View.OnKeyListener { v, keyCode, event ->
            if (keyCode == KeyEvent.KEYCODE_ENTER && event.action == KeyEvent.ACTION_UP) {

                getRequest(searchBar.text.toString())

                return@OnKeyListener true
            }
            false
        })
    }

    override fun onStart() {
        super.onStart()
        val textView = findViewById<TextView>(R.id.session_id)
        textView.text = intent.getStringExtra("code")
    }

    private fun getRequest(songToSearch : String){

        val requestQueue = Volley.newRequestQueue(this)
        val json = JSONObject()
        json.put("queryString", songToSearch)
        val url = "https://coms-309-jr-2.cs.iastate.edu/api/song/searchbysong"
        val jsonObjectRequest = object: JsonObjectRequest(Method.GET, url, json,
                Response.Listener {
                    println("Attempting to fetch JSON")
                    parseJson(it)
                },
                Response.ErrorListener {
                    println("There was an error with the response. Code: ${it.networkResponse.statusCode}")
                }) {

            override fun getHeaders(): MutableMap<String, String> {
                val headers: MutableMap<String, String> = HashMap()
                headers["Content-Type"] = "application/json"
                headers["Accept"] = "application/json"
                return headers
            }
        }


        requestQueue.add(jsonObjectRequest)
    }

    private fun parseJson(jsonBack : JSONObject)
    {
        println("Parsing JSON")
        val jsonTracks = jsonBack.getJSONObject("tracks")
        val jsonItems = jsonTracks.getJSONArray("items")
        for(x in 0..9)
        {
            var newSong = Song()
            val jsonSong = jsonItems.getJSONObject(x)
            val song = jsonSong.get("name")
            println(song)

            newSong.songName = song.toString()

            val explicit = jsonSong.getBoolean("explicit")
            println(explicit)

            newSong.explicitBool = explicit

            val artist = jsonSong.get("artist")

            newSong.artistName = artist.toString()

            val jsonAlbum = jsonSong.getJSONObject("album")
            val albumArt = jsonAlbum.getJSONArray("images")
            val firstSize = albumArt.getJSONObject(0)
            val imageUrl = firstSize.get("url")
            println(imageUrl)

            newSong.albumUrl = imageUrl.toString()

            songArray.add(newSong)
        }

    }
}