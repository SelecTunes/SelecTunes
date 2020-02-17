package cs309.selectunes

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView

class SongRecyclerViewAdapter(val songs: ArrayList<String>,
                              val context : Context): RecyclerView.Adapter<CustomViewHolder>()
{
    override fun getItemCount(): Int
    {
    return songs.size
    }

    override fun onBindViewHolder(holder: CustomViewHolder, position: Int) {
    //TODO
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): CustomViewHolder {
    val layoutInflater = LayoutInflater.from(parent?.context)
        val songCell = layoutInflater.inflate(R.layout.song_row,parent,false)
        return CustomViewHolder(songCell)
    }
}

class CustomViewHolder(view: View) : RecyclerView.ViewHolder(view)
{

}