package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import cs309.selectunes.GuestRecyclerViewAdapter
import cs309.selectunes.R
import cs309.selectunes.models.Guest
import cs309.selectunes.services.ServerServiceImpl

/**
 * The guest list activity allows people
 * to see who is currently in the party.
 * @author Joshua Edwards
 */
class GuestListActivity : AppCompatActivity()
{
    var guests = ArrayList<Guest>()


    override fun onCreate(instanceState: Bundle?) {


        ServerServiceImpl().getGuestList(this)

        super.onCreate(instanceState)
        setContentView(R.layout.guest_list_menu)


        val returnButton = findViewById<Button>(R.id.return_id)
        returnButton.setOnClickListener{
            val backOut = Intent(this, HostMenuActivity::class.java)
            startActivity(backOut)
        }

        val recycler = findViewById<RecyclerView>(R.id.guest_recycler)
        recycler.layoutManager = LinearLayoutManager(this)
        val adapter = GuestRecyclerViewAdapter(guests, this, this)

        recycler.adapter = adapter
    }
}