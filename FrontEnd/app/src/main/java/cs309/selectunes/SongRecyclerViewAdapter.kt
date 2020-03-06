package cs309.selectunes

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import cs309.selectunes.models.Guest
import cs309.selectunes.models.Song

class SongRecyclerViewAdapter(val songs: ArrayList<Song>,
                              val context : Context): RecyclerView.Adapter<CustomViewHolder>()
{
    override fun getItemCount(): Int
    {
    return songs.size
    }

    override fun onBindViewHolder(holder: CustomViewHolder, position: Int) {
        var songToBind = songs[position]
        holder.bind(songToBind)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): CustomViewHolder {
    val layoutInflater = LayoutInflater.from(parent?.context)
        val songCell = layoutInflater.inflate(R.layout.song_row,parent,false)
        return CustomViewHolder(songCell)
    }
}

class CustomViewHolder(view: View) : RecyclerView.ViewHolder(view)
{

    private var albumPic : ImageView? = null
    private var songName : TextView? = null
    private var artistName : TextView? = null

    init{
        albumPic = view.findViewById(R.id.album_cover)
        songName = view.findViewById(R.id.songName)
        artistName = view.findViewById(R.id.artistName)
    }

    fun bind(givenSong: Song)
    {
        artistName?.text = givenSong.artistName
        songName?.text = givenSong.songName

    }
}