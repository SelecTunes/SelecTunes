package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import cs309.selectunes.models.Song

class SongListActivity : AppCompatActivity()
{
    var songs = ArrayList<Song>()

    override fun onCreate(instanceState:  Bundle?)
    {
        super.onCreate(instanceState)
        setContentView(R.layout.song_search_menu)

        val returnButton = findViewById<Button>(R.id.return_id)

        //button sends back to main host menu upon click
        returnButton.setOnClickListener{
            val goBack = Intent(this, HostMenuActivity::class.java)
            startActivity(goBack)
        }


    }

}