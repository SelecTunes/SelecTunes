package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import cs309.selectunes.utils.HttpUtils

class GuestMenuActivity : AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.guest_menu)
        val songSearch = findViewById<Button>(R.id.song_search)
        val partyMembers = findViewById<Button>(R.id.party_members)
        val leaveParty = findViewById<Button>(R.id.leave_party)
        val backArrow = findViewById<Button>(R.id.back_arrow_guest)

        leaveParty.setOnClickListener {
            HttpUtils.leaveParty(this, LoginActivity::class.java)
        }

        backArrow.setOnClickListener {
            HttpUtils.leaveParty(this, JoinPartyActivity::class.java)
        }

        songSearch.setOnClickListener {
            val intent = Intent(this, SongSearchActivity::class.java)
            intent.putExtra("previousActivity", "guest")
            startActivity(intent)
        }
    }
}