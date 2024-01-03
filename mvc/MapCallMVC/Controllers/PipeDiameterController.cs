using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class PipeDiameterController : ControllerBaseWithPersistence<PipeDiameter, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;

        #endregion
        
        #region Show	

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion
        
        #region Index

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index()
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex());
                formatter.Excel(() => {
                    var results = Repository.GetAll();
                    return this.Excel(results);
                });
            });
        }

        #endregion
    
        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreatePipeDiameter(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreatePipeDiameter model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditPipeDiameter>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditPipeDiameter model)
        {
            return ActionHelper.DoUpdate(model);
        }
		
        #endregion

		#region Constructors

        public PipeDiameterController(ControllerBaseWithPersistenceArguments<IRepository<PipeDiameter>, PipeDiameter, User> args) : base(args) {}

		#endregion
    }
}