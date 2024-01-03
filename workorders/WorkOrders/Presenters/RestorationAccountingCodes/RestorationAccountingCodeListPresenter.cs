using System.Web.UI.WebControls;
using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;
using WorkOrders.Views.RestorationAccountingCodes;

namespace WorkOrders.Presenters.RestorationAccountingCodes
{
    public class RestorationAccountingCodeListPresenter : ListPresenter<RestorationAccountingCode>
    {
        #region Constants

        public static string RECORD_DELETED = "Deleted";

        #endregion
        
        #region Private Members

        private IRestorationAccountingCodeListView _listView;

        #endregion

        #region Properties

        public IRestorationAccountingCodeListView RestorationAccountingCodeListView
        {
            get
            {
                if (_listView == null)
                    _listView = View as IRestorationAccountingCodeListView;
                return _listView;
            }
        }

        #endregion

        #region Constructors

        public RestorationAccountingCodeListPresenter(IListView<RestorationAccountingCode> view)
            : base(view)
        {
        }

        #endregion

        #region Exposed Methods

        public override void OnViewLoaded()
        {
            if (RestorationAccountingCodeListView != null)
            {
                RestorationAccountingCodeListView.DeleteCommand +=
                    View_DeleteCommand;
                RestorationAccountingCodeListView.InsertCommand +=
                    View_InsertCommand;
            }
        }
        
        #endregion

        #region Event Handlers

        protected void View_DeleteCommand(object sender, GridViewDeleteEventArgs e)
        {
            RestorationAccountingCodeListView.ErrorMessage = string.Empty;
            var rpc = Repository.Get(e.Keys["RestorationAccountingCodeID"]);
            if (!rpc.CanDelete)
            {
                RestorationAccountingCodeListView.ErrorMessage = rpc.DeletingErrorMessage;
                e.Cancel = true;
            }
            else
            {
                RestorationAccountingCodeListView.ErrorMessage = RECORD_DELETED;
            }
        }

        protected void View_InsertCommand(object sender, RestorationAccountingInsertEventArgs e)
        {
            Repository.InsertNewEntity(new RestorationAccountingCode() {
                Code = e.Code,
                SubCode = e.SubCode
            });
        }

        #endregion
    }
}
