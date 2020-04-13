package cs309.selectunes.activities

import androidx.test.rule.ActivityTestRule
import cs309.selectunes.services.AuthService
import cs309.selectunes.utils.JsonUtils
import org.junit.Assert
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.mockito.Mockito

class RegisterActivityTest {

    @get:Rule
    val activityTestRule = ActivityTestRule(RegisterActivity::class.java)

    private lateinit var authService: AuthService
    private lateinit var registerActivity: RegisterActivity

    private lateinit var responses: BooleanArray

    @Before
    fun setup() {
        registerActivity = activityTestRule.activity
        authService = Mockito.mock(AuthService::class.java)
        responses = BooleanArray(3)
    }

    @Test
    fun testLoginParseJson() {
        Mockito.`when`(authService.register("jackgold@iastate.edu", "yes", "yes", registerActivity))
            .then {
                responses.set(0, JsonUtils.parseRegisterResponse(null, null, 200))
                responses.set(1, JsonUtils.parseRegisterResponse(null, null, 400))
                responses.set(2, JsonUtils.parseRegisterResponse(null, null, 500))
            }
        authService.register("jackgold@iastate.edu", "yes", "yes", registerActivity)
        Assert.assertTrue(responses[0])
        Assert.assertFalse(responses[1])
        Assert.assertFalse(responses[2])
    }
}