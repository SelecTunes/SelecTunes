package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.view.KeyEvent
import android.view.View
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import cs309.selectunes.models.Song
import cs309.selectunes.services.SongServiceImpl
import org.json.JSONObject

/**
 * The song search activity is where users
 * can search for new songs to be added to the
 * queue.
 * @author Jack Goldsworth
 */
class SongSearchActivity : AppCompatActivity() {

    internal val songList = mutableListOf<Song>()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.song_search_menu)

        val songSearch = findViewById<TextView>(R.id.search_song_input)
        val backArrow = findViewById<Button>(R.id.back_arrow_song_search)

        songSearch.setOnKeyListener(View.OnKeyListener { v, keyCode, event ->
            if (keyCode == KeyEvent.KEYCODE_ENTER && event.action == KeyEvent.ACTION_UP) {
                SongServiceImpl().searchSong(songSearch.text.toString(), this)
                return@OnKeyListener true
            }
            false
        })

        backArrow.setOnClickListener {
            if (intent.getStringExtra("previousActivity") == "host")
                startActivity(Intent(this, HostMenuActivity::class.java))
            else
                startActivity(Intent(this, GuestMenuActivity::class.java))
        }
    }

    fun parseJson(jsonBack: JSONObject) {
        songList.clear()
        val jsonItems = jsonBack.getJSONObject("tracks").getJSONArray("items")
        for (x in 0 until jsonItems.length()) {
            val jsonSong = jsonItems.getJSONObject(x)
            val song = jsonSong.get("name")
            val songName = song.toString()
            val songId = jsonSong.get("id").toString()
            val explicit = jsonSong.getBoolean("explicit")
            val jsonAlbum = jsonSong.getJSONObject("album")
            val artistName = jsonAlbum.getJSONArray("artists").getJSONObject(0).getString("name")
            val albumArt = jsonAlbum.getJSONArray("images")
            val firstSize = albumArt.getJSONObject(0)
            val albumArtSrc = firstSize.getString("url")
            songList.add(
                    Song(
                            songName,
                            songId,
                            artistName,
                            albumArtSrc,
                            explicit,
                            null
                    )
            )
        }
    }
}
