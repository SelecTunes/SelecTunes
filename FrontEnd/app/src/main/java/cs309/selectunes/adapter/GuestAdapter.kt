package cs309.selectunes.adapter

import android.annotation.SuppressLint
import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.TextView
import cs309.selectunes.R
import cs309.selectunes.activities.GuestListActivity
import cs309.selectunes.models.Guest
import cs309.selectunes.services.PartyServiceImpl

class GuestAdapter(
    context: Context,
    private val guestList: List<Guest>,
    private val act: GuestListActivity,
    private val isGuest: Boolean
) : ArrayAdapter<String>(context, R.layout.guest_list_menu, guestList as List<String>) {

    @SuppressLint("ViewHolder")
    override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {
        val layoutInflater =
            context.applicationContext.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater
        val row = layoutInflater.inflate(R.layout.guest_row, parent, false)
        val email = row.findViewById<TextView>(R.id.email)
        val kickUser = row.findViewById<Button>(R.id.kick_the_nark)
        email.text = guestList[position].givenEmail

        if (isGuest) {
            kickUser.visibility = View.GONE
        }

        kickUser.setOnClickListener {
            PartyServiceImpl().kickGuest(guestList[position].givenEmail, act)
        }
        return row
    }
}