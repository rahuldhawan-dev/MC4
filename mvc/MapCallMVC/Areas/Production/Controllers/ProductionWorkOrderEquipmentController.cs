using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class ProductionWorkOrderEquipmentController : ControllerBaseWithPersistence<
        IRepository<ProductionWorkOrderEquipment>, ProductionWorkOrderEquipment, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;
        public const string FRAGMENT_IDENTIFIER = "#EquipmentTab",
                            AS_LEFT_CONDITION_NEEDS_RE_INSPECTION_SOONER = "As Left Condition Needs ReInspection Sooner";

        #endregion

        #region Constructors

        #endregion

        #region Constructor

        public ProductionWorkOrderEquipmentController(
            ControllerBaseWithPersistenceArguments<IRepository<ProductionWorkOrderEquipment>,
                ProductionWorkOrderEquipment, User> args) : base(args) { }

        #endregion

        #region Exposed Methods

        [HttpPost, RequiresSecureForm, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(ProductionWorkOrderEquipmentViewModel model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (model.AsLeftCondition == null)
                    {
                        return RedirectToReferrerOr("Show", "ProductionWorkOrder", new { id = model.Id },
                            "#EquipmentTab");
                    }

                    if (AsLeftCondition.AUTO_CREATE_PRODUCTION_WORK_ORDER_STATUSES
                                       .Contains(model.AsLeftCondition.Value))
                    {
                        return RedirectToAction("CreateProductionWorkOrderForEquipment", "ProductionWorkOrder", model);
                    }
                    else
                    {
                        if (model.AsLeftCondition.Value ==
                            AsLeftCondition.Indices.NEEDS_RE_INSPECTION_SOONER_THAN_NORMAL)
                        {
                            SendSupervisorNeedsReInspectionSoonerThanNormalNotification(model);
                        }

                        return RedirectToReferrerOr("Show", "ProductionWorkOrder", new { id = model.Id },
                            "#EquipmentTab");
                    }
                }
            });
        }

        private void SendSupervisorNeedsReInspectionSoonerThanNormalNotification(ProductionWorkOrderEquipmentViewModel model)
        {
            var orderRepo = _container.GetInstance<IProductionWorkOrderRepository>();
            var equipmentRepo = _container.GetInstance<IEquipmentRepository>();
            var notifier = _container.GetInstance<INotificationService>();

            var pwo = orderRepo.Find(model.ProductionWorkOrderId);
            var equipment = equipmentRepo.Find(model.Equipment);

            if (pwo == null || equipment == null)
            {
                return;
            }

            var newModel = new ProductionWorkOrderEquipmentNotification {
                RoutineWorkOrder = pwo,
                ProductionWorkOrderId = pwo.Id,
                ProductionWorkDescription = pwo.ProductionWorkDescription.ToString(),
                CorrectiveOrderProblemCode = pwo.CorrectiveOrderProblemCode,
                FacilityName = pwo.Facility.FacilityName,
                FacilityUrl = GetUrlForModel(pwo.Facility, "Show", "Facility"),
                EquipmentUrl = GetUrlForModel(equipment, "Show", "Equipment"),
                ProductionWorkOrderUrl = GetUrlForModel(pwo, "Show", "ProductionWorkOrder", "Production"),
                Equipment = equipment,
                AsLeftConditionComment = model.AsLeftConditionComment
            };

            var employees = pwo.EmployeeAssignments.Select(x => x.AssignedTo)
                               .Where(y => y.ReportsTo != null && !string.IsNullOrEmpty(y.ReportsTo.EmailAddress));

            foreach (var employee in employees)
            {
                notifier.Notify(new NotifierArgs {
                    OperatingCenterId = pwo.OperatingCenter.Id,
                    Module = ROLE,
                    Purpose = AS_LEFT_CONDITION_NEEDS_RE_INSPECTION_SOONER,
                    Data = newModel,
                    Subject =
                        $"Re-Inspection Needed for {equipment.Description} at {pwo.Facility.FacilityName} identified on {String.Format(CommonStringFormats.DATE, pwo.DateReceived)}",
                    Address = employee.ReportsTo.EmailAddress
                });
            }
        }

        #endregion
    }
}
