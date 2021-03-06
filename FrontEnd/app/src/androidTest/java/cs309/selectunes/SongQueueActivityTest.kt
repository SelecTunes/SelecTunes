package cs309.selectunes

import androidx.test.rule.ActivityTestRule
import cs309.selectunes.activities.SongQueueActivity
import cs309.selectunes.models.Song
import cs309.selectunes.services.SongService
import cs309.selectunes.utils.JsonUtils
import org.json.JSONObject
import org.junit.Assert
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.mockito.Mockito

class SongQueueActivityTest {

    @get:Rule
    val activityTestRule = ActivityTestRule(SongQueueActivity::class.java)

    private lateinit var songService: SongService

    private lateinit var songQueueActivity: SongQueueActivity

    private lateinit var songList: MutableList<Song>

    private lateinit var parsedSongs: ArrayList<Song>

    @Before
    fun setup() {
        songQueueActivity = activityTestRule.activity
        songService = Mockito.mock(SongService::class.java)
        songList = mutableListOf()
        parsedSongs = ArrayList()
        // Just test the first five entries.
        songList.add(
                Song(
                        "Isis (feat. Logic)",
                        "5VrUKu4GkDe0bCA2VQR2Km",
                        "Joyner Lucas",
                        "https://i.scdn.co/image/ab67616d0000b273cc1f7795b7e7fcb43970da0c",
                        false,
                        true
                )
        )
        songList.add(
                Song(
                        "Jesus Christ 2005 God Bless America",
                        "3tk5DaXxxUoV7b5nOdCDBv",
                        "The 1975",
                        "https://i.scdn.co/image/ab67616d0000b27307d948c7ffff570f2688cc91",
                        false,
                        false
                )
        )
        songList.add(
                Song(
                        "Be My Mistake",
                        "18UsAG7SfOQ5sxJEdjAMH0",
                        "The 1975",
                        "https://i.scdn.co/image/ab67616d0000b2736c582022e90b11f0da287a9a",
                        false,
                        true
                )
        )
        songList.add(
                Song(
                        "The Sound",
                        "316r1KLN0bcmpr7TZcMCXT",
                        "The 1975",
                        "https://i.scdn.co/image/ab67616d0000b273d3de03550f715df1ea7e0c08",
                        false,
                        true
                )
        )
    }

    @Test
    fun testQueueParseJson() {
        Mockito.`when`(songService.getSongQueue(songQueueActivity, null, null)).then {
            val fileText = this.javaClass.classLoader?.getResource("example_queue_list.json")
                    ?.readText(Charsets.UTF_8)
            val jsonObject = JSONObject(fileText ?: error("example queue json file not found."))
            val voteableSongArray = jsonObject.getJSONArray("votable")
            val lockedSongArray = jsonObject.getJSONArray("lockedIn")
            val voteableSongList = JsonUtils.parseSongQueue(voteableSongArray, true)
            val lockedSongList = JsonUtils.parseSongQueue(lockedSongArray, false)
            val allSongs = ArrayList<Song>()
            allSongs.addAll(lockedSongList)
            allSongs.addAll(voteableSongList)
            allSongs.forEach { parsedSongs.add(it) }
        }
        songService.getSongQueue(songQueueActivity, null, null)
        var isSame = true
        for (i in 0..3) {
            isSame = isSame && parsedSongs[i].songName == songList[i].songName
            isSame = isSame && parsedSongs[i].artistName == songList[i].artistName
            isSame = isSame && parsedSongs[i].id == songList[i].id
            isSame = isSame && parsedSongs[i].albumArt == songList[i].albumArt
        }
        Assert.assertEquals(isSame, true)
    }

    @Test
    fun testQueueAdd() {
        val song = Song("Gimmie Love", "1", "Joji", "", explicit = false, voteable = false)
        val song2 = Song("Right", "2", "Mac Miller", "", explicit = false, voteable = false)
        var lastSong = ""
        Mockito.`when`(songService.addSongToQueue(song, songQueueActivity)).then {
            songList.add(song)
            songList.add(song2)
            lastSong = song.id
            null
        }
        Mockito.`when`(songService.removeLockedSong("", songQueueActivity)).then {
            val firstSong = songList.firstOrNull { it.id == lastSong }
            songList.remove(firstSong)
            lastSong = song2.id
            null
        }
        songService.addSongToQueue(song, songQueueActivity)
        Assert.assertEquals(song.id, lastSong)
        songService.removeLockedSong("", songQueueActivity)
        Assert.assertEquals(song2.id, lastSong)
    }
}