package cs309.selectunes.activities

import android.content.Context
import android.content.Intent
import android.graphics.BitmapFactory
import android.os.Bundle
import android.view.KeyEvent
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.ImageView
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import cs309.selectunes.models.Song
import cs309.selectunes.services.ServerServiceImpl
import cs309.selectunes.utils.BitmapCache
import org.json.JSONObject
import java.net.URL


class SongSearchActivity : AppCompatActivity() {

    internal val songList = mutableListOf<Song>()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.song_search_menu)

        val songSearch = findViewById<TextView>(R.id.search_song_input)
        val backArrow = findViewById<Button>(R.id.back_arrow_song_search)

        songSearch.setOnKeyListener(View.OnKeyListener { v, keyCode, event ->
            if (keyCode == KeyEvent.KEYCODE_ENTER && event.action == KeyEvent.ACTION_UP) {
                ServerServiceImpl().searchSong(songSearch.text.toString(), this)
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
        println(jsonBack.toString())
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
            //println("Song $x: name: $songName, id: $songId, artist: $artistName, albumSrc: $albumArtSrc, explicit: $explicit")
            songList.add(
                    Song(
                            songName,
                            songId,
                            artistName,
                            albumArtSrc,
                            explicit
                    )
            )
        }
    }


    class SongAdapter(private val ctx: Context, private val songList: List<Song>) : ArrayAdapter<String>(ctx, R.layout.song_search_menu, songList as List<String>) {

        override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {
            val layoutInflater = ctx.applicationContext.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater
            val row = layoutInflater.inflate(R.layout.song_row, parent, false)
            val artist = row.findViewById<TextView>(R.id.artistName)
            val song = row.findViewById<TextView>(R.id.songName)
            val image = row.findViewById<ImageView>(R.id.album_cover)
            val songId = songList[position].id
            artist.text = "By: ${songList[position].artistName}"
            song.text = songList[position].songName
            var bitmap = BitmapCache.loadBitmap(songId)
            if (bitmap == null) {
                val url = URL(songList[position].albumArt)
                bitmap = BitmapFactory.decodeStream(url.openConnection().getInputStream())
                BitmapCache.store(songId, bitmap)
            }
            image.setImageBitmap(bitmap)
            return row
        }
    }
}
