package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import cs309.selectunes.models.Guest

class GuestListActivity : AppCompatActivity()
{
    var guests = ArrayList<Guest>()



    override fun onCreate(instanceState: Bundle?)
    {


        super.onCreate(instanceState)
        setContentView(R.layout.guest_list_layout)

        //var listView  = findViewById<ListView>(R.id.guest_listView)

        val returnButton = findViewById<Button>(R.id.return_id)
        returnButton.setOnClickListener{
            val backOut = Intent(this, HostMenuActivity::class.java)
            startActivity(backOut)
        }
    }
}