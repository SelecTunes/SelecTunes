package cs309.selectunes.utils

import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import org.json.JSONArray
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
        if (body == "true") {
            return true
        }
        val json = JSONArray(body)
        val registerError = activity.findViewById<TextView>(R.id.register_error)
        if (json.getJSONObject(0).has("errors")) {
            registerError.text = json.getJSONObject(0).getJSONArray("ConfirmPassword").getString(0)
            return false
        } else if (json.getJSONObject(0).has("description")) {
            registerError.text = json.getJSONObject(0).getString("description")
            return false
        } else {
            registerError.text = ""
        }
        return true
    }
}