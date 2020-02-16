package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import com.spotify.sdk.android.authentication.AuthenticationClient
import com.spotify.sdk.android.authentication.AuthenticationResponse
import com.spotify.sdk.android.authentication.LoginActivity.REQUEST_CODE
import cs309.selectunes.R
import cs309.selectunes.utils.SpotifyUtils


class ChooseActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_choose)

        val createButton = findViewById<Button>(R.id.create_party_button)
        val joinButton = findViewById<Button>(R.id.join_party_button)

        createButton.setOnClickListener {
            startActivity(Intent(this, HostMenuActivity::class.java))
        }

        joinButton.setOnClickListener {
            SpotifyUtils.login(this)
        }
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, intent: Intent?) {
        super.onActivityResult(requestCode, resultCode, intent)
        if (requestCode == REQUEST_CODE) {
            val response = AuthenticationClient.getResponse(resultCode, intent)
            when (response.type) {
                AuthenticationResponse.Type.TOKEN -> {
                    println("SPOTIFY RESPONSE: ${response.accessToken}")
                }
                AuthenticationResponse.Type.ERROR -> {
                    println("SPOTIFY ERRORED")
                }
                else -> {
                }
            }
        }
    }

}
