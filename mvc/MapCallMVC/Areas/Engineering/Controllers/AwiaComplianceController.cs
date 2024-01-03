using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Engineering.Models.ViewModels.AwiaCompliance;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Engineering.Controllers
{
    [DisplayName("AWIA Compliances")]
    public class AwiaComplianceController : ControllerBaseWithPersistence<AwiaCompliance, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EngineeringRiskRegister;

        #endregion

        #region Constructors

        public AwiaComplianceController(ControllerBaseWithPersistenceArguments<IRepository<AwiaCompliance>, AwiaCompliance, User> args) : base(args) { }

        #endregion

        #region Public Methods

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchAwiaCompliance search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchAwiaCompliance search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(e => new {
                        e.Id,
                        e.State,
                        e.OperatingCenter,
                        PWSID = e.PublicWaterSupplies.ToString(),
                        e.CertificationType,
                        EnteredBy = e.CreatedBy?.FullName,
                        CertifiedBy = e.CertifiedBy?.FullName,
                        e.DateSubmitted,
                        e.DateAccepted,
                        e.RecertificationDue,
                        Notes = string.Join(", ", e.AwiaComplianceNotes.Select(x => x.Note?.Text))
                    });
                    return this.Excel(results);
                });
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateAwiaCompliance>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateAwiaCompliance viewModel)
        {
            return ActionHelper.DoCreate(viewModel);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditAwiaCompliance>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditAwiaCompliance viewModel)
        {
            return ActionHelper.DoUpdate(viewModel);
        }

        #endregion

        #endregion
    }
}