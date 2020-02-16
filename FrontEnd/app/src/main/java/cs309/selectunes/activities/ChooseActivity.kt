package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
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
import java.nio.charset.StandardCharsets


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
                println(response.code)
                createParty(response.code)
                startActivity(Intent(this, HostMenuActivity::class.java))
            }
            else if (response.type == AuthenticationResponse.Type.ERROR) {
                println("There was an error while logging into Spotify")
            }
        }
    }

    private fun createParty(auth: String): String? {
        val url = StringBuilder("https://coms-309-jr-2.cs.iastate.edu/api/Auth/Callback")
                .append("?Code=")
                .append(auth)
        val settings = getSharedPreferences("Cookie", 0)
        val httpclient = DefaultHttpClient()
        val cookieStore = BasicCookieStore()
        cookieStore.addCookie(BasicClientCookie("Holtzmann", settings.getString("cookie", "")))
        println("Cookie BOI: ${settings.getString("cookie", "")}")
        httpclient.cookieStore = cookieStore
        val httpStack = HttpClientStack(httpclient)

        val stringRequest = object : JsonObjectRequest(Method.GET, url.toString(), null, Response.Listener {
            println(it)
        }, Response.ErrorListener {
            println(it.networkResponse.data.toString(StandardCharsets.UTF_8))
            println("There was an error with the create party response. Code: ${it.networkResponse.statusCode}")
        }) {
//            override fun getParams(): Map<String, String> {
//                val params: MutableMap<String, String> = HashMap()
//                params["Code"] = auth
//                return params
//            }

            override fun getHeaders(): Map<String, String> {
                val headers: MutableMap<String, String> = HashMap()
                headers["Accept"] = "application/json, text/json"
                return headers
            }
        }
        val requestQueue = Volley.newRequestQueue(this, httpStack)
        requestQueue.add(stringRequest)
        return null
    }

}
