package cs309.selectunes

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity

class SongListActivity : AppCompatActivity()
{
    override fun onCreate(instanceState:  Bundle?)
    {
        super.onCreate(instanceState)
        setContentView(R.layout.song_list_layout)

        val returnButton = findViewById<Button>(R.id.return_id)

        //button sends back to main host menu upon click
        returnButton.setOnClickListener{
            val goBack = Intent(this, HostMenuActivity::class.java)
            startActivity(goBack)
        }



    }
}