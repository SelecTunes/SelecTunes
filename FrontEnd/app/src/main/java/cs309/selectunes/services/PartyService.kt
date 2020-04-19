package cs309.selectunes.services

import androidx.appcompat.app.AppCompatActivity
import cs309.selectunes.activities.GuestListActivity

/**
 * General http request methods for the app.
 * @author Jack Goldsworth.
 * @author Josh Edwards.
 */
interface PartyService {

    /**
     * Http requests to create a party and receive a join code.
     * @param auth Spotify auth token.
     * @param activity activity this method is called from.
     */
    fun createParty(auth: String, activity: AppCompatActivity)

    /**
     * Kicks a guest from the current party.
     * @param givenEmail email of the person to kick.
     * @param activity activity this method is called from.
     */
    fun kickGuest(givenEmail: String, activity: GuestListActivity)

    /**
     * Gets the current list of people in a party.
     * @param activity activity this method is called from.
     * @param isGuest is the caller a guest or a host.
     */
    fun getGuestList(activity: GuestListActivity, isGuest: Boolean)

    /**
     * This http request makes the user leave
     * the party they're already in.
     * @param activity activity this method is being called from.
     * @param switchedActivity activity to go to after the party is closed.
     */
    fun leaveParty(activity: AppCompatActivity, switchedActivity: Class<out AppCompatActivity>?)

    /**
     * This http request ends the party
     * the user is currently in.
     * @param activity activity this method is being called from.
     * @param switchedActivity activity to go to after the party is closed.
     */
    fun endParty(activity: AppCompatActivity, switchedActivity: Class<out AppCompatActivity>?)
}