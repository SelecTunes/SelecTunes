package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import com.bumptech.glide.load.engine.DiskCacheStrategy
import com.bumptech.glide.request.RequestOptions
import cs309.selectunes.R

class SongListActivity : AppCompatActivity()
{
    var songs = ArrayList<Song>()

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

    fun cacheImages()
    {
        val options = RequestOptions()
                .diskCacheStrategy(DiskCacheStrategy.ALL)
                .override(100,100)

        for(s in songs)
        {

        }
    }

}