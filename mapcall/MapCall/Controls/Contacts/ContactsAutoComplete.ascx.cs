using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using MMSINC.DataPages;
using MapCall.Modules.Data.Contacts;

namespace MapCall.Controls.Contacts
{
    public partial class ContactsAutoComplete : UserControl
    {

        #region Structs

        private struct ControlStateKeys
        {
            public const string SELECTED_CONTACT_ID = "SelectedContactId";
        }

        #endregion

        #region Fields

        private string _selectedContactName;

        #endregion

        #region Properties

        public bool IsRequired
        {
            get { return rfvCac.Enabled; }
            set { rfvCac.Enabled = value; }
        }

        public int SelectedContactId
        {
            get { return String.IsNullOrWhiteSpace(hidAC.Value) ? 0 : int.Parse(hidAC.Value); }
            set { hidAC.Value = value.ToString(); }
        }

        public string SelectedContactName
        {
            get { return (!string.IsNullOrEmpty(_selectedContactName) ? _selectedContactName : "[None]"); }
            set { _selectedContactName = value; }
        }

        public string ValidationGroup
        {
            get { return rfvCac.ValidationGroup; }
            set { rfvCac.ValidationGroup = value; }
        }

        #endregion

        #region Private Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            IsRequired = false;

            this.Page.RegisterRequiresControlState(this);
        }

        protected override void LoadControlState(object savedState)
        {
            base.LoadControlState(savedState);

            var cs = new ControlState(savedState);
            this.SelectedContactId = cs.Get<int>(ControlStateKeys.SELECTED_CONTACT_ID);

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Add the script if this actually renders.
            var scriptPath = this.Page.ResolveClientUrl("~/Controls/Contacts/ContactsAutoComplete.js");

            ScriptManager.RegisterClientScriptInclude(this.Page, this.Page.GetType(), "ContactsAutoComplete.js", scriptPath);

            var args = new Dictionary<string, string>();
            args.Add("instanceId", ClientID);
            args.Add("hiddenId", hidAC.ClientID);

            var serializer = new JavaScriptSerializer();
            var script = string.Format("ContactsAutoComplete.initialize({0});", serializer.Serialize(args));

            // Have to use ScriptManager or else it won't work inside of UpdatePanels. 
            ScriptManager.RegisterStartupScript(this, this.GetType(), this.UniqueID, script, true);

            if (SelectedContactId > 0)
            {
                using (var cs = new ContactsService())
                {
                    // Get the contact's name to display. 
                    var contact = cs.SearchContactsById(SelectedContactId);

                    var firstName = contact["FirstName"] ?? string.Empty;
                    var middleInitial = contact["MiddleInitial"] ?? string.Empty;
                    var lastName = contact["LastName"] ?? string.Empty;

                    SelectedContactName = FormatContactName(firstName.ToString(),
                                                                                 middleInitial.ToString(),
                                                                                 lastName.ToString());
                }
            }
        }
        private static string FormatParameter(string param)
        {
            return HttpUtility.HtmlEncode(param.Trim());
        }

        public static string FormatContactName(string firstName, string middleInitial, string lastName)
        {
            var sb = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(lastName))
            {
                sb.Append(FormatParameter(lastName));
            }
            if (!String.IsNullOrWhiteSpace(firstName))
            {
                sb.Append(", ").Append(FormatParameter(firstName));
            }

            if (!String.IsNullOrWhiteSpace(middleInitial))
            {
                sb.Append(" ").Append(FormatParameter(middleInitial)).Append(".");
            }
            return sb.ToString();
        }

        protected override object SaveControlState()
        {
            var cs = new ControlState();
            cs.Add(ControlStateKeys.SELECTED_CONTACT_ID, SelectedContactId, 0);

            return cs.GetControlStateObject();
        }

        #endregion

    }
}