using System;
using System.Web.UI.WebControls;
using MMSINC;

namespace MapCall.Controls.HR
{
    public partial class Facility : MMSINC.UserControl.DataElement
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            DataElementName = "Facility";
            DataElementParameterName = "RecordID";
        }
        protected void dsImages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var imgIcon = (Image)Utility.GetFirstControlInstance(DetailsView1, "imgIcon");
                var btnIconCancel = (Button)Utility.GetFirstControlInstance(DetailsView1, "btnIconCancel");
                var txtIcon = (HiddenField)Utility.GetFirstControlInstance(DetailsView1, "txtIcon");
                var gvImages = (GridView)Utility.GetFirstControlInstance(DetailsView1, "gvImages");
                if (gvImages != null)
                {
                    var img = (WebControl)e.Row.Cells[0].Controls[0];
                    if (img != null)
                    {
                        img.Attributes.Add("onclick",
                            String.Format(@"
                                    document.getElementById('{0}').src=this.src;
                                    document.getElementById('{1}').click();
                                    document.getElementById('{2}').value = '{3}';
                                ",
                                imgIcon.ClientID,
                                btnIconCancel.ClientID,
                                txtIcon.ClientID,
                                gvImages.DataKeys[e.Row.DataItemIndex].Value.ToString()
                            )
                        );
                    }
                }
            }
        }
    }
}