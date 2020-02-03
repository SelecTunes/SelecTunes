package cs309.selectunes

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity

class HostMenuActivity : AppCompatActivity()
{
    override fun onCreate(instanceState: Bundle?)
    {
        super.onCreate(instanceState)
        setContentView(R.layout.host_menu_main)
        val guestList = findViewById<Button>(R.id.guestList_id)
        val songList = findViewById<Button>(R.id.songQueue_id)

        guestList.setOnClickListener{
            val toGuestList = Intent(this, GuestListActivity::class.java)
            startActivity(toGuestList)
        }

        songList.setOnClickListener{
            val toSongList = Intent(this, SongListActivity::class.java)
            startActivity(toSongList)
        }
    }
}