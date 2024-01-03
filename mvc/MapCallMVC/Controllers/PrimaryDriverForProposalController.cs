using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class PrimaryDriverForProposalController :
        EntityLookupControllerBase<IRepository<PrimaryDriverForProposal>, PrimaryDriverForProposal>
    {
        public const RoleModules ROLE = UnionContractProposalController.ROLE;

        public PrimaryDriverForProposalController(
            ControllerBaseWithPersistenceArguments
                <IRepository<PrimaryDriverForProposal>, PrimaryDriverForProposal, User> args) : base(args) {}

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete), RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}