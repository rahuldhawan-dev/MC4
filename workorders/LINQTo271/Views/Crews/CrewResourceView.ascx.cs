using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.Crews
{
    public partial class CrewResourceView : WorkOrdersResourceView<Crew>
    {
        #region Control Declarations

        protected IDetailView<Crew> cdvCrew;
        protected IListView<Crew> clvCrews;
        protected IButton btnBackToList;

        #endregion

        #region Properties

        public override IButton BackToListButton
        {
            get { return btnBackToList; }
        }

        public override IDetailView<Crew> DetailView
        {
            get { return cdvCrew; }
        }

        public override IListView<Crew> ListView
        {
            get { return clvCrews; }
        }

        public override ISearchView<Crew> SearchView
        {
            get { return null; }
        }

        #endregion

        #region Exposed Methods

        public override void SetViewMode(ResourceViewMode newMode)
        {
            base.SetViewMode(newMode);
            ToggleList(false);
            ToggleDetail(false);
            ToggleSearch(false);
            ToggleBackToListButton(false);
            switch (newMode)
            {
                case ResourceViewMode.Search:
                    ToggleSearch(true);
                    break;
                case ResourceViewMode.Detail:
                    ToggleDetail(true);
                    ToggleBackToListButton(true);
                    break;
                default:
                    ToggleList(true);
                    break;
            }
        }

        #endregion
    }
}