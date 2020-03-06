package cs309.selectunes.services

import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.activities.SongSearchActivity

interface ServerService {

    fun createParty(auth: String, activity: AppCompatActivity)

    fun searchSong(songToSearch: String, activity: SongSearchActivity)
}