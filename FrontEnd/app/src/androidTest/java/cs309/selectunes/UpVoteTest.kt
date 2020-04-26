package cs309.selectunes

import androidx.test.rule.ActivityTestRule
import com.microsoft.signalr.HubConnectionBuilder
import cs309.selectunes.activities.SongQueueActivity
import cs309.selectunes.services.SongService
import junit.framework.Assert.assertEquals
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.mockito.Mockito

class UpVoteTest {

    @get:Rule
    val activityTestRule = ActivityTestRule(SongQueueActivity::class.java)

    private lateinit var serverService: SongService

    private lateinit var songListActivity: SongQueueActivity

    private var votes = HashMap<String, Int>()

    @Before
    fun setup()
    {
        songListActivity = activityTestRule.activity
        serverService = Mockito.mock(SongService::class.java)
    }

    @Test
    fun testFailedConnection()
    {
        val url = "http://coms-309-jr-2.cs.iastate.edu/ThisWillFail"

        val settings = songListActivity.getSharedPreferences("Cookie", 0)
        val hubConnection = HubConnectionBuilder.create(url)
            .withHeader("cookie", "Holtzmann=" + settings.getString("cookie", ""))
            .build()

        Mockito.`when`(serverService.getSongQueue(songListActivity, hubConnection, votes)).then {
            assertEquals(hubConnection.connectionState.name, "disconnected")
        }
    }

    @Test
    fun testSuccessfulConnection()
    {
        val url = "http://coms-309-jr-2.cs.iastate.edu/queue"

        val settings = songListActivity.getSharedPreferences("Cookie", 0)
        val hubConnection = HubConnectionBuilder.create(url)
            .withHeader("cookie", "Holtzmann=" + settings.getString("cookie", ""))
            .build()

        Mockito.`when`(serverService.getSongQueue(songListActivity, hubConnection, votes)).then {
            assertEquals(hubConnection.connectionState.name, "connected")
        }
    }


}