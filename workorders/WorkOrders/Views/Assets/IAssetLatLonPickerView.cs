using MMSINC.Data.Linq;
using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.Assets
{
    public interface IAssetLatLonPickerView : IDetailView<Asset>
    {
        #region Properties

        IRepository<Valve> ValveRepository { get; }
        IRepository<Hydrant> HydrantRepository { get; }
        IRepository<SewerOpening> SewerOpeningRepository { get; }
        IRepository<StormCatch> StormCatchRepository { get; }

        #endregion
    }
}
