using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models
{
    public class SearchEquipment : SearchSet<Equipment>
    {
        [Required]
        [SearchAlias("criteriaOperatingCenter.State", "S", "Id")]
        public int? State { get; set; }
        [Required]
        public int? Facility { get; set; }
        [Required]
        [SearchAlias("criteriaFacility.PublicWaterSupply", "P", "Id")]
        public int? PublicWaterSupply { get; set; }
    }
}