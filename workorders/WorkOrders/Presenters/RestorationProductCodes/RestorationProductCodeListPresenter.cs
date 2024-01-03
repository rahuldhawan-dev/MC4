using System.Web.UI.WebControls;
using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;
using WorkOrders.Views.RestorationProductCodes;

namespace WorkOrders.Presenters.RestorationProductCodes
{
    public class RestorationProductCodeListPresenter : ListPresenter<RestorationProductCode>
    {
        #region Constants

        public static string RECORD_DELETED = "Deleted";

        #endregion
        
        #region Private Members

        private IRestorationProductCodeListView _listView;

        #endregion

        #region Properties

        public IRestorationProductCodeListView RestorationProductCodeListView
        {
            get
            {
                if (_listView == null)
                    _listView = View as IRestorationProductCodeListView;
                return _listView;
            }
        }

        #endregion

        #region Constructors

        public RestorationProductCodeListPresenter(IListView<RestorationProductCode> view)
            : base(view)
        {
        }

        #endregion

        #region Exposed Methods

        public override void OnViewLoaded()
        {
            if (RestorationProductCodeListView != null)
            {
                RestorationProductCodeListView.DeleteCommand +=
                    View_DeleteCommand;
                RestorationProductCodeListView.InsertCommand +=
                    View_InsertCommand;
            }
        }
        
        #endregion

        #region Event Handlers

        protected void View_DeleteCommand(object sender, GridViewDeleteEventArgs e)
        {
            RestorationProductCodeListView.ErrorMessage = string.Empty;
            var rpc = Repository.Get(e.Keys["RestorationProductCodeID"]);
            if (!rpc.CanDelete)
            {
                RestorationProductCodeListView.ErrorMessage = rpc.DeletingErrorMessage;
                e.Cancel = true;
            }
            else
            {
                RestorationProductCodeListView.ErrorMessage = RECORD_DELETED;
            }
        }

        protected void View_InsertCommand(object sender, RestorationProductInsertEventArgs e)
        {
            Repository.InsertNewEntity(new RestorationProductCode { Code = e.Code});
        }

        #endregion
    }
}
