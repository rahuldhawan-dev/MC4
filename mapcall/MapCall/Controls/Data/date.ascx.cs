using System;
using System.Web.UI;

namespace MapCall.Controls.Data
{
    public partial class date : UserControl
    {
        public string StartDate
        {
            get 
            {
                // There's no need to do a null check. TextBox will never return
                // null. They always return string.Empty, even if you explicitely
                // set the property to null.

                return txtDateInstalledStart.Text;
                //if (!String.IsNullOrEmpty(txtDateInstalledStart.Text))
                //    return txtDateInstalledStart.Text;
                //else
                //    return String.Empty;
            }
        }
        public string EndDate
        {
            get 
            {
                return txtDateInstalledEnd.Text;
                //if (!String.IsNullOrEmpty(txtDateInstalledEnd.Text))
                //    return txtDateInstalledEnd.Text;
                //else
                //    return String.Empty;
            }
        }
        public string Operator
        {
            get { return ddlDateInstalledParam.SelectedValue; }
        }
        public string SelectedIndex
        {
            set
            {
                ddlDateInstalledParam.SelectedIndex = Int32.Parse(value);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // This onChange attribute gets stored to ViewState. If there's multiple, that's unneeded bloat. 
            ddlDateInstalledParam.Attributes.Add("onChange", string.Format("dateControlChange('{0}', '{1}');", tdDateInstalledStart.ClientID, ddlDateInstalledParam.ClientID));
            //ddlDateInstalledParam.Attributes.Add("onLoad", string.Format("dateControlChange('{0}', '{1}');", tdDateInstalledStart.ClientID, ddlDateInstalledParam.ClientID));
            ScriptManager.RegisterStartupScript(this, typeof(string), this.ClientID + "Script", string.Format("dateControlChange('{0}', '{1}');", tdDateInstalledStart.ClientID, ddlDateInstalledParam.ClientID), true);
        }

        /// <summary>
        /// This method automatically generates the SQL needed for the filter expression.
        /// </summary>
        /// <param name="fieldName">This needs to be the field name you are searching on.</param>
        /// <returns></returns>
        public string FilterExpression(string fieldName)
        {
            if (this.Operator != "BETWEEN")
            {
                return String.Format(" AND [{0}] {1} '{2}'", fieldName, this.Operator, this.EndDate);
            }
            else
            {
                //return String.Format(" AND [{0}] >= '{1}' AND [{0}] <= '{2}'", fieldName, this.StartDate, this.EndDate);
                return String.Format(" AND [{0}] >= '{1}' AND [{0}] < '{2}'", fieldName, this.StartDate, DateTime.Parse(this.EndDate).AddDays(1.0).ToString());
            }
        }
    }
}