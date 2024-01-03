using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace Contractors.Data.Models.Repositories {
    public interface ITapImageRepository : IAssetImageRepository<TapImage>
    {
        IEnumerable<TapImage> GetTapImagesForWorkOrder(WorkOrder entity);
    }
}