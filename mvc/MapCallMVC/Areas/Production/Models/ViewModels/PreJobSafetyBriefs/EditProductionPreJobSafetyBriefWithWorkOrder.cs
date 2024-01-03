using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs
{
    public class EditProductionPreJobSafetyBriefWithWorkOrder : EditProductionPreJobSafetyBriefBase
    {
        // This should come initially from the querystring of /New
        [Required, Secured, EntityMap, EntityMustExist(typeof(ProductionWorkOrder))]
        public override int? ProductionWorkOrder { get; set; }

        [CheckBoxList]
        public override int[] Employees { get; set; }

        public EditProductionPreJobSafetyBriefWithWorkOrder(IContainer container) : base(container) { }
    }
}