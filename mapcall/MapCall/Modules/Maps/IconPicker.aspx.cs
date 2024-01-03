using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Modules.Maps
{
    public partial class IconPicker : Page
    {
        #region Constants

        private struct FormatStrings
        {
            /// <summary>
            /// Requires 1 value, a string representing the image's filename.
            /// </summary>
            public const string ICON_URL = "~/images/{0}";
            /// <summary>
            /// Requires 1 value, the iconid that's been set by the user choosing an icon from the list.
            /// </summary>
            //public const string CHOOSE_IMAGE_SCRIPT = "window.top.document.img.src=this.src;window.top.document.hid.value={0};window.top.document.lightview.hide();";
            public const string CHOOSE_IMAGE_SCRIPT = "selectIcon(this.src, {0});";
        }

        private struct Queries
        {
            public const string GET_ICONS_QUERY = "SELECT [IconID], [IconURL], [Width], [Height] FROM [MapIcon]";
        }

        #endregion
        
        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateIconsTable();
        }
        
        #endregion
        
        #region Private Methods

        private void PopulateIconsTable()
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(Queries.GET_ICONS_QUERY, conn))
                {
                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (rdr.Read())
                        {
                            int iIconID = rdr.GetInt32(0);
                            string strIconURL = rdr.GetString(1);
                            int iWidth = rdr.GetInt32(2);
                            int iHeight = rdr.GetInt32(3);
                            TableRow tr = new TableRow();
                            TableCell tc = new TableCell();
                            ImageButton ib = new ImageButton() {
                                ImageUrl = string.Format(FormatStrings.ICON_URL, strIconURL),
                                Width = iWidth,
                                Height = iHeight,
                                OnClientClick = string.Format(FormatStrings.CHOOSE_IMAGE_SCRIPT, iIconID)
                            };

                            tc.Controls.Add(ib);
                            tr.Cells.Add(tc);
                            tblIcons.Rows.Add(tr);
                        }
                    }
                }
            }
        }
        
        #endregion
    }
}
