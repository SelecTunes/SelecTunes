package cs309.selectunes.utils

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.HttpClientStack
import com.android.volley.toolbox.StringRequest
import com.android.volley.toolbox.Volley
import org.apache.http.impl.client.BasicCookieStore
import org.apache.http.impl.client.DefaultHttpClient
import org.apache.http.impl.cookie.BasicClientCookie
import java.nio.charset.StandardCharsets

/**
 * General http based utilities.
 * @author Jack Goldsworth
 */
object HttpUtils {

    /**
     * Creates an http client stack the contains an
     * authorization cookie.
     * @param activity activity this method is called from.
     */
    fun createAuthCookie(activity: AppCompatActivity): HttpClientStack {
        val httpclient = DefaultHttpClient()
        val cookieStore = BasicCookieStore()
        val settings = activity.getSharedPreferences("Cookie", 0)
        val cookie = BasicClientCookie("Holtzmann", settings.getString("cookie", ""))
        cookie.domain = "coms-309-jr-2.cs.iastate.edu"
        cookieStore.addCookie(cookie)
        httpclient.cookieStore = cookieStore
        return HttpClientStack(httpclient)
    }

    /**
     * This http request ends the party
     * the user is currently in.
     * @param activity activity this method is being called from.
     * @param switchedActivity activity to go to after the party is closed.
     */
    fun endParty(activity: AppCompatActivity, switchedActivity: Class<out AppCompatActivity>?) {
        val stringRequest = StringRequest(Request.Method.DELETE,
                "https://coms-309-jr-2.cs.iastate.edu/api/Party/DisbandParty",
                Response.Listener {
                    if (switchedActivity != null) {
                        activity.startActivity(Intent(activity, switchedActivity))
                    }
                },
                Response.ErrorListener {
                    println("There was an error with the response. Code: ${it.networkResponse.statusCode}")
                    println("Response: ${it.networkResponse.data.toString(StandardCharsets.UTF_8)}")
                })
        val requestQueue = Volley.newRequestQueue(activity, createAuthCookie(activity))
        requestQueue.add(stringRequest)
    }

    /**
     * This http request makes the user leave
     * the party they're already in.
     * @param activity activity this method is being called from.
     * @param switchedActivity activity to go to after the party is closed.
     */
    fun leaveParty(activity: AppCompatActivity, switchedActivity: Class<out AppCompatActivity>?) {
        val stringRequest = StringRequest(Request.Method.POST,
                "https://coms-309-jr-2.cs.iastate.edu/api/Party/LeaveParty",
                Response.Listener {
                    if (switchedActivity != null) {
                        activity.startActivity(Intent(activity, switchedActivity))
                    }
                },
                Response.ErrorListener {
                    println("There was an error with the response. Code: ${it.networkResponse.statusCode}")
                    println("Response: ${it.networkResponse.data.toString(StandardCharsets.UTF_8)}")
                })
        val requestQueue = Volley.newRequestQueue(activity, createAuthCookie(activity))
        requestQueue.add(stringRequest)
    }
}