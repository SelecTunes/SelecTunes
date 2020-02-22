package cs309.selectunes

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import cs309.selectunes.models.Guest
import kotlinx.android.synthetic.main.guest_row.view.*


class GuestRecyclerViewAdapter(private val guests: ArrayList<Guest>,
                               val context : Context): RecyclerView.Adapter<CustomViewHolder>()
{
    override fun getItemCount(): Int
    {
        return guests.size
    }

    override fun onBindViewHolder(holder: CustomViewHolder, position: Int)
    {
        val thisGuest: Guest = guests[position]
        holder.view.phoneNumber.text = thisGuest.phoneNumber

    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): CustomViewHolder {
        val layoutInflater = LayoutInflater.from(parent.context)
        val guestCell = layoutInflater.inflate(R.layout.guest_row,parent,false)
        return CustomViewHolder(guestCell)
    }
}

class GuestViewHolder(val view: View) : RecyclerView.ViewHolder(view)
{

}