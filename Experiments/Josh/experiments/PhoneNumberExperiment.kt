

class PhoneNumberExperiment
{
    override fun onCreate(instanceState: Bundle?)
    {
        super.onCreate(instanceState)
        setContentView(R.layout.fragment_gallery)
        val btn_sendNum = findViewById<Button>(R.id.button_id)
        btn_sendNum.setOnClickListener
        {
            val changeActivity = Intent(this, experiment1::class.java)
            changeActivity.putExtra("Phone Number: ", findViewById<EditText>(R.id.phone_number))
            startActivity(changeActivity)
        }
    }



}
