using System;
using System.Web.Mvc;
using MapCall.Common.Controllers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Permissions;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCallMVC.Controllers
{
    // NOTE: If you want a different role, you're gonna have to inherit from this controller and change it.
    [ActionHelperViewVirtualPathFormat("~/Views/EntityLookup/{0}.cshtml")]
    public abstract class EntityLookupControllerBase<TRepository, TEntityLookup, TViewModel> : ControllerBaseWithPersistence<TRepository, TEntityLookup, User>, IControllerOfRoles
        where TEntityLookup : class, IEntityLookup, new()
        where TRepository : class, IRepository<TEntityLookup>
        where TViewModel : EntityLookupViewModel<TEntityLookup>
    {
        #region Constructors

        public EntityLookupControllerBase(ControllerBaseWithPersistenceArguments<TRepository, TEntityLookup, User> args) : base(args) {}

        #endregion

        #region Exposed Methods

        [NonAction]
        public virtual RoleModules GetDynamicRoleModuleForAction(string action)
        {
            // NOTE: action param is only useful for inheritors. This base class doesn't care.
            return RoleModules.FieldServicesDataLookups;
        }

        [HttpGet, DynamicRequiresRole(RoleActions.Read)]
        public virtual ActionResult Index()
        {
            // I don't think this controller needs a search since these tables usually only have a handful of rows.
            return ActionHelper.DoIndex();
        }

        [HttpGet, DynamicRequiresRole(RoleActions.Read)]
        public virtual ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, DynamicRequiresRole(RoleActions.Add)]
        public virtual ActionResult New()
        {
            return ActionHelper.DoNew((TViewModel)Activator.CreateInstance(typeof(TViewModel), _container));
        }

        [HttpPost]
        [DynamicRequiresRole(RoleActions.Add)]
        [RequiresSecureForm]
        public ActionResult Create(TViewModel viewModel)
        {
            return ActionHelper.DoCreate(viewModel);
        }

        [HttpGet, DynamicRequiresRole(RoleActions.Edit)]
        public virtual ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<TViewModel>(id);
        }

        [HttpPost]
        [DynamicRequiresRole(RoleActions.Edit)]
        [RequiresSecureForm]
        public virtual ActionResult Update(TViewModel viewModel)
        {
            return ActionHelper.DoUpdate(viewModel);
        }

        #endregion
    }

    public abstract class EntityLookupControllerBase<TRepository, TEntityLookup> : EntityLookupControllerBase<TRepository, TEntityLookup, EntityLookupViewModel<TEntityLookup>>
        where TRepository : class, IRepository<TEntityLookup>
        where TEntityLookup : EntityLookup, new()
    {
        #region Constructors

        public EntityLookupControllerBase(ControllerBaseWithPersistenceArguments<TRepository, TEntityLookup, User> args) : base(args) {}

        #endregion
    }

    [GenericControllerName]
    public sealed class EntityLookupController<TRepository, TEntityLookup> : EntityLookupControllerBase<TRepository, TEntityLookup>
        where TEntityLookup : EntityLookup, new()
        where TRepository : class, IRepository<TEntityLookup>
    {
        public EntityLookupController(ControllerBaseWithPersistenceArguments<TRepository, TEntityLookup, User> args) : base(args) {}
    }
}
