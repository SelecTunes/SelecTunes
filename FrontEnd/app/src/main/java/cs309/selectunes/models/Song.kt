package cs309.selectunes.models


class Song(
    val songName: String,
    val id: String,
    val artistName: String,
    val albumArt: String,
    val explicit: Boolean
) {


    var upVotes = 0
    var downVotes = 0

    fun upVote(){
        upVotes++
    }

    fun downVote(){
        downVotes++
    }
}