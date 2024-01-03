using System.ComponentModel;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchTrainingModulePositionGroupCommonName : SearchSet<TrainingModulePositionGroupCommonNameReportItem>, ISearchTrainingModulePositionGroupCommonName 
    {
        [DropDown]
        public int? TrainingModule { get; set; }

        public bool? IsOSHARequirement { get; set; }

        [DisplayName("Common Name")]
        [MultiSelect]
        public int[] Ids { get; set; }

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            mapper.MappedProperties["Ids"].ActualName = "Id";
        }
    }
}