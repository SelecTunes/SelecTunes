package cs309.selectunes.adapter

import android.annotation.SuppressLint
import android.content.Context
import android.graphics.BitmapFactory
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.ImageView
import android.widget.TextView
import cs309.selectunes.R
import cs309.selectunes.models.Song
import cs309.selectunes.utils.BitmapCache
import java.net.URL

/**
 * This is a custom adapter for the song search
 * list. It allows each song to get it's own image,
 * name, and artist name in the list.
 * @author Jack Goldsworth
 */
class SongAdapter(private val ctx: Context, private val songList: List<Song>) : ArrayAdapter<String>(ctx, R.layout.song_search_menu, songList as List<String>) {

    @SuppressLint("ViewHolder", "SetTextI18n")
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