package cs309.selectunes

import android.os.Bundle
import android.os.PersistableBundle
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import org.json.JSONObject

class LoginActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?, persistentState: PersistableBundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.login)
        val queue = Volley.newRequestQueue(this)
        val json = JSONObject()
        json.put("phoneNumber", intent.extras?.get("phone") as String)
        json.put("username", intent.extras?.get("username") as String)
        val authRequest = JsonObjectRequest(Request.Method.POST, "localhost:8080", json, Response.Listener {
            println(it)
        }, Response.ErrorListener {
            println("There was an error with the response.")
        })
        queue.add(authRequest)
    }
}