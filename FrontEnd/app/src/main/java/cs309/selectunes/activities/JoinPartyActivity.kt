package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import cs309.selectunes.R
import cs309.selectunes.utils.HttpUtils
import org.json.JSONObject

class JoinPartyActivity : AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.login_menu)
        val button = findViewById<Button>(R.id.join_button)
        val code = findViewById<TextView>(R.id.join_code)
        button.setOnClickListener {
            joinParty(code.text.toString())
        }
    }

    private fun joinParty(code: String) {
        val json = JSONObject()
        json.put("joinCode", code)
        val joinPartyRequest = JsonObjectRequest(Request.Method.POST, "https://coms-309-jr-2.cs.iastate.edu/api/Party/JoinParty", json, Response.Listener {
            startActivity(Intent(this, GuestMenu::class.java))
        }, Response.ErrorListener {
            println("There was an error with the join party response. Code: ${it.networkResponse.statusCode}")
        })
        val requestQueue = Volley.newRequestQueue(this, HttpUtils.createAuthCookie(this))
        requestQueue.add(joinPartyRequest)
    }
}