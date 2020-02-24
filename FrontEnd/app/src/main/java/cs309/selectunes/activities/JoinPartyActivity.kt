package cs309.selectunes.activities

import android.os.Bundle
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R

class JoinPartyActivity : AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.login_menu)
        val button = findViewById<Button>(R.id.join_button)
        val code = findViewById<TextView>(R.id.join_code)
        button.setOnClickListener {

        }
    }
}