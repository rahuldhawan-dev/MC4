using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.Restorations
{
    public interface IRestorationResourceRPCView : IResourceRPCView<Restoration>
    {
        int WorkOrderID { get; }
    }
}
