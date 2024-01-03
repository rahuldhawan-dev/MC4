using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.Restorations
{
    public interface IRestorationDetailView : IDetailView<Restoration>
    {
        int WorkOrderID { get; set; }
    }
}
