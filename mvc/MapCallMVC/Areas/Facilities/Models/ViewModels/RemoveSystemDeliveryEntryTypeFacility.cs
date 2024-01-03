using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class RemoveFacilitySystemDeliveryEntryType : ViewModel<Facility>
    {
        #region Properties
        
        [Required, DoesNotAutoMap]
        public int? FacilitySystemDeliveryEntryTypeId { get; set; }

        #endregion

        #region Constructors

        public RemoveFacilitySystemDeliveryEntryType(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Facility MapToEntity(Facility entity)
        {
            entity.FacilitySystemDeliveryEntryTypes.Remove(entity.FacilitySystemDeliveryEntryTypes.SingleOrDefault(x => x.Id == FacilitySystemDeliveryEntryTypeId.Value));
            return entity;
        }

        #endregion
    }
}
