using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Contractors.Models.ViewModels
{
    public class SearchContractorAgreement : SearchSet<ContractorAgreement>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(Contractor))]
        public int? Contractor { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ContractorWorkCategoryType)),
        View(ContractorAgreement.Display.CONTRACTOR_WORK_CATEGORY_TYPE)]
        public int? ContractorWorkCategoryType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ContractorAgreementStatusType)),
         View(ContractorAgreement.Display.CONTRACTOR_AGREEMENT_STATUS_TYPE)]
        public int? ContractorAgreementStatusType { get; set; }
    }
}