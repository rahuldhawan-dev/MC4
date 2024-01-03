using System;
using System.Web.UI.WebControls;

namespace MapCall.Controls
{
    public class EmailValidator : RegularExpressionValidator
    {
        #region Consts

        private const string EMAIL_REGEX =
            @"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";

        #endregion

        public EmailValidator()
        {
            ValidationExpression = EMAIL_REGEX;
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CssClass = "required";
        }
    }
}