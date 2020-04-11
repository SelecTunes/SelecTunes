package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import com.microsoft.signalr.HubConnectionBuilder
import cs309.selectunes.R
import cs309.selectunes.services.ServerServiceImpl
import cs309.selectunes.utils.NukeSSLCerts


/**
 * The song list activity is where
 * users can see a list of the songs
 * currently in the queue.
 * @author Joshua Edwards
 * @author Jack Goldsworth
 */
class SongListActivity : AppCompatActivity() {

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

    override fun onStart() {
        super.onStart()
        val url = "https://coms-309-jr-2.cs.iastate.edu/api/Song/Queue"

        val hubConnection = HubConnectionBuilder.create(url).build()

        hubConnection.on("ReceiveUpvote", { message: String ->
            println(message)
        }, String::class.java)

        hubConnection.on("ReceiveDownvote", { message: String ->
            println(message)
        }, String::class.java)
        NukeSSLCerts.nuke()
        hubConnection.start().blockingAwait()
        ServerServiceImpl().getSongQueue(this, hubConnection)
    }
}