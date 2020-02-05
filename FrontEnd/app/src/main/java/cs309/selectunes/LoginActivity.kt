package cs309.selectunes

import android.content.Intent
import android.os.Bundle
import android.os.PersistableBundle
import android.widget.Button
import android.widget.EditText
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import org.json.JSONObject

class LoginActivity : AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.login)
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