using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.SAP.Model.Entities;

namespace MapCallMVC.Areas.Customer.Models.ViewModels
{
    public class SearchSAPTechnicalMasterAccount : SearchSet<SAPTechnicalMasterAccount>
    {
        public string PremiseNumber { get; set; }
        
        // THIS IS SAP, So we send string values along to their web service
        [DropDown]
        public virtual string InstallationType { get; set; }
        public string Equipment { get; set; }
    }
}