package cs309.selectunes

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.Switch
import androidx.appcompat.app.AppCompatActivity

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
