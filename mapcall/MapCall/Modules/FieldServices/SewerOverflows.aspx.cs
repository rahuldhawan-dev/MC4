using System;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.ClassExtensions.IOrderedDictionaryExtensions;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using mod = MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls.Data;

namespace MapCall.Modules.FieldServices
{
    public partial class SewerOverflows : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Mvc/FieldOperations/SewerOverflow/Search");
        }
    }
}
