using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MapCall.Common.Helpers;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;

namespace MapCallMVC.ClassExtensions
{
    public static class ActionBarHelperExtensionMethods
    {
        public static void AddWorkOrderLinkForConfinedSpaceForm(this ActionBarHelper helper,
            PostCompletionConfinedSpaceFormBase model)
        {
            if (model.ProductionWorkOrderDisplay != null)
            {
                helper.AddLink("Order", "ab-show", "Show", "ProductionWorkOrder", new { area = "Production", id = model.ProductionWorkOrderDisplay.Id }, null);
            }
            else if (model.WorkOrder != null)
            {
                helper.AddLink("Order", "ab-show", "Show", "WorkOrder", new { area = "FieldOperations", id = model.WorkOrderDisplay.Id }, null);
            }
        }
    }
}