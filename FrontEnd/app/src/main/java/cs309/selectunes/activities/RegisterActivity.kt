package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Response
import com.android.volley.toolbox.StringRequest
import com.android.volley.toolbox.Volley
import cs309.selectunes.R
import cs309.selectunes.utils.JsonUtils
import cs309.selectunes.utils.NukeSSLCerts

class RegisterActivity: AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.register_menu)
        NukeSSLCerts.nuke()
        val email = findViewById<TextView>(R.id.register_email)
        val password = findViewById<TextView>(R.id.register_password)
        val confirmPassword = findViewById<TextView>(R.id.register_confirm_password)
        val button = findViewById<Button>(R.id.register_button)

        button.setOnClickListener {
            register(email.text.toString(), password.text.toString(), confirmPassword.text.toString())
        }
    }

    private fun register(email: String, password: String, passwordConfirmed: String) {
        val stringRequest = object: StringRequest(Method.POST, "https://coms-309-jr-2.cs.iastate.edu/api/auth/register", Response.Listener {
            val success = JsonUtils.parseRegisterResponse(this, it, 200)
            if (success) startActivity(Intent(this, LoginActivity::class.java))
            println(it)
        }, Response.ErrorListener {
            JsonUtils.parseRegisterResponse(this, null, it.networkResponse.statusCode)
            println("There was an error with the response. Code: ${it.networkResponse.statusCode}")
        }) {
            override fun getParams(): Map<String, String> {
                val params: MutableMap<String, String> = HashMap()
                params["Email"] = email
                params["Password"] = password
                params["ConfirmPassword"] = passwordConfirmed
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