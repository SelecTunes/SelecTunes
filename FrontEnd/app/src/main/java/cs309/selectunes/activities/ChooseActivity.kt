package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R

class ChooseActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_choose)

        val button = findViewById<Button>(R.id.create_party_button)

        button.setOnClickListener {
            startActivity(Intent(this, HostMenuActivity::class.java))
        }
    }
}
