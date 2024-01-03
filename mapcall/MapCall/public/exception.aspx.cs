using System;
using System.Web.UI;

namespace MapCall.public1
{
	public partial class exception : Page
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            throw new Exception();
        }
	}
}
