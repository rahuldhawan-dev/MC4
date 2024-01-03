using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchServiceInstallationMaterial : SearchSet<ServiceInstallationMaterial>
    {
        [DropDown, SearchAlias("OperatingCenter", "State.Id"), EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateIdForFieldServicesAssets", DependsOn = "State", DependentsRequired = DependentRequirement.None)]
        public int? OperatingCenter { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceCategory))]
        public int? ServiceCategory { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? ServiceSize { get; set; }
    }
}