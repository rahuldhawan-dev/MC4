using System;
using System.Text;
using System.Web.UI;
using MMSINC.ClassExtensions;

namespace MMSINC.Controls
{
    public class MvpUpdatePanel : UpdatePanel, IUpdatePanel
    {
        #region Constants

        public const string UPDATE_CLIENT_ID_SCRIPT_KEY = "_updateClientIDs";

        private struct FormatStrings
        {
            public const string UPDATE_CLIENT_ID =
                "ClientIDRepository.updateClientID('{0}', '{1}');";
        }

        #endregion

        #region Private Members

        private bool? _pageHasClientIDRepository;

        #endregion

        #region Properties

        public bool PageHasClientIDRepository
        {
            get
            {
                if (_pageHasClientIDRepository == null)
                    _pageHasClientIDRepository = FindClientIDRepository();
                return _pageHasClientIDRepository.Value;
            }
        }

        #endregion

        #region Private Methods

        private bool FindClientIDRepository()
        {
            return (ClientIDRepository.FindClientIDRepositoryInPage(Page) !=
                    null);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (Page.IsPostBack && PageHasClientIDRepository)
            {
                AddClientIDUpdateScript();
            }
        }

        private void AddClientIDUpdateScript()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), UPDATE_CLIENT_ID_SCRIPT_KEY,
                BuildClientIDUpdateScript(), true);
            //            ContentTemplateContainer.Controls.Add(new LiteralControl(BuildClientIDUpdateScript()));
        }

        private string BuildClientIDUpdateScript()
        {
            return
                BuildClientIDUpdateList(ContentTemplateContainer).ToString();
        }

        #endregion

        #region Private Static Methods

        private static StringBuilder BuildClientIDUpdateList(Control parent)
        {
            return BuildClientIDUpdateList(parent, null);
        }

        private static StringBuilder BuildClientIDUpdateList(Control parent, StringBuilder sb)
        {
            sb = sb ?? new StringBuilder();
            foreach (Control ctrl in parent.Controls)
            {
                if (!String.IsNullOrEmpty(ctrl.ID) &&
                    !String.IsNullOrEmpty(ctrl.ClientID))
                    sb.AppendFormat(FormatStrings.UPDATE_CLIENT_ID, ctrl.ID,
                        ctrl.ClientID);
                if (ctrl.Controls.Count > 0)
                    BuildClientIDUpdateList(ctrl, sb);
            }

            return sb;
        }

        #endregion

        #region Exposed Methods

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            return
                ControlExtensions.FindControl
                    <TControl>(this, id);
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            return
                ControlExtensions.FindIControl
                    <TIControl>((Control)this, id);
        }

        #endregion
    }
}
