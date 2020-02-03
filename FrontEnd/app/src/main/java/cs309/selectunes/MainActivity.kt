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
        val button = findViewById<Button>(R.id.button)
        val textBox = findViewById<EditText>(R.id.editText5)
        button.setOnClickListener {
            startActivity(Intent(this, LoginActivity::class.java))
        }
    }
}
