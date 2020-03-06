package cs309.selectunes.services

import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.activities.SongSearchActivity

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
}