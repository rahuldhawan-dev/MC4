using System;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class SewerOpeningConnectionController : ControllerBaseWithPersistence<SewerOpeningConnection, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddDropDownData<PipeMaterial>("SewerPipeMaterial");
            this.AddDropDownData<SewerTerminationType>();
            this.AddDropDownData<RecurringFrequencyUnit>("InspectionFrequencyUnit");
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id, int originalOpeningId)
        {
            var entity = Repository.Find(id);
            if (entity == null)
                return HttpNotFound(String.Format("Sewer Opening Connection with the id: {0} was not found.", id));

            SetLookupData(ControllerAction.Edit);
            this.AddDropDownData<ISewerOpeningRepository, SewerOpening>("UpstreamOpening",r => r.FindByTownId(entity.UpstreamOpening.Town.Id),sm => sm.Id,sm => sm.OpeningSuffix);
            this.AddDropDownData<ISewerOpeningRepository, SewerOpening>("DownstreamOpening", r => r.FindByTownId(entity.DownstreamOpening.Town.Id),sm => sm.Id,sm => sm.OpeningSuffix);

            return DoView("Edit",
                ViewModelFactory.BuildWithOverrides<EditSewerOpeningConnection, SewerOpeningConnection>(entity,
                    new {OriginalOpeningId = originalOpeningId}));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditSewerOpeningConnection model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs
            {
                OnSuccess = () => RedirectToAction("Show", "SewerOpening", new { area = "FieldOperations", id = model.OriginalOpeningId })
            });
        }

        #endregion

        #region Constructors

        public SewerOpeningConnectionController(ControllerBaseWithPersistenceArguments<IRepository<SewerOpeningConnection>, SewerOpeningConnection, User>args) : base(args) {}

        #endregion
    }
}