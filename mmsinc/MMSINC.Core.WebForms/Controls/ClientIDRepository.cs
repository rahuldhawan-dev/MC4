using System;
using System.Text;
using System.Web.UI;
using MMSINC.Common;

namespace MMSINC.Controls
{
    public class ClientIDRepository : MvpUserControl, IControl
    {
        #region Constants

        public struct FormatStrings
        {
            public const string SCRIPT_TAG_FORMAT =
                                    "<script type=\"text/javascript\">{0}</script>",
                                ID_JSON_FORMAT =
                                    "{{serverID:'{0}',clientID:'{1}'}},",
                                CLIENT_ID_VAR = "var CLIENT_IDS = [{0}];";
        }

        private struct ScriptStrings
        {
            public static string CLIENT_ID_REPOSITORY =
                                     "var ClientIDRepository={" +
                                     "lookup:function(id){" +
                                     "for(var i = CLIENT_IDS.length - 1; i >= 0; --i) " +
                                     "if (CLIENT_IDS[i].serverID == id)" +
                                     "return CLIENT_IDS[i].clientID;" +
                                     "return null;" +
                                     "}," +
                                     "updateClientID:function(serverID,clientID){" +
                                     "for(var i = CLIENT_IDS.length - 1; i >= 0; --i){" +
                                     "if(CLIENT_IDS[i].serverID == serverID){" +
                                     "CLIENT_IDS[i].clientID = clientID;" +
                                     "return;" +
                                     "}" +
                                     "}" +
                                     "}" +
                                     "};",
                                 GET_SERVER_ELEMENT =
                                     "function getServerElement(id){{" +
                                     "id = ClientIDRepository.lookup(id);" +
                                     "return {0}('#' + id);" +
                                     "}}";
        }

        #endregion

        #region Properties

        public bool NoConflict { get; set; }

        #endregion

        #region Private Methods

        protected override void CreateChildControls()
        {
            Controls.Add(new LiteralControl(BuildScript()));
        }

        private string BuildScript()
        {
            return String.Format(FormatStrings.SCRIPT_TAG_FORMAT,
                GetClientIDVar() +
                ScriptStrings.CLIENT_ID_REPOSITORY +
                String.Format(ScriptStrings.GET_SERVER_ELEMENT, (NoConflict) ? "jQuery" : "$"));
        }

        private string GetClientIDVar()
        {
            return String.Format(FormatStrings.CLIENT_ID_VAR, GetClientIDList());
        }

        private string GetClientIDList()
        {
            return BuildClientIDList(Page).ToString().TrimEnd(',');
        }

        private StringBuilder BuildClientIDList(Control parent)
        {
            return BuildClientIDList(parent, null);
        }

        private StringBuilder BuildClientIDList(Control parent, StringBuilder sb)
        {
            sb = sb ?? new StringBuilder();
            foreach (Control ctrl in parent.Controls)
            {
                if (!String.IsNullOrEmpty(ctrl.ID) &&
                    !String.IsNullOrEmpty(ctrl.ClientID))
                    sb.AppendFormat(FormatStrings.ID_JSON_FORMAT, ctrl.ID,
                        ctrl.ClientID);
                if (ctrl.Controls.Count > 0)
                    BuildClientIDList(ctrl, sb);
            }

            return sb;
        }

        #endregion

        #region Private Static Methods

        private static ClientIDRepository FindClientIDRepositoryInControl(Control parent)
        {
            ClientIDRepository repo;
            foreach (Control ctrl in parent.Controls)
            {
                repo = ctrl as ClientIDRepository;
                if (repo == null && ctrl.Controls.Count > 0)
                    repo = FindClientIDRepositoryInControl(ctrl);
                if (repo != null)
                    return repo;
            }

            return null;
        }

        #endregion

        #region Exposed Static Methods

        public static ClientIDRepository FindClientIDRepositoryInPage(Page page)
        {
            return FindClientIDRepositoryInControl(page);
        }

        #endregion

        #region Event Handlers

        protected override void Page_Prerender(object sender, EventArgs e)
        {
            // noop.  overridden to prevent the base functionality of loading
            // a client script (this control does not need one).
        }

        #endregion
    }
}
