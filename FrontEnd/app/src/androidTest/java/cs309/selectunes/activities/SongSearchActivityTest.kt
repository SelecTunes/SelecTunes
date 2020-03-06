package cs309.selectunes.activities

import androidx.test.rule.ActivityTestRule
import cs309.selectunes.services.ServerService
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.mockito.Mockito

class SongSearchActivityTest {

    @get:Rule
    val activityTestRule = ActivityTestRule(SongSearchActivity::class.java)

    private val serverService = Mockito.mock(ServerService::class.java)

    private lateinit var songSearchActivity: SongSearchActivity

    @Before
    fun setup() {
        songSearchActivity = activityTestRule.activity
    }

    @Test
    fun testParseJson() {

    }

    @Test
    fun testViews() {
    }
}