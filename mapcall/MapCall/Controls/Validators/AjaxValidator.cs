//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI.WebControls;

//namespace MapCall.Controls.Validators
//{
//    public class AjaxValidator : CustomValidator
//    {

//        // How would this work server-side if the properties are paths to a web service?
//        // How would it call the method? Sloppy reflection?
//        // -Ross 7/29/2011
//        // 
//        // Or maybe there could be just one webservice that's called that then uses reflection
//        // to call a method on a page or something. The property would then be a namespace/class and method. 
//        // Might be nicer than setting up a hundred web services. 

//        #region Constants

//        private const string CLIENT_FUNCTION = "uh";

//        #endregion


//        #region Properties



//        #endregion


//        #region Private Methods


//        protected override bool OnServerValidate(string value)
//        {
//            return base.OnServerValidate(value);
//        }

//        #endregion



//    }
//}