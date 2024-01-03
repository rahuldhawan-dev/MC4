using System;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// Represents each Application by their ApplicationID in the database.
    /// </summary>
    public enum RoleApplications
    {
        FieldServices = 1,
        Production = 2,
        HumanResources = 3,
        Operations = 4,
        // There's no 5
        BPU = 6,
        BusinessPerformance = 7,
        Management = 8,
        Customer = 9,
        FleetManagement = 10,
        Events = 11,

        // There's no 12
        Contractors = 13,
        WaterQuality = 14,

        // There's no 15
        H2O = 16,

        // There's no 17
        Environmental = 18,
        General = 19,

        // General will be 19
        BAPPTeamSharing = 20,
        ServiceLineProtection = 21,
        Engineering = 23
    }

    [Serializable]
    public class Application : IEntity
    {
        #region Properties

        [Obsolete("Use Id instead.")]
        public virtual int ApplicationID => Id;

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public virtual RoleApplications Value => (RoleApplications)Id;

        #endregion

        #region Exposed Methods

        public override string ToString() => Name;

        #endregion
    }
}
