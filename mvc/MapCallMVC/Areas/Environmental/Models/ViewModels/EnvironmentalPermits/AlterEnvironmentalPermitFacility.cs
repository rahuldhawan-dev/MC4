using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class AlterEnvironmentalPermitFacility : EnvironmentalPermitViewModel
    {
        #region Properties

        [Required, EntityMustExist(typeof(Facility)), DoesNotAutoMap]
        public virtual int FacilityId { get; set; }

        #endregion

        #region Constructors

        public AlterEnvironmentalPermitFacility(IContainer container) : base(container) {}

        #endregion
    }
}