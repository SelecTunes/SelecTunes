package cs309.selectunes.activities

import androidx.test.rule.ActivityTestRule
import cs309.selectunes.models.Guest
import cs309.selectunes.services.ServerService
import org.json.JSONArray
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

    private var guestList = ArrayList<Guest>()
    @Before
    fun setup() {
        guestListActivity = activityTestRule.activity
        serverService = Mockito.mock(ServerService::class.java)
        // Just test the first three entries.
        guestList.add(Guest("test1@iastate.edu"))
        guestList.add(Guest("joshuae1@iastate.edu"))
        guestList.add(Guest("natetuck@iastate.edu"))
    }

    @Test
    fun testParseGuestList() {
        Mockito.`when`(serverService.getGuestList(guestListActivity, true)).then {
            val fileText = this.javaClass.classLoader?.getResource("guestListExample.json")?.readText(Charsets.UTF_8)
            val json = JSONArray(fileText ?: error("example song json file not found."))
            guestListActivity.parseGuests(json)
        }
        serverService.getGuestList(guestListActivity, true)

        var isSame = true
        for (i in 0..2) {
            val tempList = guestListActivity.guestList
            isSame = isSame && tempList[i].givenEmail == guestList[i].givenEmail
        }
        assertEquals(isSame, true)
    }
}