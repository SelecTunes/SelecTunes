package cs309.selectunes.models

/**
 * @author Joshua Edwards
 */
data class Song(
    val songName: String,
    val id: String,
    val artistName: String,
    val albumArt: String,
    val explicit: Boolean,
    val voteable: Boolean?
)