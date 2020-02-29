package cs309.selectunes.activities

import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.utils.HttpUtils

class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onStart() {
        super.onStart()
        startActivity(Intent(this, LoginActivity::class.java))
    }

    override fun onDestroy() {
        super.onDestroy()
        HttpUtils.endParty(this, null)
    }
}
