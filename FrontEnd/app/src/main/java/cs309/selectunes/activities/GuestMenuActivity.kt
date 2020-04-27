package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import cs309.selectunes.services.PartyServiceImpl
import cs309.selectunes.utils.SpotifyUtils

/**
 * The guest menu activity is the view
 * that guest see after joining a party.
 * @author Jack Goldsworth
 */
class GuestMenuActivity : AppCompatActivity() {

    private val partyService = PartyServiceImpl()

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.guest_menu)
        val songSearch = findViewById<Button>(R.id.song_search)
        val partyMembers = findViewById<Button>(R.id.party_members)
        val leaveParty = findViewById<Button>(R.id.leave_party)
        val backArrow = findViewById<Button>(R.id.back_arrow_guest)
        val queue = findViewById<Button>(R.id.guest_current_queue)

        queue.setOnClickListener {
            val intent = Intent(this, SongQueueActivity::class.java)
            intent.putExtra("previousActivity", "guest")
            startActivity(intent)
        }

        leaveParty.setOnClickListener {
            partyService.leaveParty(this, ChooseActivity::class.java)
        }

        backArrow.setOnClickListener {
            partyService.leaveParty(this, JoinPartyActivity::class.java)
        }

        songSearch.setOnClickListener {
            val intent = Intent(this, SongSearchActivity::class.java)
            intent.putExtra("previousActivity", "guest")
            startActivity(intent)
        }

        partyMembers.setOnClickListener {
            val intent = Intent(this, GuestListActivity::class.java)
            intent.putExtra("previousActivity", "guest")
            intent.putExtra("isGuest", true)
            startActivity(intent)
        }
    }

    override fun onStart() {
        super.onStart()
        SpotifyUtils.createSpotifySocket(this)
    }
}