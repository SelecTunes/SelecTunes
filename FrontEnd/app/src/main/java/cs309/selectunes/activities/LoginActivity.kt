package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.Switch
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.app.AppCompatDelegate
import cs309.selectunes.R
import cs309.selectunes.services.AuthServiceImpl

/**
 * The login activity is where users can
 * log into the app.
 * Color pallets:
 * https://colorhunt.co/palette/69667
 * https://colorhunt.co/palette/2763
 * @author Jack Goldsworth
 */
class LoginActivity : AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.login_menu)
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
            AuthServiceImpl().login(email.text.toString(), password.text.toString(), this)
        }
    }

    private fun checkDarkMode(isChecked: Boolean) {
        if (isChecked) {
            AppCompatDelegate.setDefaultNightMode(AppCompatDelegate.MODE_NIGHT_YES)
        } else {
            AppCompatDelegate.setDefaultNightMode(AppCompatDelegate.MODE_NIGHT_NO)
        }
    }
}