package cs309.selectunes

import org.json.JSONObject
import java.io.File

object TestJson {

    @JvmStatic
    fun main(args: Array<String>) {
        val text = File("C:\\Users\\top_e\\StudioProjects\\JR_2\\FrontEnd\\app\\src\\test\\java\\cs309\\selectunes\\test.json").readText()
        val jsonObject = JSONObject(text)
        val jsonTop = jsonObject.getJSONObject("tracks")
        val jsonArr = jsonTop.getJSONArray("items")
        for(x in 0..jsonArr.length())
        {
            //var thisSong = Song()
            val song = jsonArr.getJSONObject(x)
            val info = song.getString("name")
            println(info)
            //thisSong.songName = info.
        }
    }
}