package cs309.selectunes

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity

class LoginActivity : AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.login_menu)
        val button = findViewById<Button>(R.id.login_button)
        button.setOnClickListener {
//            val queue = Volley.newRequestQueue(this)
//            val json = JSONObject()
//            json.put("phoneNumber", intent.extras?.get("phone") as String)
//            json.put("username", intent.extras?.get("username") as String)
//            val authRequest = JsonObjectRequest(Request.Method.POST, "localhost:8080", json, Response.Listener {
//                println(it)
//            }, Response.ErrorListener {
//                println("There was an error with the response.")
//            })
//            //queue.add(authRequest)
            startActivity(Intent(this, HostMenuActivity::class.java))
        }
    }
}