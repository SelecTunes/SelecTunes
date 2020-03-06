package cs309.selectunes.services

import androidx.appcompat.app.AppCompatActivity

/**
 * Http requests that are used for auth. The interface
 * is needed so we can mock these calls.
 * @author Jack Goldsworth
 */
interface AuthService {

    /**
     * Http request to allow users to log into the app.
     * @param email email address.
     * @param password password.
     * @param activity activity this is being called from.
     */
    fun login(email: String, password: String, activity: AppCompatActivity)

    /**
     * Http request that allows users to register on the app.
     * @param email email address.
     * @param password password.
     * @param passwordConfirmed confirm password.
     * @param activity activity this is being called from.
     */
    fun register(email: String, password: String, passwordConfirmed: String, activity: AppCompatActivity)
}