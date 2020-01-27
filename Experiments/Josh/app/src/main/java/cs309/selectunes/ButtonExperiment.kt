package cs309.selectunes

import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R

class ButtonExperiment : AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?) {
        super.onCreate(instanceState)
        setContentView(R.layout.fragment_gallery)

        val btn_test = findViewById(R.id.button_id) as Button
        val phone_number = findViewById(R.id.phone_number) as EditText

        println("we happening")
        btn_test.setOnClickListener{
            phone_number.getText()
        }
    }
}
