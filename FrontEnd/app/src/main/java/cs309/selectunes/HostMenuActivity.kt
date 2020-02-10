package cs309.selectunes

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.app.AppCompatDelegate


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
            val toSongList = Intent(this, SongListActivity::class.java)
            startActivity(toSongList)
        }

        backArrow.setOnClickListener{
            startActivity(Intent(this, LoginActivity::class.java))
        }

        endParty.setOnClickListener{
            startActivity(Intent(this, LoginActivity::class.java))
        }
    }

    override fun onStart() {
        super.onStart()
        val textView = findViewById<TextView>(R.id.session_id)
        val source = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789"
        val randomString = (1..6)
                .map { i -> kotlin.random.Random.nextInt(0, source.length) }
                .map(source::get)
                .joinToString("")
        textView.text = randomString
    }
}