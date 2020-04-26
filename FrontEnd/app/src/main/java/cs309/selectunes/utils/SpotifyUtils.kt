package cs309.selectunes.utils

import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.widget.ImageView
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import com.microsoft.signalr.HubConnection
import com.microsoft.signalr.HubConnectionBuilder
import com.spotify.android.appremote.api.ConnectionParams
import com.spotify.android.appremote.api.Connector
import com.spotify.android.appremote.api.SpotifyAppRemote
import com.spotify.protocol.types.PlayerState
import com.spotify.sdk.android.authentication.AuthenticationClient
import com.spotify.sdk.android.authentication.AuthenticationRequest
import com.spotify.sdk.android.authentication.AuthenticationResponse
import cs309.selectunes.R
import cs309.selectunes.activities.GuestMenuActivity
import cs309.selectunes.activities.HostMenuActivity
import cs309.selectunes.services.SongServiceImpl
import java.net.URL


/**
 * General Spotify based utility methods.
 * @author Jack Goldsworth
 */
object SpotifyUtils {

    private const val REQUEST_CODE = 1138
    private const val redirect = "https://coms-309-jr-2.cs.iastate.edu/api/auth/callback"
    private const val client = "cadb1b4323ac428fa153e815a7277dc6"
    private val connectionParams = ConnectionParams.Builder(client)
            .setRedirectUri(redirect)
            .showAuthView(true)
            .build()
    private val songService = SongServiceImpl()

    private var mSpotifyAppRemote: SpotifyAppRemote? = null
    private var hubConnection: HubConnection? = null
    private var currentSongId: String? = null

    private val scopes = StringBuilder()
            .append("user-read-private")
            .append(" user-read-email")
            .append(" user-read-playback-state")
            .append(" user-modify-playback-state")
            .append(" user-read-currently-playing")
            .append(" streaming")
            .append(" user-modify-playback-state")
            .append(" app-remote-control")
            .toString()

    private val spotify =
            AuthenticationRequest.Builder(client, AuthenticationResponse.Type.CODE, redirect)
                    .setScopes(arrayOf(scopes))
                    .build()

    /**
     * Opens the spotify login page and logs the user in.
     * @param activity activity this method is being called from.
     */
    fun login(activity: AppCompatActivity) {
        AuthenticationClient.openLoginActivity(activity, REQUEST_CODE, spotify)
    }

    fun connectToSpotify(activity: AppCompatActivity) {
        SpotifyAppRemote.connect(activity, connectionParams,
                object : Connector.ConnectionListener {
                    override fun onConnected(spotifyAppRemote: SpotifyAppRemote) {
                        mSpotifyAppRemote = spotifyAppRemote
                        mSpotifyAppRemote!!.playerApi
                                .subscribeToPlayerState()
                                .setEventCallback { playerState: PlayerState ->
                                    val track = playerState.track
                                    if (track != null) {
                                        val trackId = track.uri.replace("spotify:track:", "")
                                        hubConnection!!.send("UpdateCurrentSong", track.imageUri.raw, track.name.toString(), track.artist.name.toString(), trackId)
                                        if (currentSongId != null) {
                                            if (trackId != currentSongId) {
                                                songService.removeLockedSong(trackId, activity)
                                            }
                                        }
                                        currentSongId = trackId
                                    }
                                }
                    }

                    override fun onFailure(throwable: Throwable) {
                        println(throwable.message)
                        println("Connection Failed: ${throwable.stackTrace}")
                    }
                })
    }

    fun createSpotifySocket(activity: AppCompatActivity) {
        val url = "http://coms-309-jr-2.cs.iastate.edu/spotify"

        val settings = activity.getSharedPreferences("Cookie", 0)
        hubConnection = HubConnectionBuilder.create(url)
                .withHeader("cookie", "Holtzmann=" + settings.getString("cookie", ""))
                .build()

        hubConnection!!.on("ReceiveSong", { uri, songName, artistName, trackId ->
            var bitmap = BitmapCache.loadBitmap(trackId)
            println(uri)
            val updatedUri = uri.replace("spotify:image:", "https://i.scdn.co/image/")
            if (bitmap == null) {
                bitmap = BitmapFactory.decodeStream(URL(updatedUri).openConnection().getInputStream())
                BitmapCache.store(trackId, bitmap)
            }
            addSongUI(activity, songName, artistName, bitmap!!)
        }, String::class.java, String::class.java, String::class.java, String::class.java)

        hubConnection!!.start().blockingAwait()
    }

    private fun addSongUI(activity: AppCompatActivity, songName: String, artistName: String, bitmap: Bitmap) {
        activity.runOnUiThread {
            if (activity is HostMenuActivity) {
                activity.findViewById<ImageView>(R.id.host_song_img).setImageBitmap(bitmap)
                activity.findViewById<TextView>(R.id.host_song_name).text = songName
                activity.findViewById<TextView>(R.id.host_song_artist).text = "By $artistName"
            } else if (activity is GuestMenuActivity) {
                activity.findViewById<ImageView>(R.id.guest_song_img).setImageBitmap(bitmap)
                activity.findViewById<TextView>(R.id.host_song_name).text = songName
                activity.findViewById<TextView>(R.id.host_song_artist).text = "By $artistName"
            }
        }
    }
}