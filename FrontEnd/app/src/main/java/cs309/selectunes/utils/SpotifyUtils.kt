package cs309.selectunes.utils

import androidx.appcompat.app.AppCompatActivity
import com.spotify.sdk.android.authentication.AuthenticationClient
import com.spotify.sdk.android.authentication.AuthenticationRequest
import com.spotify.sdk.android.authentication.AuthenticationResponse

/**
 * General Spotify based utility methods.
 * @author Jack Goldsworth
 */
object SpotifyUtils {

    private const val REQUEST_CODE = 1138
    private const val redirect = "https://coms-309-jr-2.cs.iastate.edu/api/auth/callback"
    private const val client = "cadb1b4323ac428fa153e815a7277dc6"
    private val scopes = StringBuilder()
            .append("user-read-private")
            .append(" user-read-email")
            .append(" user-read-playback-state")
            .append(" user-modify-playback-state")
            .append(" user-read-currently-playing")
            .append(" streaming")
            .toString()
    private val spotify = AuthenticationRequest.Builder(client, AuthenticationResponse.Type.CODE, redirect)
            .setScopes(arrayOf(scopes))
            .build()

    /**
     * Opens the spotify login page and logs the user in.
     * @param activity activity this method is being called from.
     */
    fun login(activity: AppCompatActivity) {
        AuthenticationClient.openLoginActivity(activity, REQUEST_CODE, spotify)
    }
}