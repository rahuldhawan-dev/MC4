using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityInspectionAreaType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int ENTIRE_FACILITY = 1, 
                             GROUNDS = 2, 
                             LAB = 3, 
                             GARAGE = 4, 
                             ADMIN_AREA = 5, 
                             SHOP_AREA = 6, 
                             CHEMICAL_FEED_STORAGE_AREA = 7, 
                             TREATMENT_AREA = 8, 
                             OTHER = 9;
        }
    }
}
