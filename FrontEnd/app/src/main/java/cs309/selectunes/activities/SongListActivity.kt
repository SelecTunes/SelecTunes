package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import cs309.selectunes.services.ServerServiceImpl
import java.net.Socket

/**
 * The song list activity is where
 * users can see a list of the songs
 * currently in the queue.
 * @author Joshua Edwards
 * @author Jack Goldsworth
 */
class SongListActivity : AppCompatActivity() {

    var socket: Socket? = null

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.song_queue_menu)

        val backArrow = findViewById<Button>(R.id.back_arrow_song_queue)

        //createSocket()

        backArrow.setOnClickListener {
            if (intent.getStringExtra("previousActivity") == "host")
                startActivity(Intent(this, HostMenuActivity::class.java))
            else
                startActivity(Intent(this, GuestMenuActivity::class.java))
        }
    }

    override fun onStart() {
        super.onStart()
        println("Starting")
        ServerServiceImpl().getSongQueue(this)
    }

    /**
     * This function connects to the web-socket that
     * allows the user to receive the current up-votes and
     * down-votes.
     */
    private fun createSocket() {
        if (socket == null) {
            socket = Socket("https://coms-309-jr-2.cs.iastate.edu/api/Song/Queue", 443)
        }
    }

}