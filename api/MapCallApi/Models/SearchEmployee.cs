using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models
{
    public class SearchEmployee : SearchSet<Employee>
    {
        [Required]
        public int? ReportingFacility { get; set; }

        [Required]
        [SearchAlias("ReportingFacility", "P", "Id")]
        public int? PublicWaterSupply { get; set; }

        [Required]
        [SearchAlias("OperatingCenter", "S", "State.Id")]
        public int? State { get; set; }

        [Required]
        public int? Status => EmployeeStatus.Indices.ACTIVE;
    }
}