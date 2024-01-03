using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Contractors.Models.ViewModels
{
    public class SearchContractorInsurance : SearchSet<ContractorInsurance>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(Contractor))]
        public int? Contractor { get; set; }
        
        public SearchString InsuranceProvider { get; set; }

        public bool? MeetsCurrentContractualLimits { get; set; }

        public DateRange EffectiveDate { get; set; }

        public DateRange TerminationDate { get; set; }
    }
}