using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    //VIOLATING DRY - BUSINESS WILL INEVIATABLY CHANGE THESE
    [Serializable]
    public class EquipmentCondition : EntityLookup
    {
        public struct Indices
        {
            public const int POOR = 1, AVERAGE = 2, GOOD = 3;
        }
    }

    [Serializable]
    public class EquipmentPerformanceRating : EntityLookup
    {
        public struct Indices
        {
            public const int POOR = 1, AVERAGE = 2, GOOD = 3;
        }
    }

    [Serializable]
    public class EquipmentStaticDynamicType : EntityLookup
    {
        public struct Indices
        {
            public const int STATIC = 1, DYNAMIC = 2;
        }
    }

    [Serializable]
    public class EquipmentConsequencesOfFailureRating : EntityLookup
    {
        public struct Indices
        {
            public const int LOW = 1, MEDIUM = 2, HIGH = 3;
        }
    }

    [Serializable]
    public class EquipmentLikelyhoodOfFailureRating : EntityLookup
    {
        public struct Indices
        {
            public const int LOW = 1, MEDIUM = 2, HIGH = 3;
        }
    }

    [Serializable]
    public class EquipmentReliabilityRating : EntityLookup
    {
        public struct Indices
        {
            public const int LOW = 1, MEDIUM = 2, HIGH = 3;
        }
    }

    [Serializable]
    public class EquipmentFailureRiskRating : EntityLookup
    {
        public struct Indices
        {
            public const int LOW = 1, MEDIUM = 2, HIGH = 3;
        }
    }

    [Serializable]
    public class StrategyTier : EntityLookup
    {
        public struct Indices
        {
            public const int TIER_1 = 1, TIER_2 = 2, TIER_3 = 3;
        }
    }
}
