using System;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class EditProductionWorkOrderProductionPrerequisite : ViewModel<ProductionWorkOrderProductionPrerequisite>
    {
        #region Constructors

        public EditProductionWorkOrderProductionPrerequisite(IContainer container) : base(container) { }

        #endregion

        #region Properties

        //[DropDown, Required, EntityMap, EntityMustExist(typeof(ProductionPrerequisite))]
        //public int? ProductionPrerequisite { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        public ProductionWorkOrder ProductionWorkOrder { get; set; }
        
        [DateTimePicker]
        public DateTime? SatisfiedOn { get; set; }

        public bool? SkipRequirement { get; set; }

        [RequiredWhen("SkipRequirement",ComparisonType.EqualTo, true, ErrorMessage = "Please describe why the prerequisite is no longer required.")]
        public string SkipRequirementComments { get; set; }

        #endregion
    }
}