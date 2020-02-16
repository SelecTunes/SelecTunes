package cs309.selectunes.utils

import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import org.json.JSONObject

object JsonUtils {

    fun parseLoginResponse(activity: AppCompatActivity, body: String): Boolean {
        val json = JSONObject(body)
        val success = json.getBoolean("success")
        val loginError = activity.findViewById<TextView>(R.id.login_error)
        if (!success) {
            loginError.text = "There was an issue with the email or password."
            if (json.has("errors")) {
                loginError.text = json.getJSONObject("errors").getJSONArray("ConfirmPassword").getString(0)
            }
        } else {
            loginError.text = ""
        }
        return success
    }

    fun parseRegisterResponse(activity: AppCompatActivity, body: String): Boolean {
        val json = JSONObject(body)
        val success = json.getBoolean("success")
        val registerError = activity.findViewById<TextView>(R.id.register_error)
        if (!success) {
            registerError.text = "There was an issue with the email or password."
            if (json.has("errors")) {
                registerError.text = json.getJSONObject("errors").getJSONArray("ConfirmPassword").getString(0)
            }
        } else {
            registerError.text = ""
        }
        return success
    }
}