package cs309.selectunes

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import androidx.appcompat.app.AppCompatActivity

class PhoneNumberExperiment : AppCompatActivity() {

    override fun onCreate(instanceState: Bundle?)
    {
        super.onCreate(instanceState)
        setContentView(R.layout.fragment_gallery)
        val sendNum = findViewById<Button>(R.id.button_id)

        sendNum.setOnClickListener {
            val changeActivity = Intent(this, experiment1::class.java)
            changeActivity.putExtra("PhoneNumber", findViewById<EditText>(R.id.phone_number).text.toString())
            startActivity(changeActivity)
        }
    }



}
