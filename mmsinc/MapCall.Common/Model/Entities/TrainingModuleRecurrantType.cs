using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TrainingModuleRecurrantType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int INITIAL = 1, RECURRING = 2, INITIAL_RECURRING = 3;
        }
    }
}
