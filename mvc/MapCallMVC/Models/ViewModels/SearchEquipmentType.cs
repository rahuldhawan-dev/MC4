using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallMVC.Models.ViewModels
{
    public class SearchEquipmentType : SearchSet<EquipmentType>
    {
        #region Properties

        public virtual int? Id { get; set; }
        public virtual string Abbreviation { get; set; }
        public virtual string Description { get; set; }
        public virtual int? EquipmentGroup { get; set; }

        #endregion
    }
}