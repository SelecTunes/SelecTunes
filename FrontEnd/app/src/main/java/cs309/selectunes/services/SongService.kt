package cs309.selectunes.services

import androidx.appcompat.app.AppCompatActivity

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
}