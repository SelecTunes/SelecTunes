import kotlin.os.Message

class experiment1 implements Runnable
{

    fun main()
    {
        Handler handler  = new Handler()
        Message mess = obtainMessage(doubleGiven(2))
        handler.sendMessageAtFrontOfQueue(mess)
    }

    fun doubleGiven(given: Int): Int
    {
        var returnVal = 0
        if(given > 10)
        {
            returnVal = 69
        }
        else
        {
            returnVal = given * 2
        }
        return returnVal
    }



}