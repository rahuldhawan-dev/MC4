using TechTalk.SpecFlow;

namespace RegressionTests.Steps
{
    [Binding]
    public class LoginPage
    {
        #region Constants

        protected internal struct ControlIDs
        {
            internal const string EMAIL_INPUT = "Email";
            internal const string PASSWORD_INPUT = "Password";
            internal const string LOGIN_BUTTON = "Login";
            internal const string LOGGED_IN_DISPLAY_NAME = "loginName";
        }

        #endregion
    }
}
