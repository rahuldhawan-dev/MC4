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
    public class WasteWaterContaminantCategory : EntityLookup { }
}
