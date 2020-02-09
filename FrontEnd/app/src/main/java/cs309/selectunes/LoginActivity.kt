package cs309.selectunes

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.Switch
import androidx.appcompat.app.AppCompatActivity


class LoginActivity : AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.login_menu)
        val button = findViewById<Button>(R.id.login_button)
        val darkSlider = findViewById<Switch>(R.id.dark_mode)
        darkSlider.setOnCheckedChangeListener { buttonView, isChecked ->
            val settings = getSharedPreferences("UserInfo", 0)
            val editor = settings.edit()
            editor.putBoolean("dark_mode", isChecked)
            editor.apply()
        }

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