package cs309.selectunes.services

import androidx.appcompat.app.AppCompatActivity

interface ServerService {

    fun createParty(auth: String, activity: AppCompatActivity)
}