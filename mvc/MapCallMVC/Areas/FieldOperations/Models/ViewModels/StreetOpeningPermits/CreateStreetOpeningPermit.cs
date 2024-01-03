using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.StreetOpeningPermits
{
    public class CreateStreetOpeningPermit : StreetOpeningPermitViewModel
    {
        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        #endregion

        #region Constructors

        public CreateStreetOpeningPermit(IContainer container) : base(container) { }

        #endregion
    }
}
