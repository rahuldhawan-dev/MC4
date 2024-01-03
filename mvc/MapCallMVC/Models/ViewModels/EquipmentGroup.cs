using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JetBrains.Annotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class SearchEquipmentGroup : SearchSet<EquipmentGroup>
    {
        #region Properties
        
        public string Code { get; set; }
        
        public string Description { get; set; }

        public string Definition { get; set; }

        #endregion
    }
}