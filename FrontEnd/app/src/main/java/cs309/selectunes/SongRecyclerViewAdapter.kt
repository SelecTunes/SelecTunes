package cs309.selectunes

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import cs309.selectunes.models.Song
import kotlinx.android.synthetic.main.song_row.view.*

class SongRecyclerViewAdapter(private val songs: ArrayList<Song>,
                              val context : Context): RecyclerView.Adapter<CustomViewHolder>()
{
    override fun getItemCount(): Int
    {
        return songs.size
    }

    override fun onBindViewHolder(holder: CustomViewHolder, position: Int)
    {
        val thisSong: Song = songs[position]
        holder.view.songName.text = thisSong.songName
        holder.view.artistName.text = thisSong.artistName

    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): CustomViewHolder {
    val layoutInflater = LayoutInflater.from(parent.context)
        val songCell = layoutInflater.inflate(R.layout.song_row,parent,false)
        return CustomViewHolder(songCell)
    }
}

class CustomViewHolder(val view: View) : RecyclerView.ViewHolder(view)
{

}