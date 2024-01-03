using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchCrewAssignmentSummary : SearchSet<CrewAssignment>
    {
        //[DropDown, Required, EntityMap, EntityMustExist(typeof(State))]
        //public int? State { get; set; }

        [DropDown, Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("wo.OperatingCenter", "oc", "Id")]
        public int? OperatingCenter { get; set; }

        [Required, View("Assignment Date")]
        public RequiredDateRange AssignedFor { get; set; }

        public DateRange DateStarted { get; set; }

        public DateRange DateEnded { get; set; }

        [Search(CanMap = false), View("Active Open Crew Assignments")]
        public bool? IsOpen { get; set; }

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            if (IsOpen.HasValue)
            {
                mapper.MappedProperties[nameof(DateStarted)].Value = SearchMapperSpecialValues.IsNotNull;

                if (IsOpen.Value)
                {
                    mapper.MappedProperties[nameof(DateEnded)].Value = SearchMapperSpecialValues.IsNull;
                }
                else
                {
                    mapper.MappedProperties[nameof(DateEnded)].Value = SearchMapperSpecialValues.IsNotNull;
                }
            }
        }
    }
}