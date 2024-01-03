using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchWorkDescription : SearchSet<WorkDescription>
    {
        public string Description { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(AssetType))]
        public int? AssetType { get; set; }
    }
}