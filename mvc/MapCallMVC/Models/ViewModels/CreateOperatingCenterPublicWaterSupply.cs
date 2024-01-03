using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    [Serializable]
    public class CreateOperatingCenterPublicWaterSupply : ViewModel<OperatingCenterPublicWaterSupply>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int OperatingCenter { get; set; }
        [EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public int PublicWaterSupply { get; set; }

        public CreateOperatingCenterPublicWaterSupply(IContainer container) : base(container) { }
    }
}