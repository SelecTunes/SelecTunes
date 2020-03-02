package cs309.selectunes.activities

import android.widget.Button
import androidx.test.rule.ActivityTestRule
import cs309.selectunes.R
import org.junit.After
import org.junit.Assert.assertNotNull
import org.junit.Before
import org.junit.Rule
import org.junit.Test

class ChooseActivityTest {

    @get:Rule
    val activityTestRule = ActivityTestRule<ChooseActivity>(ChooseActivity::class.java)

    private var chooseActivity: ChooseActivity? = null

    @Before
    fun setup() {
        chooseActivity = activityTestRule.activity
    }

    @Test
    fun testViews() {
        val createPartyButton = chooseActivity?.findViewById<Button>(R.id.create_party_button)
        val joinPartyButton = chooseActivity?.findViewById<Button>(R.id.join_party_button)
        assertNotNull(createPartyButton)
        assertNotNull(joinPartyButton)
    }

    @After
    fun tearDown() {
        chooseActivity = null
    }
}