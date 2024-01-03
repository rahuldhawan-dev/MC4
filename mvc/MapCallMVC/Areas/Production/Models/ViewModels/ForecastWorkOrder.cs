using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class ForecastWorkOrder
    {
        #region Properties

        public string PlannedDate { get; set; }
        public string LocalTaskDescription { get; set; }
        public string Resources { get; set; }
        public string EstimatedHours { get; set; }
        public string SkillSet { get; set; }

        public IEnumerable<ScheduledAssignment> Assignments { get; set; } = new List<ScheduledAssignment>();

        #endregion
    }
}