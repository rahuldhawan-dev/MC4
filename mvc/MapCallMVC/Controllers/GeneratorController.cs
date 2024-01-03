using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class GeneratorController : ControllerBaseWithPersistence<IRepository<Generator>, Generator, User> 
    {
        #region Constants

        public const string NOT_FOUND = "Generator Detail with the id '{0}' was not found.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddDropDownData<Equipment>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<FuelType>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<EmergencyPowerType>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<IEquipmentManufacturerRepository, EquipmentManufacturer>("EngineManufacturer", f => f.GetAllSorted().Where(z => z.EquipmentType.Id == EquipmentType.Indices.ENG), f => f.Id, f => f.Description);
                    this.AddDropDownData<IEquipmentManufacturerRepository, EquipmentManufacturer>("GeneratorManufacturer", f => f.GetAllSorted().Where(z => z.EquipmentType.Id == EquipmentType.Indices.GEN), f => f.Id, f => f.Description);
                    break;
            }
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Add)]
        public ActionResult New(int? equipmentId = null)
        {
            var model = new CreateGenerator(_container);
            if (equipmentId.HasValue)
                model.Equipment = equipmentId;

            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Add)]
        public ActionResult Create(CreateGenerator model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => RedirectToAction("Show", "Equipment", new { area = "", id = model.Equipment})
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditGenerator>(id);
        }

        [HttpPost, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Edit)]
        public ActionResult Update(EditGenerator model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "Equipment", new { area = "", id = model.Equipment })
            });
        }

        #endregion

        public GeneratorController(ControllerBaseWithPersistenceArguments<IRepository<Generator>, Generator, User> args) : base(args) {}
    }
}
