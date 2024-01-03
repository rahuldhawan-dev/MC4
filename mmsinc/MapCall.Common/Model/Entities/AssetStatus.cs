using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AssetStatus : EntityLookup
    {
        public struct Indices
        {
            // NOTE: If you add or change a value to this then you MUST
            // add a cooresponding TestDataFactory.
            public const int
                ACTIVE = 1,
                CANCELLED = 2,
                PENDING = 3,
                REMOVED = 5,
                RETIRED = 6,
                INACTIVE = 11,
                INSTALLED = 12,
                REQUEST_CANCELLATION = 13,
                REQUEST_RETIREMENT = 14,
                NSI_PENDING = 15;
        }

        public const string ACTIVE = "ACTIVE",
                            RETIRED = "RETIRED",
                            PENDING = "PENDING",
                            INACTIVE = "INACTIVE";

        #region Properties

        /// <summary>
        /// Indicates that a user must have the UserAdmin role in order to 
        /// select this status while editing a hydrant.
        /// </summary>
        public virtual bool IsUserAdminOnly { get; set; }

        // MC-1725: "REQUEST CANCELLATION" and "REQUEST RETIREMENT" are not considered "active" for copying.
        public static int[] CanBeCopiedStatuses => new[] {Indices.ACTIVE, Indices.PENDING, Indices.INSTALLED};

        public static readonly int[] ALL_STATUS_IDS =
            new[] {
                Indices.ACTIVE,
                Indices.CANCELLED,
                Indices.PENDING,
                Indices.REMOVED,
                Indices.RETIRED,
                Indices.INACTIVE,
                Indices.INSTALLED,
                Indices.REQUEST_CANCELLATION,
                Indices.REQUEST_RETIREMENT
            };

        /// <summary>
        /// MC-1725: These are the statuses that are considered ACTIVE for inspections and BPU purposes.
        /// This is different from what is considered active for asset number duplication.
        /// </summary>
        public static readonly IEnumerable<int> ACTIVE_STATUSES = new[]
            {Indices.ACTIVE, Indices.REQUEST_CANCELLATION, Indices.REQUEST_RETIREMENT};

        /// <summary>
        /// MC-2607 - A set of status codes that are to be considered RETIRED
        /// </summary>
        public static readonly int[] RETIRED_STATUS_IDS = new[] {Indices.RETIRED, Indices.REMOVED};

        /// <summary>
        /// MC-2857: These are the statuses that are considered when collecting WBS number
        /// </summary>
        public static readonly IEnumerable<int> WBS_STATUS_IDS = new[] { Indices.RETIRED, Indices.REMOVED, Indices.ACTIVE };

        /// <summary>
        /// MC-2471: These are the statuses that are considered INACTIVE for creating
        /// Inspections and Main Cleanings for a sewer opening.
        /// </summary>
        public static readonly IEnumerable<int> INACTIVE_STATUSES = new[]
            {Indices.CANCELLED, Indices.INACTIVE, Indices.REMOVED, Indices.RETIRED};

        /// <summary>
        /// MC-4922: These are the statuses that are considered for not creating a
        /// work order from a hydrant, valve and sewer opening.
        /// </summary>
        public static readonly IEnumerable<int> WORK_ORDER_DISABLED_STATUSES = new[]
            { Indices.CANCELLED, Indices.INACTIVE, Indices.REMOVED, Indices.RETIRED };

        #endregion

        #region Methods

        /// <summary>
        ///     Gets an array of status codes that are to be considered RETIRED
        /// </summary>
        /// <remarks>
        ///     MC-2607 added the rule that the "REMOVED" status should be treated like "RETIRED". 
        /// </remarks>
        /// <returns>
        ///   The set of retired status codes.
        /// </returns>
        public static int[] GetRetiredStatusIds() => AssetStatus.RETIRED_STATUS_IDS;

        #endregion
    }
}
