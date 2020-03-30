package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import cs309.selectunes.models.Song

/**
 * The song list activity is where
 * users can see a list of the songs
 * currently in the queue.
 * @author Joshua Edwards
 */
class SongListActivity : AppCompatActivity() {
    var songs = ArrayList<Song>()

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.song_queue_menu)

        val backArrow = findViewById<Button>(R.id.back_arrow_song_queue)

        backArrow.setOnClickListener {
            if (intent.getStringExtra("previousActivity") == "host")
                startActivity(Intent(this, HostMenuActivity::class.java))
            else
                startActivity(Intent(this, GuestMenuActivity::class.java))
        }
    }

}