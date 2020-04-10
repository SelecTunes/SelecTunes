package cs309.selectunes.services

import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.StringRequest
import com.android.volley.toolbox.Volley
import cs309.selectunes.R
import cs309.selectunes.utils.HttpUtils

class SongServiceImpl : SongService {

    override fun makeSongsExplicit(activity: AppCompatActivity) {
        val stringRequest = StringRequest(
            Request.Method.POST,
            "https://coms-309-jr-2.cs.iastate.edu/api/party/explicit",
            Response.Listener {
                isExplicit(activity)
            },
            Response.ErrorListener {
                println("There was an error with making the song explicit: ${it.networkResponse.statusCode}")
                println(it.networkResponse.data)
            })
        val requestQueue = Volley.newRequestQueue(activity, HttpUtils.createAuthCookie(activity))
        requestQueue.add(stringRequest)
    }

    override fun isExplicit(activity: AppCompatActivity) {
        val stringRequest = JsonObjectRequest(
            Request.Method.GET,
            "https://coms-309-jr-2.cs.iastate.edu/api/party/explicit",
            null,
            Response.Listener {
                if (it != null) {
                    val explicit = activity.findViewById<Button>(R.id.explicit)
                    if (it.getBoolean("allowed")) {
                        explicit.setBackgroundColor(activity.resources.getColor(R.color.colorSecondaryDark))
                    } else {
                        explicit.setBackgroundColor(activity.resources.getColor(R.color.colorBright))
                    }
                }
            },
            Response.ErrorListener {
                println("There was an error with making the song explicit: ${it.networkResponse.statusCode}")
                println(it.networkResponse.data)
            })
        val requestQueue = Volley.newRequestQueue(activity, HttpUtils.createAuthCookie(activity))
        requestQueue.add(stringRequest)
    }
}