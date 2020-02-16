package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.Switch
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.app.AppCompatDelegate
import com.android.volley.NetworkResponse
import com.android.volley.Response
import com.android.volley.toolbox.StringRequest
import com.android.volley.toolbox.Volley
import cs309.selectunes.R
import cs309.selectunes.utils.JsonUtils
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
                val success = JsonUtils.parseLoginResponse(this, it)
                if (success) startActivity(Intent(this, ChooseActivity::class.java))
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

            override fun parseNetworkResponse(response: NetworkResponse?): Response<String> {
                val responseHeaders = response?.headers
                val rawCookies = responseHeaders?.get("Set-Cookie")
                if (rawCookies != null) {
                    println("Cookie Before: $rawCookies")
                    val search = "Holtzmann="
                    val cookie = rawCookies.substring(rawCookies.indexOf(search) + search.length, rawCookies.indexOf(';'))
                    val settings = getSharedPreferences("Cookie", 0)
                    settings.edit().putString("cookie", cookie).apply()
                }
                return super.parseNetworkResponse(response)
            }
        }
        val requestQueue = Volley.newRequestQueue(this)
        requestQueue.add(stringRequest)
    }
}