package cs309.selectunes

import android.content.Intent
import android.os.Bundle
import android.view.KeyEvent
import android.view.View
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.NetworkResponse
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import org.json.JSONObject

class HostMenuActivity : AppCompatActivity()
{
    override fun onCreate(instanceState: Bundle?)
    {
        super.onCreate(instanceState)
        setContentView(R.layout.host_menu_main)
        val guestList = findViewById<Button>(R.id.guestList_id)
        val songList = findViewById<Button>(R.id.songQueue_id)
        val songSearch = findViewById<TextView>(R.id.song_search_id)

        songSearch.setOnKeyListener(View.OnKeyListener { v, keyCode, event ->
            if (keyCode == KeyEvent.KEYCODE_ENTER && event.action == KeyEvent.ACTION_UP)
            {
                postRequest(songSearch.text.toString())
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

    private fun postRequest(songToSearch : String){

        val partyCodeBox = findViewById<TextView>(R.id.PartyCode)

        val requestQueue = Volley.newRequestQueue(this)
        val json = JSONObject(songToSearch)
        val url = "https://coms-309-jr-2.cs.iastate.edu/api/Song/SearchBySong"

        val jsonObjectRequest = JsonObjectRequest(Request.Method.POST, url, json,
                Response.Listener {
                    println("Attempting to fetch JSON")
                    parseJson(it)
                },
                Response.ErrorListener {

                    println("Error fetching JSON object")

                })

        requestQueue.add(jsonObjectRequest)
    }

    private fun parseJson(jsonBack : JSONObject)
    {
        println("Parsing JSON")
        val jsonTop = jsonBack.getJSONObject("tracks")
        val jsonArr = jsonTop.getJSONArray("items")
        for(x in 0..10)
        {
            var thisSong = Song()
            val song = jsonArr.getJSONObject(x)
            val info = song.getJSONObject("name")
            println(info.toString())
            //thisSong.songName = info.
        }

    }
}