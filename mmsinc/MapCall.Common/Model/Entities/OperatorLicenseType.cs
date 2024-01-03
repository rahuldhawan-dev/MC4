using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Utilities.Excel;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OperatorLicenseType : ReadOnlyEntityLookup
    {
        #region Indices

        public struct Indices
        {
            public const int WATER_DISTRIBUTION_OPERATOR = 1,
                             WATER_TREATMENT_OPERATOR = 2,
                             SEWER_COLLECTION_OPERATOR = 3,
                             SEWER_TREATMENT_OPERATOR = 4,
                             WASTE_TREATMENT_OPERATOR = 5;
        }

        #endregion

        public static int[] _operatingLincenseTypeIndices = {
            Indices.WATER_DISTRIBUTION_OPERATOR,
            Indices.WATER_TREATMENT_OPERATOR,
            Indices.SEWER_COLLECTION_OPERATOR,
            Indices.SEWER_TREATMENT_OPERATOR,
            Indices.WASTE_TREATMENT_OPERATOR
        };
    }
}
