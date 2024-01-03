using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Events.Models.ViewModels
{
    public class SearchEventDocument : SearchSet<EventDocument>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Facility))]
        public virtual int? Facility { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EventType))]
        public virtual int? EventType { get; set; }

        #endregion
    }
}