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

class GuestListActivity : AppCompatActivity()
{
    var guests = ArrayList<Guest>()



    override fun onCreate(instanceState: Bundle?)
    {


        var guest1 = Guest("Reggie Bush")
        var guest2 = Guest("George Bush")
        var guest3 = Guest("Busch Light")
        var guest4 = Guest("Large Bush")
        var guest5 = Guest("Bushy Bush")
        guests.add(guest1)
        guests.add(guest2)
        guests.add(guest3)
        guests.add(guest4)
        guests.add(guest5)

        super.onCreate(instanceState)
        setContentView(R.layout.guest_list_menu)


        val returnButton = findViewById<Button>(R.id.return_id)
        returnButton.setOnClickListener{
            val backOut = Intent(this, HostMenuActivity::class.java)
            startActivity(backOut)
        }

        val recycler = findViewById<RecyclerView>(R.id.guest_recycler)
        recycler.setLayoutManager(LinearLayoutManager(this))
        val adapter = GuestRecyclerViewAdapter(guests, this)

        recycler.adapter = adapter
    }
}