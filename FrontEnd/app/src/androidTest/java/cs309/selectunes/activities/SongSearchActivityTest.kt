package cs309.selectunes.activities

import androidx.test.rule.ActivityTestRule
import cs309.selectunes.models.Song
import cs309.selectunes.services.ServerService
import org.json.JSONObject
import org.junit.Assert.assertEquals
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.mockito.Mockito

class SongSearchActivityTest {

    @get:Rule
    val activityTestRule = ActivityTestRule(SongSearchActivity::class.java)

    private lateinit var serverService: ServerService

    private lateinit var songSearchActivity: SongSearchActivity

    private lateinit var songList: MutableList<Song>

    @Before
    fun setup() {
        songSearchActivity = activityTestRule.activity
        serverService = Mockito.mock(ServerService::class.java)
        songList = mutableListOf()
        // Just test the first five entries.
        songList.add(Song("Good News", "1DWZUa5Mzf2BwzpHtgbHPY", "Mac Miller", "https://i.scdn.co/image/ab67616d0000b27326b7dd89810cc1a40ada642c", false, null))
        songList.add(Song("Good News", "1dXCXb006YbPSAajh6qhaF", "Ocean Park Standoff", "https://i.scdn.co/image/ab67616d0000b273d33abaa87fdea5da142fd201", false, null))
        songList.add(Song("Good News", "5cInnHweXJJrRsHxwihgkK", "K.Flay", "https://i.scdn.co/image/ab67616d0000b2737f888564b247d806d838761e", false, null))
        songList.add(Song("Good News", "5sSqOGfBz5CjJ9IGL8pz31", "Apashe", "https://i.scdn.co/image/ab67616d0000b2735a1f4b77ef7a4ba35e6f7d1c", false, null))
        songList.add(Song("Good News", "3QqTmV1zpTNEAMviQFUFkW", "Mandisa", "https://i.scdn.co/image/ab67616d0000b273005a275e8a0821ab2311e33e", false, null))

    }

    @Test
    fun testParseJson() {
        Mockito.`when`(serverService.searchSong("Good News", songSearchActivity)).then {
            val fileText = this.javaClass.classLoader?.getResource("example_song_list.json")?.readText(Charsets.UTF_8)
            val json = JSONObject(fileText ?: error("example song json file not found."))
            songSearchActivity.parseJson(json)
        }
        serverService.searchSong("Good News", songSearchActivity)
        var isSame = true
        for (i in 0..4) {
            val tempList = songSearchActivity.songList
            isSame = isSame && tempList[i].songName == songList[i].songName
            isSame = isSame && tempList[i].artistName == songList[i].artistName
            isSame = isSame && tempList[i].id == songList[i].id
            isSame = isSame && tempList[i].albumArt == songList[i].albumArt
            isSame = isSame && tempList[i].explicit == songList[i].explicit
        }
        assertEquals(isSame, true)
    }

    @Test
    fun testViews() {
    }
}