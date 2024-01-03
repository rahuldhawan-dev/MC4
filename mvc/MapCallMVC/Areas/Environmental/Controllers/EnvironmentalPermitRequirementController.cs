using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    public class EnvironmentalPermitRequirementController : ControllerBaseWithPersistence<IRepository<EnvironmentalPermitRequirement>, EnvironmentalPermitRequirement, User>
    {
        #region Constants

        public const RoleModules ROLE = EnvironmentalPermitController.ROLE;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddDropDownData<EnvironmentalPermitRequirementType>("RequirementType");
            this.AddDropDownData<EnvironmentalPermitRequirementValueUnit>("ValueUnit");
            this.AddDropDownData<EnvironmentalPermitRequirementValueDefinition>("ValueDefinition");
            this.AddDropDownData<EnvironmentalPermitRequirementTrackingFrequency>("TrackingFrequency");
            this.AddDropDownData<EnvironmentalPermitRequirementReportingFrequency>("ReportingFrequency");
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            SetLookupData(ControllerAction.Edit);

            return ActionHelper.DoEdit<EditEnvironmentalPermitRequirement>(id, onModelFound: req => {
                var permit = req.EnvironmentalPermit;
                var employeeGetter = permit.OperatingCenters.Any()
                    ? (Func<IRepository<Employee>, IQueryable<Employee>>)
                    (r => r.GetEmployeesByOperatingCenters(permit.OperatingCenters.Select(oc => oc.Id)))
                    : r => r.Where(e => e.OperatingCenter.State.Id == permit.State.Id);

                this.AddDynamicDropDownData<Employee, EmployeeDisplayItem>("ProcessOwner", employeeGetter);
                this.AddDynamicDropDownData<Employee, EmployeeDisplayItem>("ReportingOwner", employeeGetter);
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEnvironmentalPermitRequirement model)
        {
            // TODO: There's a problem here when validation errors occur. The stuff that happens in
            // onModelFound in Edit does not happen here. This causes an error to be thrown when
            // ActionHelper tries to dynamically load dropdown data. Your first thought might be to
            // add the correct EntityMustExist attribute on the two properties, but that will cause
            // an unfiltered and, ultimately unused, Employee data to be loaded.
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "EnvironmentalPermit", new { area = "Environmental", id = model.EnvironmentalPermit })
            });
        }

        #endregion

        public EnvironmentalPermitRequirementController(ControllerBaseWithPersistenceArguments<IRepository<EnvironmentalPermitRequirement>, EnvironmentalPermitRequirement, User> args) : base(args) {}
    }
}