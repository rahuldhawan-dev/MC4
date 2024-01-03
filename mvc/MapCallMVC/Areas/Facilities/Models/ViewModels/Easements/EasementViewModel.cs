using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels.Easements
{
    public class EasementViewModel : ViewModel<Easement>
    {
        #region Constructors

        public EasementViewModel(IContainer container) : base(container) { } 

        #endregion

        #region Properties

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above"),
         EntityMustExist(typeof(Town)), EntityMap, Required]
        public virtual int? Town { get; set; }

        [DropDown("", "TownSection", "ActiveByTownId", DependsOn = "Town", PromptText = "Select a town from above"),
         EntityMustExist(typeof(TownSection)), EntityMap]
        public virtual int? TownSection { get; set; }

        public virtual string StreetNumber { get; set; }

        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Select a town above"),
         EntityMustExist(typeof(Street)), EntityMap]
        public virtual int? Street { get; set; }

        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Select a town above"),
         EntityMustExist(typeof(Street)), EntityMap]
        public virtual int? CrossStreet { get; set; }

        [Required, Coordinate(AddressCallback = "Easement.getAddress", IconSet = IconSets.SingleDefaultIcon)]
        [EntityMustExist(typeof(Coordinate)), EntityMap]
        public virtual int? Coordinate { get; set; }

        [StringLength(Easement.StringLengths.RECORD_NUMBER), Required]
        public virtual string RecordNumber { get; set; }

        [DropDown, EntityMustExist(typeof(EasementStatus)), EntityMap, Required]
        public virtual int? Status { get; set; }

        [DropDown, EntityMustExist(typeof(EasementCategory)), EntityMap, Required]
        public virtual int? Category { get; set; }

        public virtual string Wbs { get; set; }

        [MinLength(5), Required]
        public virtual string EasementDescription { get; set; }

        [DropDown, EntityMustExist(typeof(EasementReason)), EntityMap]
        public virtual int? Reason { get; set; }

        [DropDown, EntityMustExist(typeof(EasementType)), EntityMap, Required]
        public virtual int? Type { get; set; }

        [Required]
        public virtual DateTime? DateRecorded { get; set; }

        public virtual string DeedBook { get; set; }

        public virtual string DeedPage { get; set; }

        public virtual string BlockLot { get; set; }

        [DropDown, EntityMustExist(typeof(GrantorType)), EntityMap, Required]
        public virtual int? GrantorType { get; set; }

        public virtual string OwnerName { get; set; }

        public virtual string OwnerAddress { get; set; }

        public virtual string OwnerPhone { get; set; }

        #endregion

    }
}