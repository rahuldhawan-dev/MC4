using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models
{
    public class SearchPublicWaterSupply : SearchSet<PublicWaterSupply>
    {
        public static int[] PublicWaterSupplyStatusWaterlyLookup = {
            PublicWaterSupplyStatus.Indices.ACTIVE,
            PublicWaterSupplyStatus.Indices.PENDING,
            PublicWaterSupplyStatus.Indices.PENDING_MERGER
        };

        public static int?[] PublicWaterSupplyOwnershipWaterlyLookup = {
            PublicWaterSupplyOwnership.Indices.AW_CONTRACT,
            PublicWaterSupplyOwnership.Indices.AW_OWNED,
        };

        public int? State { get; set; }
        public int?[] Ownership => PublicWaterSupplyOwnershipWaterlyLookup;
        public int[] Status => PublicWaterSupplyStatusWaterlyLookup;
    }
}