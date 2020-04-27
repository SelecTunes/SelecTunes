package cs309.selectunes.utils

import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.R
import cs309.selectunes.models.Song
import org.json.JSONArray
import org.json.JSONObject

/**
 * General JSON based utility functions.
 * @author Jack Goldsworth
 */
object JsonUtils {

    /**
     * Parses the login request response and reports if there is an error.
     * @param activity activity this method is being called from.
     * @param body login response body.
     */
    fun parseLoginResponse(activity: AppCompatActivity, body: String): Boolean {
        val json = JSONObject(body)
        val success = json.getBoolean("success")
        val loginError = activity.findViewById<TextView>(R.id.login_error)
        if (!success) {
            loginError.text = "There was an issue with the email or password."
            if (json.has("errors")) {
                loginError.text =
                    json.getJSONObject("errors").getJSONArray("ConfirmPassword").getString(0)
            }
        } else {
            loginError.text = ""
        }
        return success
    }

    /**
     * Parses the register request response and reports if there is an error.
     * @param activity activity this method is being called from.
     * @param body login response body.
     * @param status request response code.
     */
    fun parseRegisterResponse(activity: AppCompatActivity?, body: String?, status: Int): Boolean {
        println(body)
        val registerError = activity?.findViewById<TextView>(R.id.register_error)
        if (body == "true") {
            registerError?.text = ""
            return true
        }
        var json: JSONArray? = null
        if (body != null) {
            json = JSONArray(body)
        }
        return when {
            status == 400 -> {
                if (activity != null) registerError?.text = "The passwords don't match."
                false
            }
            status == 500 -> {
                if (activity != null) registerError?.text = "The server is down."
                false
            }
            body != null && json?.getJSONObject(0)!!.has("errors") -> {
                registerError?.text =
                    json.getJSONObject(0).getJSONArray("ConfirmPassword").getString(0)
                false
            }
            body != null && json!!.getJSONObject(0).has("description") -> {
                registerError?.text = json.getJSONObject(0).getString("description")
                false
            }
            else -> true
        }
    }

    /**
     * Parses the song queue into a list of songs.
     * @param songs song array from the getSongQueue request.
     * @return list of songs.
     */
    fun parseSongQueue(songs: JSONArray, isVoteable: Boolean): List<Song> {
        val songList = mutableListOf<Song>()
        for (i in 0 until songs.length()) {
            val jsonObj = songs.getJSONObject(i)
            songList.add(
                Song(
                    jsonObj.getString("name"),
                    jsonObj.getString("id"),
                    jsonObj.getString("artistName"),
                    jsonObj.getString("albumArt"),
                    false,
                    isVoteable
                )
            )
        }
        return songList
    }
}