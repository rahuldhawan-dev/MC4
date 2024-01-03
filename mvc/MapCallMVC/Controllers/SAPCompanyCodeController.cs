using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    [RequiresAdmin]
    public class SAPCompanyCodeController : EntityLookupControllerBase<IRepository<SAPCompanyCode>, SAPCompanyCode>
    {
        public SAPCompanyCodeController(ControllerBaseWithPersistenceArguments<IRepository<SAPCompanyCode>, SAPCompanyCode, User> args) : base(args) {}
    }
}