package cs309.selectunes.activities

import android.os.Bundle
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import cs309.selectunes.services.AuthServiceImpl
import cs309.selectunes.utils.NukeSSLCerts

class RegisterActivity : AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.register_menu)
        NukeSSLCerts.nuke()
        val email = findViewById<TextView>(R.id.register_email)
        val password = findViewById<TextView>(R.id.register_password)
        val confirmPassword = findViewById<TextView>(R.id.register_confirm_password)
        val button = findViewById<Button>(R.id.register_button)

        button.setOnClickListener {
            AuthServiceImpl().register(email.text.toString(), password.text.toString(), confirmPassword.text.toString(), this)
        }
    }
}