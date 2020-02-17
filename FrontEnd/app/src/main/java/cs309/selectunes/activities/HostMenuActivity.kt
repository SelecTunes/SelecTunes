package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R


class HostMenuActivity : AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.test_host_menu)

        val guestList = findViewById<Button>(R.id.guest_list)
        val songList = findViewById<Button>(R.id.song_list)
        val backArrow = findViewById<Button>(R.id.back_arrow)
        val endParty = findViewById<Button>(R.id.end_party)
        val moderators = findViewById<Button>(R.id.moderators)
        val explicit = findViewById<Button>(R.id.explicit)

        guestList.setOnClickListener{
            val toGuestList = Intent(this, GuestListActivity::class.java)
            startActivity(toGuestList)
        }

        songList.setOnClickListener{
            val toSongList = Intent(this, TempHostMenuActivity::class.java)
            startActivity(toSongList)
        }

        backArrow.setOnClickListener{
            //TODO: Actually end party.
            startActivity(Intent(this, ChooseActivity::class.java))
        }

        endParty.setOnClickListener{
            //TODO: Actually end party.
            startActivity(Intent(this, LoginActivity::class.java))
        }
    }

    override fun onStart() {
        super.onStart()
        val textView = findViewById<TextView>(R.id.session_id)
        textView.text = intent.getStringExtra("code")
    }
}