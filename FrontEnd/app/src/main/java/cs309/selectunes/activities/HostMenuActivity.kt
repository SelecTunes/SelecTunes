package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import cs309.selectunes.services.SongServiceImpl
import cs309.selectunes.utils.HttpUtils

/**
 * The host menu activity is what a user
 * sees after they choose to create a party.
 * It includes a party join code.
 * @author Jack Goldsworth
 */
class HostMenuActivity : AppCompatActivity() {

    private val songService = SongServiceImpl()

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.host_menu)

        val guestList = findViewById<Button>(R.id.guest_list)
        val songList = findViewById<Button>(R.id.song_list)
        val songSearch = findViewById<Button>(R.id.host_song_search_button)
        val backArrow = findViewById<Button>(R.id.back_arrow)
        val endParty = findViewById<Button>(R.id.end_party)
        val explicit = findViewById<Button>(R.id.explicit)

        guestList.setOnClickListener{
            val intent = Intent(this, GuestListActivity::class.java)
            intent.putExtra("isGuest", false)
            startActivity(intent)
        }

        songList.setOnClickListener{
            val intent = Intent(this, SongListActivity::class.java)
            intent.putExtra("previousActivity", "host")
            startActivity(intent)
        }

        songSearch.setOnClickListener {
            val intent = Intent(this, SongSearchActivity::class.java)
            intent.putExtra("previousActivity", "host")
            startActivity(intent)
        }

        backArrow.setOnClickListener{
            HttpUtils.endParty(this, ChooseActivity::class.java)
        }

        endParty.setOnClickListener{
            HttpUtils.endParty(this, ChooseActivity::class.java)
        }

        explicit.setOnClickListener {
            songService.makeSongsExplicit(this)
        }
    }

    override fun onStart() {
        super.onStart()
        songService.isExplicit(this)
        val joinCode = findViewById<Button>(R.id.join_code_button)
        val settings = getSharedPreferences("PartyInfo", 0)
        joinCode.text = settings.getString("join_code", "Code not found.")
    }
}