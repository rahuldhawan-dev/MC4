using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Controls;
using WorkOrders.Model;
using WorkOrders.Views.RestorationAccountingCodes;

namespace LINQTo271.Views.RestorationAccountingCodes
{
    public partial class RestorationAccountingCodeListView : WorkOrdersListView<RestorationAccountingCode>, IRestorationAccountingCodeListView
    {
        #region Constants

        public struct RestorationAccountingCodeParameterNames
        {
            public const string CODE = "Code",
                                SUBCODE = "SubCode";
        }

        public struct ControlIDs
        {
            public const string CODE_TEXTBOX = "txtCode",
                                SUBCODE_TEXTBOX = "txtSubCode";

        }

        #endregion
        
        #region Private Members

        private ITextBox _txtCode, _txtSubCode;

        #endregion
        
        #region Properties

        public override IListControl ListControl
        {
            get { return gvRestorationAccountingCodes; }
        }

        public String ErrorMessage { get; set; }

        public string Code
        {
            get
            {
                if (_txtCode == null)
                    _txtCode =
                        gvRestorationAccountingCodes.IFooterRow.FindIControl
                            <ITextBox>(ControlIDs.CODE_TEXTBOX);
                return _txtCode.Text;
            }
        }

        public string SubCode
        {
            get
            {
                if (_txtSubCode == null)
                    _txtSubCode =
                        gvRestorationAccountingCodes.IFooterRow.FindIControl
                            <ITextBox>(ControlIDs.SUBCODE_TEXTBOX);
                return _txtSubCode.Text;
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
            var rpc = RestorationAccountingCodeRepository.GetEntity(e.Keys["RestorationAccountingCodeID"]);
            RestorationAccountingCodeRepository.Update(rpc);
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
            InsertCommand(sender, new RestorationAccountingInsertEventArgs(Code, SubCode));
        }

        #endregion

        #region Exposed Methods

        public override void SetViewControlsVisible(bool visible)
        {
            // noop
        }

        #endregion

        #region Events

        public event RestorationAccountingCodeEventHandler DeleteCommand;
        public event RestorationAccountingCodeInsertEventHandler InsertCommand;

        #endregion

    }
}