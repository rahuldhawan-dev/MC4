using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Controls;

namespace MapCall.Controls
{
    public partial class MaterialPicker : UserControl
    {
        #region Private Members

        protected MvpListBox lbMaterials;

        #endregion

        #region Properties

        public string SelectedValue
        {
            get { return hidMaterialID.Value; }
            set { hidMaterialID.Value = value; }
        }

        public string OperatingCenterID
        {
            get
            {
                if (ViewState["OperatingCenterID"] == null)
                    ViewState["OperatingCenterID"] = string.Empty;
                return ViewState["OperatingCenterID"].ToString();
            }
            set
            {
                ViewState["OperatingCenterID"] = value;
                hidOperatingCenterID.Value = value;
            }
        }

        #endregion

        #region Private Methods

        protected override void  OnPreRender(System.EventArgs e)
        {
 	         base.OnPreRender(e);
             Page.ClientScript.RegisterClientScriptInclude(
                 Page.GetType(), 
                 "MaterialPicker.js",
                 Page.ResolveClientUrl("~/Controls/MaterialPicker.js"));
             //Page.ClientScript.RegisterClientScriptBlock(typeof(String), "bar", "Sys.Application.add_load(function () { MaterialPicker.txtSearch_Keyup($('#txtSerch')); });", true);
        }
        
        #endregion
    }
}