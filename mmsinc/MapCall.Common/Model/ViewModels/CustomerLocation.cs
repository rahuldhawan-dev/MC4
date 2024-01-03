using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public interface ISearchCustomerLocation : ISearchSet<CustomerLocation>
    {
        #region Properties

        // This property is only used for logic inside the CustomerLocationRepository.Search method.
        [Search(CanMap = false)]
        bool? HasVerifiedCoordinate { get; set; }

        #endregion
    }
}
