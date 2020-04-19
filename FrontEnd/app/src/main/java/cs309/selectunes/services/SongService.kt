package cs309.selectunes.services

import androidx.appcompat.app.AppCompatActivity
import com.microsoft.signalr.HubConnection
import cs309.selectunes.activities.SongQueueActivity
import cs309.selectunes.activities.SongSearchActivity
import cs309.selectunes.models.Song

/**
 * Service to interact with the back-end
 * for song related requests.
 * @author Jack Goldsworth
 */
interface SongService {

    /**
     * This makes a request to the backend to
     * filter the songs based on dirty words.
     * @param activity activity this is used in.
     */
    fun makeSongsExplicit(activity: AppCompatActivity)

    /**
     * Makes a request to see if the songs are in explicit
     * only mode or not.
     * @param activity activity this is used in.
     */
    fun isExplicit(activity: AppCompatActivity)

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
            activity: SongQueueActivity,
            socket: HubConnection?,
            votes: Map<String, Int>?
    )
}