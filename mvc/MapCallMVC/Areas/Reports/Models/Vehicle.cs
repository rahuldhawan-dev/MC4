using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchVehicleUtilizationReport : SearchSet<VehicleUtilizationReportItem>, ISearchVehicleUtilizationReport
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleDepartment))]
        public int? Department { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleStatus))]
        public int? Status { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleAssignmentStatus))]
        public int? AssignmentStatus { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleAssignmentCategory))]
        public int? AssignmentCategory { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleAssignmentJustification))]
        public int? AssignmentJustification { get; set; }
        public bool? PoolUse { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleAccountingRequirement))]
        public int? AccountingRequirement { get; set; }
        public bool? Flag { get; set; }

        #endregion

    }
}