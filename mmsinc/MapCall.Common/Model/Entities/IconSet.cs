using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class IconSet : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual MapIcon DefaultIcon { get; set; }
        public virtual IList<MapIcon> Icons { get; set; }

        public IconSet()
        {
            Icons = new List<MapIcon>();
        }
    }

    /// <summary>
    /// Aside from "All", these values have to match with the database apparently.
    /// </summary>
    [Serializable]
    public enum IconSets
    {
        All = 0,
        Antennae = 1,
        Assets = 2,
        Hourglasses = 3,
        Miscellaneous = 4,
        Pins = 5,
        Shovels = 6,
        Incidents = 7,
        GeneralLiabilityClaims = 8,
        LockoutForms = 9,
        MainCrossings = 10,

        /// <summary>
        /// Only has one icon. Use this for coordinate pickers that don't require or allow
        /// a user to pick an icon(ie hydrants and valves).
        /// </summary>
        SingleDefaultIcon = 11,
        WorkOrders = 12,
        Beakers = 13,
        NearMiss = 14
    }
}
