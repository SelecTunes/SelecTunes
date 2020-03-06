package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import cs309.selectunes.R
import cs309.selectunes.SongRecyclerViewAdapter
import cs309.selectunes.models.Song

class SongListActivity : AppCompatActivity()
{
    var songs = ArrayList<Song>()

    override fun onCreate(instanceState:  Bundle?)
    {
        super.onCreate(instanceState)
        setContentView(R.layout.song_queue)


        val returnButton = findViewById<Button>(R.id.back_button)

        //button sends back to main host menu upon click
        returnButton.setOnClickListener{
            val goBack = Intent(this, HostMenuActivity::class.java)
            startActivity(goBack)
        }
        val recycler = findViewById<RecyclerView>(R.id.song_recycler_id)
        recycler.setLayoutManager(LinearLayoutManager(this))
        val adapter = SongRecyclerViewAdapter(songs, this)

        recycler.adapter = adapter

    }

}