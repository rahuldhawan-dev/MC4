using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyCreateHydrant : CreateHydrantBase
    {
        #region Properties

        public int? SAPEquipmentId { get; set; }
        public DateTime? DateTested { get; set; }
        public int? FLRouteNumber { get; set; }
        public int? FLRouteSequence { get; set; }
        public int? ObjectID { get; set; }

        #endregion

        #region Constructors

        public MyCreateHydrant(IContainer container, AssetStatusNumberDuplicationValidator numberDuplicationValidator) : base(container, numberDuplicationValidator) {}

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> ValidateHydrantSuffixForFoundHydrants()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
