using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Models.ViewModels.FireDistricts
{
    public class SearchFireDistrict : SearchSet<FireDistrict>
    {
        public virtual SearchString DistrictName { get; set; }

        [EntityMap, EntityMustExist(typeof(State)), DropDown]
        public virtual int? State { get; set; }
    }
}
