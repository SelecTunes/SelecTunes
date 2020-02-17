package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.HttpClientStack
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import com.spotify.sdk.android.authentication.AuthenticationClient
import com.spotify.sdk.android.authentication.AuthenticationResponse
import com.spotify.sdk.android.authentication.LoginActivity.REQUEST_CODE
import cs309.selectunes.R
import cs309.selectunes.utils.SpotifyUtils
import org.apache.http.impl.client.BasicCookieStore
import org.apache.http.impl.client.DefaultHttpClient
import org.apache.http.impl.cookie.BasicClientCookie


class ChooseActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_choose)

        val createButton = findViewById<Button>(R.id.create_party_button)
        val joinButton = findViewById<Button>(R.id.join_party_button)

        createButton.setOnClickListener {
            SpotifyUtils.login(this)
        }

        joinButton.setOnClickListener {
        }
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, intent: Intent?) {
        super.onActivityResult(requestCode, resultCode, intent)
        if (requestCode == REQUEST_CODE) {
            val response = AuthenticationClient.getResponse(resultCode, intent)
            if (response.type == AuthenticationResponse.Type.CODE) {
                createParty()
            }
            else if (response.type == AuthenticationResponse.Type.ERROR) {
                println("There was an error while logging into Spotify")
            }
        }
    }

    private fun createParty() {
        val httpclient = DefaultHttpClient()
        val cookieStore = BasicCookieStore()
        val settings = getSharedPreferences("Cookie", 0)
        val cookie = BasicClientCookie("Holtzmann", settings.getString("cookie", ""))
        cookie.domain = "coms-309-jr-2.cs.iastate.edu"
        cookieStore.addCookie(cookie)
        httpclient.cookieStore = cookieStore
        val httpStack = HttpClientStack(httpclient)

        val stringRequest = JsonObjectRequest(Request.Method.GET, "https://coms-309-jr-2.cs.iastate.edu/api/Auth/oauthisdumb", null, Response.Listener {
            val joinCode = it.getString("joinCode")
            val hostIntent = Intent(this, HostMenuActivity::class.java)
            hostIntent.putExtra("code", joinCode)
            startActivity(hostIntent)
        }, Response.ErrorListener {
            println("There was an error with the create party response. Code: ${it.networkResponse.statusCode}")
        })
        val requestQueue = Volley.newRequestQueue(this, httpStack)
        requestQueue.add(stringRequest)
    }

}
