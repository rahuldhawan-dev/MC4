using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class TailgateTalkController : ControllerBaseWithPersistence<ITailgateTalkRepository, TailgateTalk, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsHealthAndSafety;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(dataGetter: this.GetUserOperatingCentersFn(ROLE));
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("PresentedBy", this.GetUserOperatingCenterAccessibleEmployeesFn(ROLE));
                    this.AddDropDownData<TailgateTopicCategory>();
                    break; 
                case ControllerAction.New:
                    this.AddDynamicDropDownData<TailgateTalkTopic, TailgateTalkTopicDisplayItem>("Topic", filter: t => t.IsActive);
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("PresentedBy", this.GetUserOperatingCenterAccessibleEmployeesFn(ROLE), filter: e => e.Status.Id == EmployeeStatus.Indices.ACTIVE);
                    break;
                case ControllerAction.Edit:
                    this.AddDynamicDropDownData<TailgateTalkTopic, TailgateTalkTopicDisplayItem>("Topic", filter: t => t.IsActive);
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("PresentedBy", this.GetUserOperatingCenterAccessibleEmployeesFn(ROLE));
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchTailgateTalk search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchTailgateTalk search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));

                // What's using this? -Ross 12/17/2019
                formatter.Json(() => {
                    if (search.HeldOn == null || search.HeldOn.Operator != RangeOperator.Between ||
                        search.HeldOn.End == null || search.HeldOn.End.Value
                            .Subtract(search.HeldOn.Start.Value).TotalDays > 30)
                    {
                        throw new InvalidOperationException(
                            "HeldOn must be a 'between' search of a month or less.");
                    }
                    search.EnablePaging = false;
                    var results = Repository.Search(search);
                    return Json(new {
                        Data = results.Select(t => new {
                            t.Id,
                            OperatingCenter = t.OperatingCenter?.ToString(),
                            t.HeldOn,
                            t.TrainingTimeHours,
                            Category = t.Category?.ToString(),
                            Topic = t.Topic?.ToString(),
                            PresentedBy = t.PresentedBy?.ToString(),
                            t.OrmReferenceNumber
                        })
                    }, JsonRequestBehavior.AllowGet);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateTailgateTalk(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateTailgateTalk model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    // Bug 2609: Add presenter as having attended the tailgate talk they presented.
                    //           This can not be done by the model since the entity id wouldn't be created yet.
                    var newLink = new EmployeeLink {
                        LinkedId = model.Id,
                        DataType = _container.GetInstance<IDataTypeRepository>().GetByTableNameAndDataTypeName(TailgateTalkMap.TABLE_NAME, TailgateTalk.DATA_TYPE_NAME).Single(),
                        LinkedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate(),
                        LinkedBy = AuthenticationService.CurrentUser.FullName ?? AuthenticationService.CurrentUser.UniqueName,
                        Employee = _container.GetInstance<IEmployeeRepository>().Find(model.PresentedBy.Value)
                    };

                    _container.GetInstance<IRepository<EmployeeLink>>().Save(newLink);

                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditTailgateTalk>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit),]
        public ActionResult Update(EditTailgateTalk model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public TailgateTalkController(ControllerBaseWithPersistenceArguments<ITailgateTalkRepository, TailgateTalk, User> args) : base(args) {}
    }
}