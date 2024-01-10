using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    [Serializable]
    public class CreatePublicWaterSupplyLicensedOperator : ViewModel<PublicWaterSupplyLicensedOperator>
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public int? PublicWaterSupply { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatorLicense))]
        public int? LicensedOperator { get; set; }


        #endregion

        #region Constructors

        public CreatePublicWaterSupplyLicensedOperator(IContainer container) : base(container) { }

        #endregion
    }

}
