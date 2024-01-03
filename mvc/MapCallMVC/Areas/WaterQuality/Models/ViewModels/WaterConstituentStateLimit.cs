using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels
{
    public class AddWaterConstituentStateLimit : ViewModel<WaterConstituentStateLimit>
    {
        #region Properties

        [StringLength(255)]
        public string Description { get; set; }
        [StringLength(255)]
        public string Agency { get; set; }

        public float? Min { get; set; }
        public float? Max { get; set; }
        public float? Mcl { get; set; }
        public float? Mclg { get; set; }
        public float? Smcl { get; set; }

        [StringLength(255)]
        public string ActionLimit { get; set; }
        public int? StateDEPAnalyteCode { get; set; }

        [EntityMap, EntityMustExist(typeof(WaterConstituent))]
        public int? WaterConstituent { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(UnitOfWaterSampleMeasure))]
        public int? UnitOfMeasure { get; set; }

        [StringLength(255)]
        public string Regulation { get; set; }

        #endregion

        #region Constructors

        public AddWaterConstituentStateLimit(IContainer container) : base(container) { }

        #endregion
    }
}