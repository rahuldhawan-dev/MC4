using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OverrideInfoMasterReason : EntityLookup
    {
        public struct Indices
        {
            public const int MORATORIUM = 1, LOCAL_POLITICS = 2, GIS_DATA_INCORRECT = 3;
        }
    }
}
