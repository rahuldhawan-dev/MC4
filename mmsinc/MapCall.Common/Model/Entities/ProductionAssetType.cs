using System;
using MMSINC.Data;
using NHibernate.Criterion;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionAssetType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int HYDRANT = 1,
                             MECHANICAL = 2,
                             ELECTRICAL = 3,
                             TANKS = 4,
                             SEWER = 5,
                             VALVE = 6,
                             OPENING = 7,
                             VEHICLE = 8;
        }

        public struct Descriptions
        {
            public const string HYDRANT = "Hydrant",
                                MECHANICAL = "Mechanical",
                                ELECTRICAL = "Electrical",
                                TANKS = "Tanks",
                                SEWER = "Sewer",
                                VALVE = "Valve",
                                OPENING = "Opening",
                                VEHICLE = "Vehicle";
        }

        public struct DescriptionsUppercase
        {
            public const string HYDRANT = "HYDRANT",
                                MECHANICAL = "MECHANICAL",
                                ELECTRICAL = "ELECTRICAL",
                                TANKS = "TANKS",
                                SEWER = "SEWER",
                                VALVE = "VALVE",
                                OPENING = "OPENING",
                                VEHICLE = "VEHICLE";
        }

        public struct TableName
        {
            public const string TABLE_NAME = "ProductionAssetTypes";
        }
    }
}