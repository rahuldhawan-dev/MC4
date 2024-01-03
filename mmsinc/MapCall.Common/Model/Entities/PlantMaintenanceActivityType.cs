using System;
using System.Linq.Expressions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PlantMaintenanceActivityType : EntityLookup
    {
        #region Private Members

        private PlantMaintenanceActivityTypeDisplay _display;

        #endregion

        #region Consts

        public new struct StringLengths
        {
            public const int CODE = 3;
        }

        public struct Indices
        {
            public const int
                DVA = 9,
                PBC = 18,
                RBS = 19,
                RPS = 32,
                RPT = 33,
                BRG = 5;
        }

        #endregion

        #region Properties

        public virtual string Code { get; set; }
        public virtual OrderType OrderType { get; set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return (_display ?? (_display = new PlantMaintenanceActivityTypeDisplay {
                Code = Code,
                Description = Description
            })).Display;
        }

        // This array coincides with the IsOverrideCode database column
        // if anything is added or removed from this Array
        // the table/record will need to be updated to reflect that
        public static int[] GetOverrideCodes()
        {
            return new[] {Indices.DVA, Indices.PBC, Indices.RBS, Indices.RPS, Indices.RPT, Indices.BRG};
        }

        // This array coincides with the RequiresWBSNumber database column
        // if anything is added or removed from this Array
        // the table/record will need to be updated to reflect that
        public static int[] GetOverrideCodesRequiringWBSNumber()
        {
            return new[] {Indices.DVA, Indices.RBS, Indices.RPS, Indices.RPT, Indices.BRG};
        }

        #endregion
    }

    [Serializable]
    public class PlantMaintenanceActivityTypeDisplay : IEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public string Display => $"{Code} - {Description}";
    }
}
