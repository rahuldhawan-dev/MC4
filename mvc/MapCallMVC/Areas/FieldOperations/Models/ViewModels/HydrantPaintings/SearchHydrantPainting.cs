using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.HydrantPaintings
{
    public class SearchHydrantPainting : SearchSet<HydrantPainting>
    {
        [EntityMustExist(typeof(HydrantPainting)), DoesNotAutoMap, Search(CanMap = false)]
        public int? EditPainting { get; set; }

        [DoesNotAutoMap, Search(CanMap = false)]
        public HydrantPainting EditPaintingObj { get; set; }

        [EntityMap, EntityMustExist(typeof(Hydrant))]
        public int? Hydrant { get; set; }
    }
}
