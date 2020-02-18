package cs309.selectunes.utils

import androidx.appcompat.app.AppCompatActivity
import com.android.volley.toolbox.HttpClientStack
import org.apache.http.impl.client.BasicCookieStore
import org.apache.http.impl.client.DefaultHttpClient
import org.apache.http.impl.cookie.BasicClientCookie

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
}