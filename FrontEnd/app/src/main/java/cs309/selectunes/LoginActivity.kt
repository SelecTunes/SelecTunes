package cs309.selectunes

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.Switch
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.app.AppCompatDelegate
import com.android.volley.Response
import com.android.volley.toolbox.StringRequest
import com.android.volley.toolbox.Volley
import cs309.selectunes.utils.NukeSSLCerts

//https://colorhunt.co/palette/69667
//https://colorhunt.co/palette/2763
class LoginActivity : AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.login_menu)
        NukeSSLCerts.nuke()
        val email = findViewById<TextView>(R.id.login_email)
        val password = findViewById<TextView>(R.id.login_password)
        val button = findViewById<Button>(R.id.register_button)
        val darkSlider = findViewById<Switch>(R.id.dark_mode)
        val register = findViewById<TextView>(R.id.register)

        val settings = getSharedPreferences("UserInfo", 0)
        darkSlider.isChecked = settings.getBoolean("dark_mode", false)
        checkDarkMode(darkSlider.isChecked)
        darkSlider.setOnCheckedChangeListener { buttonView, isChecked ->
            checkDarkMode(isChecked)
            recreate()
            val editor = settings.edit()
            editor.putBoolean("dark_mode", isChecked)
            editor.apply()
        }

        register.setOnClickListener {
            startActivity(Intent(this, RegisterActivity::class.java))
        }

        button.setOnClickListener {
            login(email.text.toString(), password.text.toString())
        }
    }

    private fun checkDarkMode(isChecked: Boolean) {
        if(isChecked) {
            AppCompatDelegate.setDefaultNightMode(AppCompatDelegate.MODE_NIGHT_YES)
        } else {
            AppCompatDelegate.setDefaultNightMode(AppCompatDelegate.MODE_NIGHT_NO)
        }
    }

    private fun login(email: String, password: String) {
        val stringRequest = object: StringRequest(Method.POST, "https://coms-309-jr-2.cs.iastate.edu/api/auth/login", Response.Listener {
            startActivity(Intent(this, ChooseActivity::class.java))
            println(it)
        }, Response.ErrorListener {
            println("There was an error with the response. Code: ${it.networkResponse.statusCode}")
        }) {
            override fun getParams(): Map<String, String> {
                val params: MutableMap<String, String> = HashMap()
                params["Email"] = email
                params["Password"] = password
                params["ConfirmPassword"] = password
                return params
            }

            override fun getHeaders(): Map<String, String> {
                val headers: MutableMap<String, String> = HashMap()
                headers["Content-Type"] = "application/x-www-form-urlencoded"
                headers["Accept"] = "application/json, text/json"
                return headers
            }
        }
        val requestQueue = Volley.newRequestQueue(this)
        requestQueue.add(stringRequest)
    }
}