package cs309.selectunes.services

import androidx.appcompat.app.AppCompatActivity

interface AuthService {

    fun login(email: String, password: String, activity: AppCompatActivity)

    fun register(email: String, password: String, passwordConfirmed: String, activity: AppCompatActivity)
}