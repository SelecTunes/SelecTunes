//package cs309.selectunes.activities
//
//import androidx.test.rule.ActivityTestRule
//import cs309.selectunes.models.Guest
//import cs309.selectunes.models.Song
//import cs309.selectunes.services.ServerService
//import org.json.JSONObject
//import org.junit.Assert.assertEquals
//import org.junit.Before
//import org.junit.Rule
//import org.junit.Test
//import org.mockito.Mockito
//
//class GuestListActivityTest {
//
//    @get:Rule
//    val activityTestRule = ActivityTestRule(GuestListActivity::class.java)
//
//    private lateinit var serverService: ServerService
//
//    private lateinit var guestListActivity: GuestListActivity
//
//
//    @Before
//    fun setup() {
//        guestListActivity = activityTestRule.activity
//        serverService = Mockito.mock(ServerService::class.java)
//        var guestList = ArrayList<Guest>()
//        // Just test the first five entries.
//        guestList.add(Guest("jackGold@iastate.edu"))
//        guestList.add(Guest("joshuae1@iastate.edu"))
//        guestList.add(Guest("natetuck@iastate.edu"))
//        guestList.add(Guest("alexyoung@iastate.edu"))
//
//    }
//
//    @Test
//    fun testParseGuestList() {
////        Mockito.`when`(serverService.getGuestList().then {
////
////
////        }
////        serverService.searchSong("Good News", songSearchActivity)
////        var isSame = true
////        for (i in 0..4) {
////            val tempList = songSearchActivity.songList
////            isSame = isSame && tempList[i].songName == songList[i].songName
////            isSame = isSame && tempList[i].artistName == songList[i].artistName
////            isSame = isSame && tempList[i].id == songList[i].id
////            isSame = isSame && tempList[i].albumArt == songList[i].albumArt
////            isSame = isSame && tempList[i].explicit == songList[i].explicit
////        }
////        assertEquals(isSame, true)
//    }
//
//    @Test
//    fun testViews() {
//    }
//}