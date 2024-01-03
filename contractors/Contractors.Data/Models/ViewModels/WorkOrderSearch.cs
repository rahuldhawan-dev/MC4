using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace Contractors.Data.Models.ViewModels
{
    public interface IWorkOrderSearch : ISearchSet<WorkOrder>
    {
        #region Abstract Properties

        int? Id { get; }

        [Search(CanMap = false)] // This must be manually mapped
        int[] DocumentType { get; set; }

        #endregion

        #region Abstract Methods

        bool QueryIsNull();
        bool NonWorkOrderNumberFieldsAreNull();

        #endregion
    }
}
