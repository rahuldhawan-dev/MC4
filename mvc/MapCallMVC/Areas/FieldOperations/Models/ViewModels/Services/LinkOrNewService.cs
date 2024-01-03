using System.Linq;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services
{
    public class LinkOrNewService
    {
        #region Properties

        public WorkOrder WorkOrder { get; set; }

        public IQueryable<Service> RelatedServices { get; set; }

        #endregion
    }
}
