package cs309.selectunes

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView

private lateinit var linearLayoutManager: LinearLayoutManager

class SongListActivity : AppCompatActivity()
{
    override fun onCreate(instanceState:  Bundle?)
    {
        super.onCreate(instanceState)
        setContentView(R.layout.song_list_layout)

        linearLayoutManager = LinearLayoutManager(this)
        var songRecycler = findViewById<RecyclerView>(R.id.song_recycler)
        songRecycler.layoutManager = linearLayoutManager

        val returnButton = findViewById<Button>(R.id.return_id)

        //button sends back to main host menu upon click
        returnButton.setOnClickListener{
            val goBack = Intent(this, HostMenuActivity::class.java)
            startActivity(goBack)
        }



    }
}