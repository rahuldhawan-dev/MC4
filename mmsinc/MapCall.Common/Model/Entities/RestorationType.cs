using System;
using System.Collections.Generic;
using MMSINC.Data;
using System.Linq;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.Common.Model.Entities
{
    public enum RestorationMeasurementTypes
    {
        SquareFt,
        LinearFt
    }

    [Serializable]
    public class RestorationType : EntityLookup
    {
        #region Properties

        /// <summary>
        /// Gets/sets how  many days this type of restoration has until it must be completed for a partial/base restoration.
        /// </summary>
        public virtual int PartialRestorationDaysToComplete { get; set; }

        /// <summary>
        /// Gets/sets how  many days this type of restoration has until it must be completed for a final restoration.
        /// </summary>
        public virtual int FinalRestorationDaysToComplete { get; set; }

        public virtual IList<RestorationTypeCost> RestorationTypeCosts { get; set; }

        public virtual RestorationMeasurementTypes MeasurementType =>
            // Contractors version uses ToUpper().Contains() but it's not necessary. The two
            // types that CURB matches for both start with CURB.
            Description.StartsWith("CURB", StringComparison.InvariantCultureIgnoreCase)
                ? RestorationMeasurementTypes.LinearFt
                : RestorationMeasurementTypes.SquareFt;

        #endregion

        #region Constructor

        public RestorationType()
        {
            RestorationTypeCosts = new List<RestorationTypeCost>();
        }

        #endregion

        #region Public Methods

        public virtual decimal GetCostMultiplierForOperatingCenter(OperatingCenter opc)
        {
            opc.ThrowIfNull("opc");
            var markiplier = RestorationTypeCosts.SingleOrDefault(x => x.OperatingCenter == opc);
            return markiplier != null ? (decimal)markiplier.Cost : decimal.Zero;
        }

        #endregion
    }
}
