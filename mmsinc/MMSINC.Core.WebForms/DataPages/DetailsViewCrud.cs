using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages.Permissions;

namespace MMSINC.DataPages
{
    // TODO: DetailsViewCrudField's type information is being stored in ViewState. Find out why.
    // BUG: This isn't taking into account DataPageBase.IsReadOnlyPage. !!!

    public class DetailsViewCrudField : TemplateField
    {
        #region Properties

        public IDataPagePermissions PagePermissions { get; private set; }

        #endregion

        #region Constructors

        public DetailsViewCrudField(IDataPagePermissions perms)
        {
            if (perms == null)
            {
                throw new ArgumentNullException("perms");
            }

            PagePermissions = perms;
            ShowHeader = false;
            Init();
        }

        #endregion

        internal void Init()
        {
            ItemTemplate = new DetailsViewCrudTemplate(PagePermissions, DetailsViewCrudTemplateType.ItemTemplate);
            InsertItemTemplate =
                new DetailsViewCrudTemplate(PagePermissions, DetailsViewCrudTemplateType.InsertTemplate);
            EditItemTemplate = new DetailsViewCrudTemplate(PagePermissions, DetailsViewCrudTemplateType.EditTemplate);
        }
    }

    internal enum DetailsViewCrudTemplateType
    {
        ItemTemplate,
        InsertTemplate,
        EditTemplate
    }

    internal class DetailsViewCrudTemplate : ITemplate
    {
        #region Constants

        public const string DELETE_CONFIRM = "return confirm('Are you sure?');";

        #endregion

        #region Structs

        public struct COMMANDS
        {
            public const string CANCEL = "Cancel";
            public const string DELETE = "Delete";
            public const string EDIT = "Edit";
            public const string INSERT = "Insert";
            public const string UPDATE = "Update";
        }

        #endregion

        #region Properties

        public IDataPagePermissions PagePermissions { get; private set; }
        public DetailsViewCrudTemplateType TemplateType { get; private set; }

        #endregion

        public DetailsViewCrudTemplate(IDataPagePermissions perms, DetailsViewCrudTemplateType type)
        {
            PagePermissions = perms;
            TemplateType = type;
        }

        public void InstantiateIn(Control container)
        {
            var ph = new PlaceHolder();

            // The CreateXButton methods will return null
            // when a user does not have appropriate access.

            Action<Control> addChild = (lb) => { ph.Controls.Add(lb); };

            switch (TemplateType)
            {
                case DetailsViewCrudTemplateType.ItemTemplate:
                    addChild(CreateEditButton());
                    addChild(CreateButtonSpacer());
                    addChild(CreateDeleteButton());
                    break;

                case DetailsViewCrudTemplateType.InsertTemplate:
                    addChild(CreateInsertButton());
                    addChild(CreateButtonSpacer());
                    addChild(CreateCancelButton());
                    break;

                case DetailsViewCrudTemplateType.EditTemplate:
                    addChild(CreateUpdateButton());
                    addChild(CreateButtonSpacer());
                    addChild(CreateCancelButton());
                    break;
            }

            container.Controls.Add(ph);
        }

        internal static LinkButton BuildButton(bool isEnabled, bool causesValidation, string commandName)
        {
            var lb = new LinkButton();
            lb.Visible = isEnabled;
            lb.Enabled = isEnabled;
            lb.CausesValidation = causesValidation;
            lb.CommandName = commandName;
            lb.Text = commandName;
            return lb;
        }

        internal static Literal CreateButtonSpacer()
        {
            // Needed because the links need a character of space
            // between them. This is just to make it so it renders
            // exactly as it always has. We can change this layout
            // however we want.
            var l = new Literal();
            l.Text = " ";
            return l;
        }

        public LinkButton CreateCancelButton()
        {
            // Cancel buttons don't need any particular permissions
            // or template. They're basically a back button. 
            return BuildButton(true, false, COMMANDS.CANCEL);
        }

        public LinkButton CreateDeleteButton()
        {
            var delBtn = BuildButton(PagePermissions.DeleteAccess.IsAllowed, false, COMMANDS.DELETE);
            delBtn.OnClientClick = DELETE_CONFIRM;
            return delBtn;
        }

        public LinkButton CreateEditButton()
        {
            return BuildButton(PagePermissions.EditAccess.IsAllowed, false, COMMANDS.EDIT);
        }

        public LinkButton CreateInsertButton()
        {
            return BuildButton(true, true, COMMANDS.INSERT);
        }

        public LinkButton CreateUpdateButton()
        {
            return BuildButton(true, true, COMMANDS.UPDATE);
        }
    }
}
