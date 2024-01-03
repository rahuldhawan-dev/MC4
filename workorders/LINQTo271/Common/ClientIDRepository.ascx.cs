using System;
using System.Text;
using System.Web.UI;

namespace LINQTo271.Common
{
    public partial class ClientIDRepository : UserControl
    {
        #region Constants

        private const string ID_JSON_FORMAT =
            "{{serverID:'{0}',clientID:'{1}'}},";

        #endregion

        #region Properties

        protected string ClientIDList
        {
            get { return GetClientIDList(); }
        }

        #endregion

        #region Private Methods

        private string GetClientIDList()
        {
            return BuildClientIDList(Parent).TrimEnd(",".ToCharArray());
        }

        private string BuildClientIDList(Control parent)
        {
            var sb = new StringBuilder();
            foreach (Control ctrl in parent.Controls)
            {
                if (!String.IsNullOrEmpty(ctrl.ID) &&
                    !String.IsNullOrEmpty(ctrl.ClientID))
                    sb.AppendFormat(ID_JSON_FORMAT, ctrl.ID, ctrl.ClientID);
                if (ctrl.Controls.Count > 0)
                    sb.Append(BuildClientIDList(ctrl));
            }

            return sb.ToString();
        }

        #endregion
    }
}