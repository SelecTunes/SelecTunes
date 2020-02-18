package cs309.selectunes.utils

import java.security.SecureRandom
import java.security.cert.X509Certificate
import javax.net.ssl.HttpsURLConnection
import javax.net.ssl.SSLContext
import javax.net.ssl.TrustManager
import javax.net.ssl.X509TrustManager


/**
 * This is to allow requests to ignore SSL certifications.
 * TESTS ONLY.
 * from https://newfivefour.com/android-trust-all-ssl-certificates.html
 */
object NukeSSLCerts {
    fun nuke() {
        try {
            val trustAllCerts: Array<TrustManager> = arrayOf(
                    object : X509TrustManager {
                        override fun checkClientTrusted(certs: Array<X509Certificate?>?, authType: String?) {}
                        override fun checkServerTrusted(certs: Array<X509Certificate?>?, authType: String?) {}
                        override fun getAcceptedIssuers(): Array<X509Certificate> {
                            TODO("not implemented") //To change body of created functions use File | Settings | File Templates.
                        }
                    }
            )
            val sc: SSLContext = SSLContext.getInstance("SSL")
            sc.init(null, trustAllCerts, SecureRandom())
            HttpsURLConnection.setDefaultSSLSocketFactory(sc.getSocketFactory())
            HttpsURLConnection.setDefaultHostnameVerifier { arg0, arg1 -> true }
        } catch (e: Exception) {
        }
    }
}