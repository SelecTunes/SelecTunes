package cs309.selectunes



class Song() {

    var upVotes = 0

    fun upVote(){
        upVotes++
    }

    var downVotes = 0

    fun downVote(){
        downVotes++
    }

    var songName = ""
    var id = ""
    var artistName = ""
    var albumArtSrc = ""
    var explicitBool = false

}