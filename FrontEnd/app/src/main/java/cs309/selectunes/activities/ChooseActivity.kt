package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import com.spotify.sdk.android.authentication.AuthenticationClient
import com.spotify.sdk.android.authentication.AuthenticationResponse
import com.spotify.sdk.android.authentication.LoginActivity.REQUEST_CODE
import cs309.selectunes.R
import cs309.selectunes.services.ServerServiceImpl
import cs309.selectunes.utils.HttpUtils
import cs309.selectunes.utils.SpotifyUtils

/**
 * The choose activity allows you to choose
 * between joining a party and creating one.
 * @author Jack Goldsworth
 */
class ChooseActivity : AppCompatActivity() {

    private val serverService = ServerServiceImpl()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.choose_menu)

        val createButton = findViewById<Button>(R.id.create_party_button)
        val joinButton = findViewById<Button>(R.id.join_party_button)

        createButton.setOnClickListener {
            HttpUtils.endParty(this, null)
            SpotifyUtils.login(this)
        }

        joinButton.setOnClickListener {
            startActivity(Intent(this, JoinPartyActivity::class.java))
        }
    }

    public override fun onActivityResult(requestCode: Int, resultCode: Int, intent: Intent?) {
        super.onActivityResult(requestCode, resultCode, intent)
        if (requestCode == REQUEST_CODE) {
            val response = AuthenticationClient.getResponse(resultCode, intent)
            if (response.type == AuthenticationResponse.Type.CODE) {
                serverService.createParty(response.code, this)
            } else if (response.type == AuthenticationResponse.Type.ERROR) {
                println("There was an error while logging into Spotify")
            }
        }
    }
}
