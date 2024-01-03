using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class BondViewModel : ViewModel<Bond>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }
        [EntityMap, EntityMustExist(typeof(County))]
        [DropDown("", "County", "ByStateId", DependsOn = "State", PromptText = "Select a state above")]
        public int? County { get; set; }
        [EntityMap, EntityMustExist(typeof(Town))]
        [DropDown("", "Town", "ByCountyId", DependsOn = "County", PromptText = "Select a county above")]
        public int? Town { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(BondType))]
        public int? BondType { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(BondPurpose))]
        public int? BondPurpose { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        [StringLength(Bond.StringLengths.BOND_NUMBER)]
        public string BondNumber { get; set; }
        [StringLength(Bond.StringLengths.PRINCIPAL)]
        public string Principal { get; set; }
        [StringLength(Bond.StringLengths.OBLIGEE)]
        public string Obligee { get; set; }
        public bool RecurringBond { get; set; }
        [StringLength(Bond.StringLengths.BONDING_AGENCY)]
        public string BondingAgency { get; set; }
        public decimal? BondValue { get; set; }
        public decimal? AnnualPremium { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //[Required]
        //public int? PermitsBondId { get; set; }
        public bool? BondOpen { get; set; }
        public int PermitsBondId { get; set; }

        #endregion

        #region Constructors

        public BondViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateBond : BondViewModel
    {
        #region Constructors

		public CreateBond(IContainer container) : base(container) {}

        #endregion
	}

    public class EditBond : BondViewModel
    {
        #region Constructors

		public EditBond(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchBond : SearchSet<Bond>
    {
        #region Properties

        public string BondNumber { get; set; }
        public int? Id { get; set; }
        [DropDown]
        public int? OperatingCenter { get; set; }
        [DropDown]
        public int? State { get; set; }
        [DropDown("", "County", "ByStateId", DependsOn = "State", PromptText = "Select a state above")]
        public int? County{ get; set; }
        [DropDown("", "Town", "ByCountyId", DependsOn = "County", PromptText = "Select a county above")]
        public int? Town { get; set; }

        [DisplayName("Is Open")]
        public bool? BondOpen { get; set; }

        #endregion
    }
}