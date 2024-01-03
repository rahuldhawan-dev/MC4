using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class RegulationViewModel : ViewModel<Regulation>
    {
        #region Properties

        public virtual DateTime? CreatedAt { get; set; }
        [DropDown, Required, EntityMap]
        public virtual int? Status { get; set; }
        [DropDown, Required, EntityMap]
        public virtual int? Agency { get; set; }
        [DisplayName("Regulation")]
        public virtual string RegulationShort { get; set; } 
        public virtual string Title { get; set; } 
        public virtual string Statute { get; set; } 
        public virtual string Citation { get; set; } 
        public virtual DateTime? EffectiveDate { get; set; } 
        public virtual string Purpose { get; set; } 
        public virtual string GeneralDescription { get; set; } 
        public virtual string Requirements { get; set; } 
        public virtual string UtilitiesCovered { get; set; } 
        public virtual string CostImpact { get; set; } 
        public virtual string Notes { get; set; } 
        public virtual bool AllAreas { get; set; } 
        public virtual bool FieldOperations { get; set; } 
        public virtual bool Production { get; set; } 
        public virtual bool Environmental { get; set; } 
        public virtual bool WaterQuality { get; set; } 
        public virtual bool Engineering { get; set; }

        #endregion

        #region Constructors

        public RegulationViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateRegulation : RegulationViewModel
    {
        #region Constructors

        public CreateRegulation(IContainer container) : base(container) {}

        #endregion
    }

    public class EditRegulation : RegulationViewModel
    {
        #region Constructors

        public EditRegulation(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchRegulation : SearchSet<Regulation>
    {
        [DisplayName("Regulation")]
        public string RegulationShort { get; set; }
        [DropDown]
        public int? Status { get; set; }
        [DropDown]
        public int? Agency { get; set; }
    }
}