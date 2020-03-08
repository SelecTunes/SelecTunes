package cs309.selectunes

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.Switch
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import cs309.selectunes.models.Guest


class GuestRecyclerViewAdapter(private val guests: ArrayList<Guest>,
                               val context : Context): RecyclerView.Adapter<GuestViewHolder>()
{
    override fun getItemCount(): Int
    {
        return guests.size
    }

    override fun onBindViewHolder(holder: GuestViewHolder, position: Int)
    {
        val thisGuest: Guest = guests[position]
        holder.bind(thisGuest)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): GuestViewHolder {
        val layoutInflater = LayoutInflater.from(parent.context)
        val guestCell = layoutInflater.inflate(R.layout.guest_row,parent,false)
        return GuestViewHolder(guestCell)
    }
}

class GuestViewHolder(view: View) : RecyclerView.ViewHolder(view)
{

    private val email = view.findViewById<TextView>(R.id.email)
    private val modBool = view.findViewById<Switch>(R.id.songName)
    private var kickUser = view.findViewById<Button>(R.id.kick_the_nark)

    init {
        kickUser.setOnClickListener {

        }
        //modBool.setOnCheckedChangeListener(view, )
    }

    fun bind(givenGuest: Guest)
    {
        email.text = givenGuest.email
    }
}