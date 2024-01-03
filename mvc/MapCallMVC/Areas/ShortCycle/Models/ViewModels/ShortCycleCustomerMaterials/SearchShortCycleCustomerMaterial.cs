using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Validation;

namespace MapCallMVC.Areas.ShortCycle.Models.ViewModels.ShortCycleCustomerMaterials
{
    public class SearchShortCycleCustomerMaterial : SearchSet<ShortCycleCustomerMaterial>
    {
        [Required, EntityMustExist(typeof(Premise))]
        public int? Premise { get; set; }
    }
}
