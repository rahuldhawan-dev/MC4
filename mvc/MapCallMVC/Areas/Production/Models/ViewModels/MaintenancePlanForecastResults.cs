using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class MaintenancePlanForecastResults
    {
        #region Properties

        public int MaintenancePlan { get; set; }
        public int OperatingCenter { get; set; }

        public IEnumerable<ForecastWorkOrder> ForecastWorkOrders { get; set; } = new List<ForecastWorkOrder>();

        #endregion
    }
}
