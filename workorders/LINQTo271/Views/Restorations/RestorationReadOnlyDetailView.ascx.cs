using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.Restorations
{
    public partial class RestorationReadOnlyDetailView : WorkOrdersDetailView<Restoration>
    {
        #region Control Declarations

        //private IDetailControl fvRestoration;
        //private IObjectContainerDataSource odsRestoration;

	    #endregion

        #region Properties

        public override IDetailControl DetailControl
        {
            get { return fvRestoration; }
        }

        public override IObjectContainerDataSource DataSource
        {
            get { return odsRestoration; }
        }

        #endregion
    }
}