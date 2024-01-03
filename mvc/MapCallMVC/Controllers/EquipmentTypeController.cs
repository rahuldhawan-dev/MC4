using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate.Util;
using StructureMap.Graph;

namespace MapCallMVC.Controllers
{
    public class EquipmentTypeController : ControllerBaseWithPersistence<EquipmentType, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionEquipment;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Show:
                    this.AddDropDownData<EquipmentCharacteristicFieldType>("FieldType", i => i.Id, i => i.DataType);
                    this.AddDropDownData<UnitOfMeasure>("UnitOfMeasure", i => i.Id, i => i.Description);
                    break;
            }
            
            base.SetLookupData(action);
        }

        #endregion

        #region Search/Index/Show

        [RequiresRole(ROLE)]
        [HttpGet]
        public ActionResult Show(int id, bool pdf = true)
        {
            return this.RespondTo(format => {
                format.View(() => ActionHelper.DoShow(id));
                format.Pdf(() => ActionHelper.DoPdf(id));
            });
        }

        [RequiresRole(ROLE)]
        [HttpGet]
        public ActionResult Index(SearchEquipmentType search)
        {
            search.EnablePaging = false;
            return ActionHelper.DoIndex(search);
        }

        [HttpGet]
        public ActionResult ByFacilityId(int facilityId) // I'm not sure why this was removed from this controller so I put it back
        {
            var result = new CascadingActionResult();
            result.TextField = nameof(EquipmentType.Description);
            result.ValueField = nameof(EquipmentType.Id);
            var facility = _container.GetInstance<RepositoryBase<Facility>>().Where(x => x.Id == facilityId).FirstOrDefault();
            if (facility?.Equipment != null && facility.Equipment.Count > 0)
            {
                result.Data = facility.Equipment.Where(x => x.EquipmentType != null).Select(x => x.EquipmentType).Distinct();
            }
            else
            {
                result.Data = Enumerable.Empty<EquipmentType>();
            }

            return result;
        }

        [HttpGet]
        public ActionResult ByFacilityIdAndSometimesProductionWorkOrder(int facilityId, int? productionWorkOrder)
        {
            var result = new CascadingActionResult();
            result.TextField = nameof(EquipmentType.Description);
            result.ValueField = nameof(EquipmentType.Id);
            var facility = _container.GetInstance<RepositoryBase<Facility>>().Where(x => x.Id == facilityId).FirstOrDefault();

            if (productionWorkOrder.HasValue)
            {
                var productionWo = _container.GetInstance<RepositoryBase<ProductionWorkOrder>>().Find(productionWorkOrder.Value);
                result.Data = productionWo.Equipments.Where(x => x.Equipment != null).Select(x => x.Equipment.EquipmentPurpose.EquipmentType).Distinct();
            }
            else if (facility?.Equipment != null && facility.Equipment.Count > 0)
            {
                result.Data = facility.Equipment.Where(x => x.EquipmentType != null).Select(x => x.EquipmentType).Distinct();
            }
            else
            {
                result.Data = Enumerable.Empty<EquipmentType>();
            }
            return result;
        }

        [HttpGet]
        public ActionResult ByFacilityIds(int[] facilityIds)
        {
            var result = new CascadingActionResult();
            result.TextField = nameof(EquipmentType.Description);
            result.ValueField = nameof(EquipmentType.Id);
            // This is using RepositoryBase<Facility> to bypass the role filtering that happens in
            // FacilityRepository. Same as ByFacilityId. Also this is seemingly the only way to
            // query for Facility -> EquipmentType as there's no reverse relationship. -Ross 8/6/2020
            var facilities = _container.GetInstance<RepositoryBase<Facility>>()
                                       .Where(x => facilityIds.Contains(x.Id));
            var equipment = facilities.SelectMany(x => x.Equipment)
                                      .Where(x => x.EquipmentType != null);
            var equipmentTypes = equipment.Select(x => new EquipmentType {Id = x.EquipmentType.Id, Description = x.EquipmentType.Description}).Distinct();
            result.Data = equipmentTypes;
            return result;
        }

        [HttpGet]
        public ActionResult ByEquipmentGroupId(int[] equipmentGroupId)
        {
            return new CascadingActionResult<EquipmentType, EquipmentTypeDisplayItem>(
                Repository.Where(t => t.EquipmentGroup != null && equipmentGroupId.Contains(t.EquipmentGroup.Id)),
                "Description",
                "Id");
        }

        #endregion

        #region Edit/Update

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit()
        {
            // This action exists only so the _ShowCharacteristicsField view
            // can do a CurrentUserCanEdit check to display the _CreateCharacteristicField view.
            // We should probably look into another way of doing that someday.
            throw new NotImplementedException();
        }
        
        #endregion

        #region Child Elements

        [HttpPost, RequiresAdmin]
        public ActionResult RemoveMeasurementPoint(RemoveMeasurementPointEquipmentType model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToReferrerOr("Show", "EquipmentType", new { model.Id }, "#MeasurementPointsTab"),
                OnError = () => RedirectToReferrerOr("Show", "EquipmentType", new { model.Id }, "#MeasurementPointsTab")
            });
        }
        
        #endregion
        
        #region Constructors

        public EquipmentTypeController(ControllerBaseWithPersistenceArguments<IRepository<EquipmentType>, EquipmentType, User> args) : base(args) {}

        #endregion
    }
}
