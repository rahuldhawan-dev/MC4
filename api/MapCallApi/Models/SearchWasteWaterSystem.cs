using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models
{
    public class SearchWasteWaterSystem : SearchSet<WasteWaterSystem>
    {
        [Required]
        [SearchAlias("OperatingCenter", "S", "State.Id")]
        public int? State { get; set; }
        public int[] Status => WasteWaterSystemStatus.WaterlyLookupStatuses;
        public int[] Ownership => WasteWaterSystemOwnership.WaterlyLookupStatuses;

    }
}