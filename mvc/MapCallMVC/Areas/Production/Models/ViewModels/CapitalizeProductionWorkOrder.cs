using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class CapitalizeProductionWorkOrder : ViewModel<ProductionWorkOrder>
    {
        public CapitalizeProductionWorkOrder(IContainer container) : base(container) { }

        [Required]
        public string CapitalizationReason { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ProductionWorkOrderCancellationReason))]
            
        [RequiredWhen("CapitalizationCancelsOrder", ComparisonType.EqualTo, true)]
        public int? CancellationReason { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        public bool CapitalizationCancelsOrder { get; set; }

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            base.MapToEntity(entity);
            // 3.If there is no time charged on the order, Cancel the original order(Type 0020) and Create 0040 Type(Capital) order
            if (!entity.EmployeeAssignments.Any(x => x.DateEnded.HasValue))
            {
                CapitalizationCancelsOrder = true;
                entity.DateCancelled = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            }
            else // 4.If there is time charged (Employee assignment completed ) , Then the original order need to be completed. It should not be cancelled.
            {
                CapitalizationCancelsOrder = false;
                entity.DateCompleted = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                entity.CompletedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            }
            return entity;    
        }
    }
}