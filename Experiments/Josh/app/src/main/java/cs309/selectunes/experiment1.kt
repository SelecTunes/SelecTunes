package cs309.selectunes

import android.os.Bundle
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity

class experiment1 : AppCompatActivity()
{

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.fragment_home)
        val textBox = findViewById<TextView>(R.id.text_home)
        textBox.text = intent.getStringExtra("PhoneNumber")
    }



}