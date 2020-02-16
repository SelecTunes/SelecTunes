package cs309.selectunes.utils

import androidx.appcompat.app.AppCompatActivity
import com.spotify.sdk.android.authentication.AuthenticationClient
import com.spotify.sdk.android.authentication.AuthenticationRequest
import com.spotify.sdk.android.authentication.AuthenticationResponse

object SpotifyUtils {

    private const val REQUEST_CODE = 1138
    private const val redirect = "https://coms-309-jr-2.cs.iastate.edu/redirect"
    private const val client = "4b95d3c864544a20a1db87ae2447cc1c"
    private val spotify = AuthenticationRequest.Builder(client, AuthenticationResponse.Type.TOKEN, redirect)
            .setScopes(arrayOf("user-read-private user-read-email streaming user-read-currently-playing user-modify-playback-state user-read-playback-state"))
            .build()

    fun login(activity: AppCompatActivity) {
        AuthenticationClient.openLoginActivity(activity, REQUEST_CODE, spotify)
    }
}