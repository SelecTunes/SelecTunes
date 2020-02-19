package cs309.selectunes.utils

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

object HttpUtils {

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

    fun endParty(activity: AppCompatActivity) {
        val stringRequest = StringRequest(Request.Method.DELETE,
            "https://coms-309-jr-2.cs.iastate.edu/api/Party/DisbandParty",
            null,
            Response.ErrorListener {
                println("There was an error with the response. Code: ${it.networkResponse.statusCode}")
                println("Response: ${it.networkResponse.data.toString(StandardCharsets.UTF_8)}")
            })
        val requestQueue = Volley.newRequestQueue(activity, createAuthCookie(activity))
        requestQueue.add(stringRequest)
    }
}