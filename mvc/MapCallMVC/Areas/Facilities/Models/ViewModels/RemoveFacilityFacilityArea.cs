using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class RemoveFacilityFacilityArea : ViewModel<Facility>
    {
        #region Properties

        [DoesNotAutoMap]
        [Required, EntityMustExist(typeof(FacilityFacilityArea))]
        public int? FacilityFacilityArea { get; set; }

        #endregion

        #region Constructors

        public RemoveFacilityFacilityArea(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Facility MapToEntity(Facility entity)
        {
            entity.FacilityAreas.Remove(entity.FacilityAreas.Single(x => x.Id == FacilityFacilityArea.Value));
            return entity;
        }

        #endregion
    }
}
