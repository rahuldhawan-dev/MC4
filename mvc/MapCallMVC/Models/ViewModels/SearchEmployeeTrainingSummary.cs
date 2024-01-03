using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using MMSINC.Validation;


namespace MapCallMVC.Models.ViewModels
{
    public class SearchOperatingCenterTrainingSummary : SearchSet<OperatingCenterTrainingSummaryReportItem>, ISearchOperatingCenterTrainingSummary
    {
        #region Properties

        [DropDown, Required]
        public int? State { get; set; }
        
        [MultiSelect("", "OperatingCenter", "ByStateId", DependsOn = "State")]
        [Required]
        [SearchAlias("OperatingCenter", "opCntr", "Id")]
        public int[] OperatingCenter { get; set; }

        [DropDown]
        public int? TrainingRequirement { get; set; }

        public bool? IsOSHARequirement { get; set; }

        [Description("Click the help icon on the top right for more info.")]
        public DateTime? DueBy { get; set; }
        
        #endregion
    }

    public class SearchOperatingCenterTrainingOverview : SearchSet<OperatingCenterTrainingOverviewReportItem>, ISearchOperatingCenterTrainingOverview
    {
        [DropDown, Required]
        public int? State { get; set; }
        public bool? IsOSHARequirement { get; set; }
    }
}