package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import cs309.selectunes.R
import cs309.selectunes.utils.HttpUtils
import org.json.JSONObject
import java.nio.charset.StandardCharsets

/**
 * The join party activity is what users see
 * if they choose the join party option in the
 * choose activity. This includes text input
 * where they can input a party code.
 * @author Jack Goldsworth
 */
class JoinPartyActivity : AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.join_party_menu)
        val button = findViewById<Button>(R.id.join_button)
        val code = findViewById<TextView>(R.id.join_code)
        button.setOnClickListener {
            joinParty(code.text.toString())
        }
    }

    private fun joinParty(code: String) {
        val json = JSONObject()
        json.put("joinCode", code)
        val jsonObjectRequest = object : JsonObjectRequest(Method.POST,
            "https://coms-309-jr-2.cs.iastate.edu/api/Party/JoinParty",
            json,
            Response.Listener {
                startActivity(Intent(this, GuestMenuActivity::class.java))
            },
            Response.ErrorListener {
                println("Error joining party: ${it.networkResponse.statusCode}")
                println(it.networkResponse.data.toString(StandardCharsets.UTF_8))
            }) {

            override fun getHeaders(): Map<String, String> {
                val headers: MutableMap<String, String> = java.util.HashMap()
                headers["Content-Type"] = "application/json"
                headers["Accept"] = "application/json, text/json"
                return headers
            }
        }
        val requestQueue = Volley.newRequestQueue(this, HttpUtils.createAuthCookie(this))
        requestQueue.add(jsonObjectRequest)
    }
}