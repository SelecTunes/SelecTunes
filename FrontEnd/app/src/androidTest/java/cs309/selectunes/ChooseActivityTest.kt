package cs309.selectunes

import android.widget.Button
import androidx.test.rule.ActivityTestRule
import cs309.selectunes.activities.ChooseActivity
import cs309.selectunes.services.PartyService
import org.json.JSONObject
import org.junit.Assert.assertEquals
import org.junit.Assert.assertNotNull
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.mockito.Mockito

class ChooseActivityTest {

    @get:Rule
    val activityTestRule = ActivityTestRule(ChooseActivity::class.java)

    private val serverService = Mockito.mock(PartyService::class.java)

    private lateinit var chooseActivity: ChooseActivity

    @Before
    fun setup() {
        chooseActivity = activityTestRule.activity
    }

    @Test
    fun testCreateParty() {
        val settings = chooseActivity.getSharedPreferences("PartyInfo", 0)
        Mockito.`when`(serverService.createParty("test", chooseActivity)).then {
            val jsonObj = JSONObject()
            jsonObj.put("joinCode", "tempJoinCode")
            val editor = settings.edit()
            editor.putString("join_code", jsonObj.getString("joinCode"))
            editor.apply()
        }
        serverService.createParty("test", chooseActivity)
        assertEquals(settings.getString("join_code", ""), "tempJoinCode")
    }

    @Test
    fun testViews() {
        val createPartyButton = chooseActivity.findViewById<Button>(R.id.create_party_button)
        val joinPartyButton = chooseActivity.findViewById<Button>(R.id.join_party_button)
        assertNotNull(createPartyButton)
        assertNotNull(joinPartyButton)
    }
}