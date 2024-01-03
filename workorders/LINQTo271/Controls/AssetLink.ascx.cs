using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LINQTo271.Controls
{
    public partial class AssetLink : System.Web.UI.UserControl
    {
        public string AssetType { get;set; }
        public string AssetIdentifier { get; set; }
        public int AssetId { get; set; }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (AssetId == 0)
            {
                lnkAsset.Visible = false;
                if (!string.IsNullOrWhiteSpace(AssetIdentifier))
                {
                    lblAsset.Visible = true;
                    lblAsset.Text = AssetIdentifier;
                }
            }
            else
            {
                var midPath = "";
                switch (AssetType)
                {
                    case "Main Crossing":
                        midPath = "Facilities/";
                        break;
                    case "Valve":
                    case "Hydrant":
                    case "Sewer Opening":
                        midPath = "FieldOperations/";
                        break;
                    case "Storm/Catch":
                        AssetType = "StormWaterAsset";
                        break;
                }
                lnkAsset.NavigateUrl =
                    $"/Modules/mvc/{midPath}{AssetType.Replace(" ", "")}/Show/{AssetId}";
                lnkAsset.Text = AssetIdentifier;
            }
        }
    }
}