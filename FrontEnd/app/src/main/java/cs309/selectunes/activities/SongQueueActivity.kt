package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import com.microsoft.signalr.HubConnectionBuilder
import cs309.selectunes.R
import cs309.selectunes.services.SongServiceImpl


/**
 * The song list activity is where
 * users can see a list of the songs
 * currently in the queue.
 * @author Joshua Edwards
 * @author Jack Goldsworth
 */
class SongQueueActivity : AppCompatActivity() {

    companion object {
        var votes = HashMap<String, Int>()
        var songsVotedOn = HashMap<String, String>()
    }

    private val songService = SongServiceImpl()

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
        val url = "http://coms-309-jr-2.cs.iastate.edu/queue"

        val settings = getSharedPreferences("Cookie", 0)
        val hubConnection = HubConnectionBuilder.create(url)
            .withHeader("cookie", "Holtzmann=" + settings.getString("cookie", ""))
            .build()

        println(hubConnection.connectionState.name)

        hubConnection.on("ReceiveUpvote", { id, count ->
            votes[id] = count.toInt()
            songService.getSongQueue(this, hubConnection, votes)
        }, String::class.java, String::class.java)

        hubConnection.on("ReceiveDownvote", { id, count ->
            votes[id] = count.toInt()
            songService.getSongQueue(this, hubConnection, votes)
        }, String::class.java, String::class.java)

        hubConnection.on("ReceiveMoveSongToFront", { id ->
            votes.remove(id)
            songsVotedOn.remove(id)
            songService.getSongQueue(this, hubConnection, votes)
        }, String::class.java)

        hubConnection.on("ReceiveRemoveSong", { id ->
            votes.remove(id)
            songsVotedOn.remove(id)
            songService.getSongQueue(this, hubConnection, votes)
        }, String::class.java)

        hubConnection.start().blockingAwait()
        songService.getSongQueue(this, hubConnection, votes)
    }
}