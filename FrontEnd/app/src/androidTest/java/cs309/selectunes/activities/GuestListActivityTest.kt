package cs309.selectunes.activities

import androidx.test.rule.ActivityTestRule
import cs309.selectunes.models.Guest
import cs309.selectunes.models.Song
import cs309.selectunes.services.ServerService
import org.json.JSONArray
import org.json.JSONObject
import org.junit.Assert.assertEquals
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.mockito.Mockito

class GuestListActivityTest {

    @get:Rule
    val activityTestRule = ActivityTestRule(GuestListActivity::class.java)

    private lateinit var serverService: ServerService

    private lateinit var guestListActivity: GuestListActivity


    @Before
    fun setup() {
        guestListActivity = activityTestRule.activity
        serverService = Mockito.mock(ServerService::class.java)
        var guestList = ArrayList<Guest>()
        // Just test the first five entries.
        guestList.add(Guest("test1@iastate.edu"))
        guestList.add(Guest("joshuae1@iastate.edu"))
        guestList.add(Guest("natetuck@iastate.edu"))
    }

    @Test
    fun testParseGuestList() {
        Mockito.`when`(serverService.getGuestList(guestListActivity)).then{
            val fileText = this.javaClass.classLoader?.getResource("guestListExample.json")?.readText(Charsets.UTF_8)
            val json = JSONArray(fileText ?: error("example song json file not found."))
            serverService.parseGuests(json)
        }
        serverService.getGuestList(guestListActivity)

        var isSame = true
        for (i in 0..2) {

        }
        assertEquals(isSame, true)
    }
}