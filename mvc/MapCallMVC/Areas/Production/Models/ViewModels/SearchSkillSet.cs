using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SearchSkillSet : SearchSet<SkillSet>
    {
        #region Properties

        public SearchString Name { get; set; }
        public SearchString Abbreviation { get; set; }
        public bool? IsActive { get; set; }
        public SearchString Description { get; set; }

        #endregion
    }
}