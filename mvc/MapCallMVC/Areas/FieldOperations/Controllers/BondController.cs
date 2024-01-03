using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;
using Permits.Data.Client.Repositories;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class BondController : ControllerBaseWithPersistence<Bond, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Private Members

        private IPermitsDataClientRepository<Permits.Data.Client.Entities.Bond> _permitsDataClientRepository;

        #endregion

        #region Properties

        public IPermitsDataClientRepository<Permits.Data.Client.Entities.Bond> PermitsBondRepository
        {
            get
            {
                if (_permitsDataClientRepository == null)
                {
                    var factory = _container.GetInstance<IPermitsRepositoryFactory>();
                    _permitsDataClientRepository = factory.GetRepository<Permits.Data.Client.Entities.Bond>(AuthenticationService.CurrentUser.DefaultOperatingCenter.PermitsUserName);
                }
                return _permitsDataClientRepository;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a bond object from the bond view model that can be used 
        /// to interact with the permit's api repository
        /// </summary>
        private Permits.Data.Client.Entities.Bond SetupBond(BondViewModel viewModel)
        {
            var stateRepo = _container.GetInstance<IStateRepository>();
            var countyRepo = _container.GetInstance<MapCall.Common.Model.Repositories.ICountyRepository>();

            var bond = new Permits.Data.Client.Entities.Bond
            {
                StateName = stateRepo.Find(viewModel.State.Value).Name,
                CountyName = countyRepo.Find(viewModel.County.Value).Name,
                ExpirationDate = viewModel.EndDate,
                StartDate = viewModel.StartDate,
                IsRecurring = viewModel.RecurringBond
            };
            
            if (viewModel.Town.HasValue)
            {
                var townRepo = _container.GetInstance<ITownRepository>();
                var town = townRepo.Find(viewModel.Town.Value);
                if (town != null)
                {
                    bond.MunicipalityName = town.ShortName;
                }
            }

            return bond;
        }

        private int SaveBond(BondViewModel model)
        {
            var bond = SetupBond(model);
            // try catch here and provide better message to end user if it fails
            // e.g. fails if town doesn't exist on the permit site
            var result = PermitsBondRepository.Save(bond);
            return result.Id;
        }

        /// <summary>
        /// Create a bond on the permit site if we have a road opening permit
        /// </summary>
        private void CreatePermitsBond(BondViewModel viewModel)
        {
            viewModel.PermitsBondId = (!viewModel.BondPurpose.HasValue || viewModel.BondPurpose.Value != BondPurpose.Indices.ROAD_OPENING_PERMIT)
                ? 0
                : SaveBond(viewModel);
        }

        /// <summary>
        /// Delete the permit bond
        /// </summary>
        /// <param name="permitsBondId">Id of the bond on the permit's site</param>
        private void DeletePermitsBond(int permitsBondId)
        {
            if (permitsBondId > 0)
                PermitsBondRepository.Delete(permitsBondId);
        }

        /// <summary>
        /// If we have a permitsBondId remove it from the permits site.
        /// Then create it if necessary
        /// </summary>
        private void UpdatePermitsBond(EditBond viewModel)
        {
            DeletePermitsBond(viewModel.PermitsBondId);
            CreatePermitsBond(viewModel);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
            this.AddDropDownData<BondType>();
            this.AddDropDownData<BondPurpose>();
            if (action == ControllerAction.New)
            {
                this.AddOperatingCenterDropDownData(x => x.IsActive);
            }
            else
            {
                this.AddOperatingCenterDropDownData();
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchBond search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchBond search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateBond(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateBond model)
        {
            // TODO: This really should be called after we know validation has succeeded.
            // Otherwise we're potentially creating a permit bond and then throwing an error
            // if validation fails.
            CreatePermitsBond(model);
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs
            {
                OnSuccess = () => RedirectToAction("Show", new { id = model.Id })
            });
        }
        
        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditBond>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditBond model)
        {
            // TODO: Why is this being called twice?  And why is validation not being checked first?
            UpdatePermitsBond(model);
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    UpdatePermitsBond(model);
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            var bond = Repository.Find(id)?.PermitsBondId;
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                OnSuccess = () => {
                    DeletePermitsBond(bond.Value);
                    return RedirectToAction("Search");
                }
            });
        }

        #endregion

		#region Constructors

        public BondController(ControllerBaseWithPersistenceArguments<IRepository<Bond>, Bond, User> args) : base(args) {}

		#endregion
    }
}