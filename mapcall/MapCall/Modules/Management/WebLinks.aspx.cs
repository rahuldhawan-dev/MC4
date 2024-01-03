using System;
using System.Web.UI.WebControls;
using MMSINC.Utilities.Permissions;
using MapCall.Controls;

namespace MapCall.Modules.Management
{
    public partial class WebLinks : TemplatedDetailsViewDataPageBase
    {
        #region Properties

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.DataLookups; }
        }
        #endregion

        #region Private Methods

        protected override void OnDetailsViewItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            base.OnDetailsViewItemInserting(sender, e);

            var url = e.Values["Url"].ToString();

            url = FixUrl(url);
            e.Values["Url"] = url;
        }

        /// <summary>
        /// Returns a url with http:// added to it if it's missing it.
        /// </summary>
        protected static string FixUrl(string url)
        {
            if (String.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            if (!url.Contains("://"))
            {
                url = "http://" + url;
            }

            Uri u;

            if (!Uri.TryCreate(url, UriKind.Absolute, out u))
            {
                return string.Empty;
            }

            return url;
        }

        /// <summary>
        /// CustomValidator on the DetailsView Url textbox for making sure a proper url is entered. 
        /// </summary>
        protected void rfvUrlOnServerValidate(object source, ServerValidateEventArgs args)
        {
            var url = FixUrl(args.Value);
            args.IsValid = (!String.IsNullOrWhiteSpace(url));
        }

        #endregion

    
    }
}
