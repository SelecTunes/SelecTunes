package cs309.selectunes.adapter

import android.annotation.SuppressLint
import android.content.Context
import android.graphics.BitmapFactory
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.ImageView
import android.widget.TextView
import androidx.core.content.ContextCompat
import androidx.core.view.isVisible
import com.microsoft.signalr.HubConnection
import cs309.selectunes.R
import cs309.selectunes.activities.SongListActivity
import cs309.selectunes.models.Song
import cs309.selectunes.utils.BitmapCache
import java.net.URL

/**
 * This is a custom adapter for the song queue.
 * list. It allows each song to get it's own image,
 * name, and artist name in the list.
 * @author Jack Goldsworth
 */
class QueueAdapter(
    private val ctx: Context,
    private val songList: List<Song>,
    private val socket: HubConnection,
    private val votes: Map<String, Int>
) : ArrayAdapter<String>(ctx, R.layout.song_queue_menu, songList as List<String>) {

    @SuppressLint("ViewHolder")
    override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {
        val layoutInflater =
            ctx.applicationContext.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater
        val row = layoutInflater.inflate(R.layout.queue_song_row, parent, false)
        val artist = row.findViewById<TextView>(R.id.queue_artistName)
        val song = row.findViewById<TextView>(R.id.queue_songName)
        val image = row.findViewById<ImageView>(R.id.queue_album_cover)
        val upvoteButton = row.findViewById<Button>(R.id.upvote_button)
        val downvoteButton = row.findViewById<Button>(R.id.downvote_button)
        val songVoteTotal = row.findViewById<TextView>(R.id.song_vote_total)
        val songId = songList[position].id
        val votedSongs = SongListActivity.songsVotedOn

        // Hide song vote options if the song is not voteable.
        if(!songList[position].voteable!!) {
            upvoteButton.visibility = View.GONE
            downvoteButton.visibility = View.GONE
            songVoteTotal.visibility = View.GONE
        }

        // Keep upvote and downvote icons on reload.
        if(votedSongs.contains(songId)) {
            if(votedSongs[songId] == "UP") {
                upvoteButton.setBackgroundResource(R.drawable.thumbs_up_click)
            } else {
                downvoteButton.setBackgroundResource(R.drawable.thumbs_down_click)
            }
        }

        println(votes[songId])

        songVoteTotal.text = votes[songId]?.toString() ?: "0"

        upvoteButton.setOnClickListener {
            if(!votedSongs.contains(songId)) {
                socket.send("UpvoteSong", songId)
                songVoteTotal.text = votes[songId]?.plus(1)?.toString() ?: "1"
                upvoteButton.setBackgroundResource(R.drawable.thumbs_up_click)
                SongListActivity.songsVotedOn[songId] = "UP"
            } else if(votedSongs.contains(songId) && votedSongs[songId] == "UP") {
                socket.send("DownvoteSong", songId)
                songVoteTotal.text = votes[songId]?.minus(1)?.toString() ?: "0"
                upvoteButton.setBackgroundResource(R.drawable.thumbs_up_not)
                SongListActivity.songsVotedOn.remove(songId)
            }
        }

        downvoteButton.setOnClickListener {
            if(!SongListActivity.songsVotedOn.contains(songId)) {
                socket.send("DownvoteSong", songId)
                songVoteTotal.text = votes[songId]?.minus(1)?.toString() ?: "-1"
                downvoteButton.setBackgroundResource(R.drawable.thumbs_down_click)
                SongListActivity.songsVotedOn[songId] = "DOWN"
            } else if(votedSongs.contains(songId) && votedSongs[songId] == "DOWN") {
                socket.send("UpvoteSong", songId)
                songVoteTotal.text = votes[songId]?.plus(1)?.toString() ?: "0"
                downvoteButton.setBackgroundResource(R.drawable.thumbs_down_not)
                SongListActivity.songsVotedOn.remove(songId)
            }
        }

        artist.text = "By: ${songList[position].artistName}"
        song.text = songList[position].songName
        var bitmap = BitmapCache.loadBitmap(songId)
        if (bitmap == null) {
            val url = URL(songList[position].albumArt)
            bitmap = BitmapFactory.decodeStream(url.openConnection().getInputStream())
            BitmapCache.store(songId, bitmap)
        }
        image.setImageBitmap(bitmap)
        return row
    }
}