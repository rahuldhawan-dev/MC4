using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents
{
    public class CreateEnvironmentalNonComplianceEvent : EnvironmentalNonComplianceEventViewModel
    {
        #region Constructors

        public CreateEnvironmentalNonComplianceEvent(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required, DropDown("", "OperatingCenter", "ActiveByStateIdOrAll", DependsOn = "State"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public override int? OperatingCenter { get; set; }
        
        #endregion

        #region Exposed Methods

        public override EnvironmentalNonComplianceEvent MapToEntity(EnvironmentalNonComplianceEvent entity)
        {
            base.MapToEntity(entity);
            entity.IssueYear = EventDate?.Year;
            return entity;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Responsibility = EnvironmentalNonComplianceEventResponsibility.Indices.TBD;
        }

        #endregion
    }
}
