#region

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages;

#endregion

// TODO: Check that validation works on pages with this. The DetailsView fields may
//       need to have their own ValidationGroup. 

// TODO: Make sure the dvContacts edit/insert/whatever buttons are properly set to the correct
//       visibilty since using them in a template does not work.

namespace MapCall.Controls.Contacts
{

    // NAMING SCHEME:
    //
    // "Contact" - Relates specifically to the Contact table and data inside it.
    // "ContactFor" - Relates to a table that has a foreign key to a Contact and to something else.
    // "ContactTypesFor" - Relates to ContactTypes that are valid on the ContactFor table.

    public partial class ContactsView : UserControl
    {

        #region Constants

        private const string CONTACTID_PARAMETER = "ContactID";

        #endregion

        #region Structs

        private struct ControlStateKeys
        {
            // Serialization's more condensed when the key is not a string.
            public const string CURRENT_STATE = "CurrentState";
            public const string SELECTED_CONTACTID = "SelectedContactId";
            public const string CONTACTS_FOR_TABLE_KEY_VALUE = "ContactsForTableKeyValue";
        }

        #endregion

        #region Enums

        private enum State
        {
            Results = 0, // Default
            AddContactFor = 5,
        }

        #endregion

        #region Fields

        private string _contactsForTableKeyValue;
        private State _currentState;
        private int? _selectedContactId;

        #endregion

        #region Properties

        public string ContactsForDisplayName { get; set; }

        /// <summary>
        /// The name of the ContactTypesFor[Table] table that has a foreign key constraint on the ContactTypes table.
        /// </summary>
        public string ContactTypesForTableName { get; set; }

        /// <summary>
        /// The name of the table that contains Contacts for another table. Ie: ContractorContacts.
        /// </summary>
        public string ContactsForTableName { get; set; }

        /// <summary>
        /// The name of the key column used by the ContactsFor* table. Ie: ContractorID on ContractorContacts.
        /// </summary>
        public string ContactsForTableKeyName { get; set; }

        /// <summary>
        /// The value to look up in the ContactsForTable. 
        /// </summary>
        public string ContactsForTableKeyValue
        {
            get { return _contactsForTableKeyValue; }
            set
            {
                _contactsForTableKeyValue = value;

                dsContactResults.SelectParameters["ContactsForTableKeyValue"].DefaultValue = value;
            }

        }


        /// <summary>
        /// The name of the primary key column in the ContactsFor* table. ie: ContractorContactID. 
        /// </summary>
        public string ContactsForTablePrimaryKeyName { get; set; }

        private State CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;

                switch (value)
                {
                    case State.Results:
                        SelectedContactId = null;
                        break;

                    case State.AddContactFor:
                        break;
                }
            }
        }

        private int? SelectedContactId
        {
            get { return _selectedContactId; }
            set
            {
                _selectedContactId = value;
            }
        }

        #endregion

        #region Private Methods

        #region Static

        private static void VerifyDynamicSelectParameters(string parameterName, string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new NullReferenceException("Parameter not set: " + parameterName);
            }
        }

        private static string FormatParameter(string param)
        {
            return HttpUtility.HtmlEncode(param.Trim());
        }

        protected static string FormatContactName(string firstName, string middleInitial, string lastName)
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

        protected static string FormatAddress(string address1, string address2, string city, string state, string zip)
        {
            const string br = "<br />";

            var sb = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(address1))
            {
                sb.Append(FormatParameter(address1)).Append(br);
            }
            if (!String.IsNullOrWhiteSpace(address2))
            {
                sb.Append(FormatParameter(address2)).Append(br);
            }

            if (!String.IsNullOrWhiteSpace(city))
            {
                sb.Append(FormatParameter(city)).Append(", ");
            }

            if (!String.IsNullOrWhiteSpace(state))
            {
                sb.Append(FormatParameter(state));
            }

            if (!String.IsNullOrWhiteSpace(zip))
            {
                sb.Append(" ").Append(FormatParameter(zip));
            }

            return sb.ToString();
        }

        #endregion

        private string FormatSqlCommand(string command)
        {
            VerifyDynamicSelectParameters("ContactsForTableName", ContactsForTableName);
            VerifyDynamicSelectParameters("ContactsForTableKeyName", ContactsForTableKeyName);
            VerifyDynamicSelectParameters("ContactTypesForTableName", ContactTypesForTableName);
            VerifyDynamicSelectParameters("ContactsForTablePrimaryKeyName", ContactsForTablePrimaryKeyName);

            var sb = new StringBuilder(command);
            sb.Replace("{ContactsForTableName}", ContactsForTableName);
            sb.Replace("{ContactsForTableKeyName}", ContactsForTableKeyName);
            sb.Replace("{ContactTypesForTableName}", ContactTypesForTableName);
            sb.Replace("{ContactsForTablePrimaryKeyName}", ContactsForTablePrimaryKeyName);

            return sb.ToString();
        }

        private bool IsContactForDataValid()
        {
            return (cacFindContact.SelectedContactId > 0);
        }


        #region Lifecycle

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Page.RegisterRequiresControlState(this);

            if (!IsPostBack)
            {
                // Needs to be set because LoadControlState isn't called
                // on non-postbacks.
                CurrentState = State.Results;
            }

            // Testing the UpdateProgress control. 
            // System.Threading.Thread.Sleep(1000);

            dsContactResults.SelectCommand = FormatSqlCommand(dsContactResults.SelectCommand);
            dsContactResults.DeleteCommand = FormatSqlCommand(dsContactResults.DeleteCommand);

            mcProdContactTypesFor.SelectCommand = FormatSqlCommand(mcProdContactTypesFor.SelectCommand);

            // Both keys are required. PrimaryKeyName for removing a ContactFor, ContactID for viewing Contact info.
            gvContacts.DataKeyNames = new string[] { ContactsForTablePrimaryKeyName, CONTACTID_PARAMETER };
        }

        protected override void LoadControlState(object savedState)
        {
            base.LoadControlState(savedState);

            var cs = new ControlState(savedState);
            ContactsForTableKeyValue = cs.Get<string>(ControlStateKeys.CONTACTS_FOR_TABLE_KEY_VALUE);
            CurrentState = cs.Get<State>(ControlStateKeys.CURRENT_STATE);
            SelectedContactId = cs.Get<int>(ControlStateKeys.SELECTED_CONTACTID);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), "ContactsView.js", Page.ResolveClientUrl("~/Controls/Contacts/Contacts.js"));

            // Set various panel visibilities based on the CurrentState. This needs to be done
            // during PreRender and not when the property is set due to LoadControlState not being
            // called during button click postbacks in updatepanels or some stupid crap.
            var value = CurrentState;
            pnlResults.Visible = (value == State.Results || value == State.AddContactFor);
            pnlAddContactFor.Visible = (value == State.AddContactFor);
            lblAddContactForError.Visible = (!string.IsNullOrEmpty(lblAddContactForError.Text));
           

        }

        protected override object SaveControlState()
        {
            var cs = new ControlState();
            cs.Add(ControlStateKeys.CONTACTS_FOR_TABLE_KEY_VALUE, ContactsForTableKeyValue, null);
            cs.Add(ControlStateKeys.CURRENT_STATE, CurrentState, State.Results);
            cs.Add(ControlStateKeys.SELECTED_CONTACTID, SelectedContactId.GetValueOrDefault(), 0);
            return cs.GetControlStateObject();
        }

        #endregion

        #endregion

        #region Event Handlers

        protected void gvContactsOnSelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentState = State.Results;

            var selectedKey = (DataKey)gvContacts.SelectedDataKey;

            if (selectedKey == null)
            {
                throw new NullReferenceException("SelectedKey is null for ContactsView control.");
            }

            var contactId = selectedKey[CONTACTID_PARAMETER].ToString();

            SelectedContactId = (!String.IsNullOrWhiteSpace(contactId) ? int.Parse(contactId) : 0);

        }

        #region Find Contact panel buttons

        protected void btnLinkContactOnClick(object sender, EventArgs e)
        {
            CurrentState = State.AddContactFor;
        }

        #endregion

        #region AddContactFor buttons

        protected void btnSaveContactForOnClick(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (IsContactForDataValid())
                {
                    const string commText =
                        @"INSERT INTO [{ContactsForTableName}] ([{ContactsForTableKeyName}], [ContactID], [ContactTypeID])
                            VALUES(@ContactsForTableKeyValue, @ContactID, @ContactTypeID)";

                    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
                    {
                        using (var com = conn.CreateCommand())
                        {
                            com.CommandText = FormatSqlCommand(commText);
                            com.Parameters.AddWithValue("ContactsForTableKeyValue", int.Parse(ContactsForTableKeyValue));
                            com.Parameters.AddWithValue("ContactID", cacFindContact.SelectedContactId);
                            com.Parameters.AddWithValue("ContactTypeID", int.Parse(ddlContactType.SelectedValue));

                            conn.Open();

                            com.ExecuteNonQuery();
                        }
                    }

                    //   dvAddContactFor.InsertItem(true);
                    // Do inserting things. 
                    CurrentState = State.Results;
                }
                else
                {
                    lblAddContactForError.Text = "You must select a Contact.";
                }
            }
        }

        protected void btnCancelContactForOnClick(object sender, EventArgs e)
        {
            CurrentState = State.Results;
            // Reset it so a second click on "Add Contact For" doesn't
            // remember the previously selected contact. 
            cacFindContact.SelectedContactId = 0;

        }
        protected void btnViewFullContactInfoOnClick(object sender, EventArgs e)
        {
            SelectedContactId = cacFindContact.SelectedContactId;
            Response.Redirect("~/modules/mvc/contact/show/" + SelectedContactId.Value);
        }
        #endregion

        #region Contact Details panel buttons

        protected void btnBackToResultsOnClick(object sender, EventArgs e)
        {
            CurrentState = State.Results;
        }

        #endregion

        #endregion

    }
}