package cs309.selectunes

import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity

class LoginActivity : AppCompatActivity()

{

    override fun onCreate(instanceState: Bundle?)
    {
        super.onCreate(instanceState)
        setContentView(R.layout.login_main)
        val login = findViewById<Button>(R.id.login_id)
        login.setOnClickListener{

        }
    }

}