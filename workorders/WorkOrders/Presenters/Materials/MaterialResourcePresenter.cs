using System;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Views.Materials;

namespace WorkOrders.Presenters.Materials
{
    public class MaterialResourcePresenter : WorkOrdersAdminResourcePresenter<Material>
    {
        #region Properties

        protected IMaterialSearchView MaterialSearchView
        {
            get { return (IMaterialSearchView)SearchView;}
        }

        protected IMaterialListView MaterialListView
        {
            get { return (IMaterialListView)ListView; }
        }

        #endregion

        #region Constructors

        public MaterialResourcePresenter(IResourceView view,
                                         IRepository<Material> repository)
            : base(view, repository) {}

        #endregion

        #region Event Handlers

        protected override void View_LoadComplete(object sender, EventArgs e)
        {
            if (ListView != null && ListView.Visible)
            {
                SetListViewData(SearchView.GenerateExpression());
                ListView.DataBind();
            }
        }

        #endregion
    }
}
