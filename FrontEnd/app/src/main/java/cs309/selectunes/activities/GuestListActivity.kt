package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import cs309.selectunes.models.Guest
import cs309.selectunes.services.ServerServiceImpl
import org.json.JSONArray

/**
 * The guest list activity allows people
 * to see who is currently in the party.
 * @author Joshua Edwards
 */
class GuestListActivity : AppCompatActivity() {

    internal val guestList = mutableListOf<Guest>()

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.guest_list_menu)
        val returnButton = findViewById<Button>(R.id.return_id)
        returnButton.setOnClickListener {
            val backOut = Intent(this, HostMenuActivity::class.java)
            startActivity(backOut)
        }
    }

    override fun onStart() {
        super.onStart()
        ServerServiceImpl().getGuestList(this, intent.getBooleanExtra("isGuest", true))
    }

    fun parseGuests(givenJSON: JSONArray): List<Guest> {
        guestList.clear()
        for (x in 0 until givenJSON.length()) {
            val guestObj = givenJSON.getJSONObject(x)
            val guest = Guest(guestObj.getString("email"))
            guestList.add(guest)
        }
        return guestList
    }
}