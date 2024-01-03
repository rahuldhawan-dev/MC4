using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;
using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CreateSpoilRemoval : SpoilRemovalViewModel
    {
        #region Properties

        [DropDown, Required, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public override int? State { get; set; }

        [DropDown("", "OperatingCenter", "WorkOrdersEnabledByStateId", DependsOn = "State", PromptText = "Please select a State above."), Required, DoesNotAutoMap, EntityMustExist(typeof(OperatingCenter))]
        public override int? OperatingCenter { get; set; }

        #endregion

        #region Constructors

        public CreateSpoilRemoval(IContainer container) : base(container) { }

        #endregion
    }
}