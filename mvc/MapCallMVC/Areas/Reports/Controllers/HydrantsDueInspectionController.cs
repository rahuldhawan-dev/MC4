﻿using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class HydrantsDueInspectionController : ControllerBaseWithPersistence<IHydrantRepository, Hydrant, User>
    {
        #region Constructor

        public HydrantsDueInspectionController(ControllerBaseWithPersistenceArguments<IHydrantRepository, Hydrant, User> args) : base(args) {}

        #endregion

        #region Public methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
        }

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchHydrantsDueInspectionReport>();
        }

        [HttpGet]
        public ActionResult Index(SearchHydrantsDueInspectionReport model)
        {
            // No paging for this for regular views or excel.
            model.EnablePaging = false;
            var args = new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetHydrantsDueInspection(model)
            };
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(model, args));
                f.Excel(() => ActionHelper.DoExcel(model, args));
            });
        }

        #endregion
    }
}