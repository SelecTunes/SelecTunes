mport kotlin.os.Message

class experiment1
{

    override fun onCreate(instanceState: Bundle?)
    {
        super.onCreate(instanceState)
        setContentView(R.layout.fragment_home)
        val textBox = findViewById(R.id.text_home)
        textBox.text = changeActivity.getStringExtra("Phone Number:")
    }



}