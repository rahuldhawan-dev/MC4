using System;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using WorkOrders.Model;
using WorkOrders.Views.RestorationProductCodes;

namespace LINQTo271.Views.RestorationProductCodes
{
    public partial class RestorationProductCodeListView : WorkOrdersListView<RestorationProductCode>, IRestorationProductCodeListView
    {
        #region Constants

        public struct RestorationAccountingCodeParameterNames
        {
            public const string CODE = "Code";
        }

        public struct ControlIDs
        {
            public const string CODE_TEXTBOX = "txtCode";
        }

        #endregion

        #region Private Members

        private ITextBox _txtCode;

        #endregion

        #region Properties

        public override IListControl ListControl
        {
            get { return gvRestorationProductCodes; }
        }

        public string ErrorMessage { get; set; }

        public string Code
        {
            get
            {
                if (_txtCode == null)
                    _txtCode =
                        gvRestorationProductCodes.IFooterRow.FindIControl
                            <ITextBox>(ControlIDs.CODE_TEXTBOX);
                return _txtCode.Text;
            }
        }

        #endregion

        #region Private Methods

        private void SetGridViewIndex(int index)
        {
            ((GridView)ListControl).EditIndex = index;
        }

        #endregion

        #region Event Handlers

        protected void ListControl_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var rpc = RestorationProductCodeRepository.GetEntity(e.Keys["RestorationProductCodeID"]);
            RestorationProductCodeRepository.Update(rpc);
            SetGridViewIndex(-1);
        }

        protected void ListControl_RowEditing(object sender, GridViewEditEventArgs e)
        {
            SetGridViewIndex(e.NewEditIndex);
        }

        protected void ListControl_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            SetGridViewIndex(-1);
        }

        protected void ListControl_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DeleteCommand(sender, e);
            lblError.Text = ErrorMessage;
        }

        protected void ListControl_RowInserting(object sender, EventArgs e)
        {
            InsertCommand(sender, new RestorationProductInsertEventArgs(Code));
        }

        #endregion

        #region Exposed Methods

        public override void SetViewControlsVisible(bool visible)
        {
            // noop
        }

        #endregion

        #region Events

        public event RestorationProductCodeEventHandler DeleteCommand;
        public event RestorationProductCodeInsertEventHandler InsertCommand;

        #endregion
    }
}