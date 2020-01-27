package cs309.selectunes

import android.os.Bundle
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity

class PhoneNumberActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.show_phone)
        val text = findViewById<TextView>(R.id.editText)
        text.text = intent.getStringExtra("phone")
    }
}
