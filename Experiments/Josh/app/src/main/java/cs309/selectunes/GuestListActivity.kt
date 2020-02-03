package cs309.selectunes

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity

class GuestListActivity : AppCompatActivity()
{

    override fun onCreate(instanceState: Bundle?)
    {
        super.onCreate(instanceState)
        setContentView(R.layout.guest_list_layout)

        val returnButton = findViewById<Button>(R.id.return_id)
        returnButton.setOnClickListener{
            val backOut = Intent(this, HostMenuActivity::class.java)
            startActivity(backOut)
        }
    }
}