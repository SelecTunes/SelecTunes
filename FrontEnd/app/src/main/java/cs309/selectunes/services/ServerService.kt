package cs309.selectunes.services

import androidx.appcompat.app.AppCompatActivity
import com.microsoft.signalr.HubConnection
import cs309.selectunes.activities.GuestListActivity
import cs309.selectunes.activities.SongListActivity
import cs309.selectunes.activities.SongSearchActivity
import cs309.selectunes.models.Guest
import cs309.selectunes.models.Song
import org.json.JSONArray

/**
 * General http request methods for the app.
 * @author Jack Goldsworth
 */
interface ServerService {

    /**
     * Http requests to create a party and receive a join code.
     * @param auth Spotify auth token.
     * @param activity activity this method is called from.
     */
    fun createParty(auth: String, activity: AppCompatActivity)

    /**
     * Http request to search a song and receive a list of songs.
     * @param songToSearch song name.
     * @param activity activity this method is called from.
     */
    fun searchSong(songToSearch: String, activity: SongSearchActivity)

    /**
     * Http request to add a song to the queue, where it can
     * then be voted on.
     * @param song Song to add.
     * @param activity activity this method is called from.
     */
    fun addSongToQueue(song: Song, activity: AppCompatActivity)

    /**
     * This gets the current song queue.
     * @param activity activity this method is called from.
     * @param socket socket connection to the backend.
     * @param votes map of votes by id to number of votes.
     */
    fun getSongQueue(
        activity: SongListActivity,
        socket: HubConnection,
        votes: Map<String, Int>
    )

    fun getGuestList(activity: AppCompatActivity)

    fun parseGuests(givenJSON: JSONArray) : ArrayList<Guest>
}