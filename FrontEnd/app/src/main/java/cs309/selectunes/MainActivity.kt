package cs309.selectunes

import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import android.widget.Button
import android.widget.EditText

class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.login)
        val button = findViewById<Button>(R.id.login_button)
        val username = findViewById<EditText>(R.id.username)
        val phoneNumber = findViewById<EditText>(R.id.phone)
        button.setOnClickListener {
            val intent = Intent(this, LoginActivity::class.java)
            intent.putExtra("number", phoneNumber.text)
            intent.putExtra("username", username.text)
            startActivity(intent)
        }
    }
}
