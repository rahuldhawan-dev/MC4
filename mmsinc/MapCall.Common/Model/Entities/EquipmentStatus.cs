using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentStatus : EntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int IN_SERVICE = 1,
                             OUT_OF_SERVICE = 2,
                             PENDING = 3,
                             RETIRED = 4,
                             PENDING_RETIREMENT = 5,
                             CANCELLED = 6,
                             FIELD_INSTALLED = 7;
        }

        public static string IN_SERVICE = "In Service",
                             OUT_OF_SERVICE = "Out of Service",
                             PENDING = "Pending",
                             RETIRED = "Retired",
                             PENDING_RETIREMENT = "Pending Retirement",
                             CANCELLED = "Cancelled",
                             FIELD_INSTALLED = "Field Installed";

        public static int[] CanBeCopiedStatuses => new[] {Indices.IN_SERVICE, Indices.PENDING, Indices.FIELD_INSTALLED};

        #endregion

        #region Properties

        public virtual IList<Equipment> Equipment { get; set; }

        #endregion

        #region Constructors

        public EquipmentStatus()
        {
            Equipment = new List<Equipment>();
        }

        #endregion
    }
}
