using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public abstract class BaseValveImageViewModel : ViewModel<ValveImage>
    {
        public struct ErrorMessages
        {
            public const string IS_DEFAULT_IMAGE = "The Is Default Image for Valve field is required.";
        }

        #region Properties

        [Required, DropDown]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required, DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [StringLength(ValveImage.StringLengths.TOWNSECTION)]
        public string TownSection { get; set; }

        [StringLength(ValveImage.StringLengths.STREET_NUMBER)]
        public string StreetNumber { get; set; }

        [StringLength(ValveImage.StringLengths.APARTMENT_NUMBER)]
        public string ApartmentNumber { get; set; }

        [StringLength(ValveImage.StringLengths.STREET_PREFIX)]
        public string StreetPrefix { get; set; }
        
        [DisplayName("Street Name")]
        [StringLength(ValveImage.StringLengths.STREET)]
        public string Street { get; set; }

        [StringLength(ValveImage.StringLengths.STREET_SUFFIX)]
        public string StreetSuffix { get; set; }

        [StringLength(ValveImage.StringLengths.VALVE_NUMBER)]
        public string ValveNumber { get; set; }

        [DisplayName("Valve Asset")]
        [DropDown("FieldOperations", "Valve", "ByStreetId", DependsOn = "StreetIdentifyingInteger", PromptText = "Please select a street above")]
        [EntityMap, EntityMustExist(typeof(Valve))]
        public int? Valve { get; set; }

        [StringLength(ValveImage.StringLengths.CROSS_STREET_PREFIX)]
        public string CrossStreetPrefix { get; set; }

        [Required]
        [StringLength(ValveImage.StringLengths.CROSS_STREET)]
        public string CrossStreet { get; set; }

        [StringLength(ValveImage.StringLengths.CROSS_STREET_SUFFIX)]
        public string CrossStreetSuffix { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [StringLength(ValveImage.StringLengths.LOCATION)]
        public string Location { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ValveNormalPosition))]
        public int? NormalPosition { get; set; }

        [StringLength(ValveImage.StringLengths.NUMBER_OF_TURNS)]
        public string NumberOfTurns { get; set; }

        [StringLength(ValveImage.StringLengths.DATE_COMPLETED)]
        public string DateCompleted { get; set; }

        [Required]
        [StringLength(ValveImage.StringLengths.VALVE_SIZE)]
        public string ValveSize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ValveOpenDirection))]
        public int? OpenDirection { get; set; }

        [Required(ErrorMessage = ErrorMessages.IS_DEFAULT_IMAGE)]
        public bool? IsDefaultImageForValve { get; set; }

        #endregion

        #region Constructor

        protected BaseValveImageViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateValveImage : BaseValveImageViewModel
    {
        #region Properties

        [Required, DoesNotAutoMap]
        [FileUpload(FileTypes.Tiff, FileTypes.Pdf)]
        public AjaxFileUpload FileUpload { get; set; }
        
        // The view requires a dropdown for streets, but if the street
        // can't be selected from there then the user is allowed to 
        // fill in the Street textbox instead.

        // Also because properties can't end in "Id" and I need this to be
        // an id that identifies something, we ge this name. -Ross 9/23/2014
        [EntityMustExist(typeof(Street))]
        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        [DisplayName("Street"), DoesNotAutoMap]
        public int? StreetIdentifyingInteger { get; set; }

        #endregion

        #region Constructors

        public CreateValveImage(IContainer container) : base(container) { }
        
        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateFileUpload()
        {
            // TODO: This whole validation scheme is terrible and we shouldn't care
            //       about the file name being used. But we have to for backwards
            //       compatability with ViewOne(I guess). This should die the moment
            //       that's converted.

            // All this stuff is handled by regular validation attributes
            if (FileUpload == null) { yield break; }

            var town = _container.GetInstance<ITownRepository>().Find(Town.GetValueOrDefault());

            if (town == null) { yield break; }

            if (_container.GetInstance<IValveImageRepository>().FileExists(FileUpload.FileName, town))
            {
                yield return
                    new ValidationResult("The file uploaded already exists. Please rename and upload again.",
                        new[] { "FileUpload.Key" });
            }
        }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();

            if (Valve.HasValue)
            {
                var v = _container.GetInstance<IValveRepository>().Find(Valve.Value);
                if (v != null)
                {
                    OperatingCenter = v.OperatingCenter.Id;
                    Town = v.Town.Id;
                    StreetNumber = v.StreetNumber;
                    ValveNumber = v.ValveNumber;
                    Location = v.ValveLocation;
                    NumberOfTurns = v.Turns.ToString();
                    if (v.DateInstalled != null)
                    {
                        DateCompleted = v.DateInstalled.Value.ToShortDateString();
                    }
                    if (v.Street != null)
                    {
                        StreetIdentifyingInteger = v.Street.Id;
                        Street = v.Street.Name;
                        StreetPrefix = v.Street.Prefix?.Description;
                        StreetSuffix = v.Street.Suffix?.Description;
                    }
                    if (v.TownSection != null)
                    {
                        TownSection = v.TownSection.Description;
                    }
                    if (v.CrossStreet != null)
                    {
                        CrossStreet = v.CrossStreet.Name;
                        CrossStreetPrefix = v.CrossStreet.Prefix?.Description;
                        CrossStreetSuffix = v.CrossStreet.Suffix?.Description;
                    }
                    if (v.NormalPosition != null)
                    {
                        NormalPosition = v.NormalPosition.Id;
                    }

                    if (v.ValveSize != null)
                    {
                        ValveSize = v.ValveSize.Description;
                    }

                    if (v.OpenDirection != null)
                    {
                        OpenDirection = v.OpenDirection.Id;
                    }
                }
            }
        }

        public override ValveImage MapToEntity(ValveImage entity)
        {
            base.MapToEntity(entity);
            entity.FileName = FileUpload.FileName;
            entity.ImageData = FileUpload.BinaryData;

            if (StreetIdentifyingInteger.HasValue)
            {
                // Validation is going to assume that the street id is valid.
                var s = _container.GetInstance<IStreetRepository>().Find(StreetIdentifyingInteger.Value);
                entity.Street = s.Name;
                entity.StreetPrefix = s.Prefix?.Description;
                entity.StreetSuffix = s.Suffix?.Description;
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateFileUpload());
        }
        #endregion
    }

    public class EditValveImage : BaseValveImageViewModel
    {
        #region Properties

        // The view requires a dropdown for streets, but if the street
        // can't be selected from there then the user is allowed to 
        // fill in the Street textbox instead.

        // Also because properties can't end in "Id" and I need this to be
        // an id that identifies something, we ge this name. -Ross 9/23/2014
        [EntityMustExist(typeof(Street))]
        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        [DisplayName("Street"), DoesNotAutoMap]
        public int? StreetIdentifyingInteger { get; set; }

        public bool OfficeReviewRequired { get; set; }

        #endregion

        #region Constructors

        public EditValveImage(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void Map(ValveImage entity)
        {
            base.Map(entity);
            // This needs to be mapped correctly so cascades don't blow up.
            // If the entity has a Service, use its Street value.
            //
            // A TapImage without a Service set won't have a cascadable street.
            if (entity.Valve != null && entity.Valve.Street != null)
            {
                StreetIdentifyingInteger = entity.Valve.Street.Id;
            }
        }

        #endregion

        #region Public Methods

        public override ValveImage MapToEntity(ValveImage entity)
        {
            base.MapToEntity(entity);

            if (StreetIdentifyingInteger.HasValue)
            {
                // Validation is going to assume that the street id is valid.
                var s = _container.GetInstance<IStreetRepository>().Find(StreetIdentifyingInteger.Value);
                entity.Street = s.Name;
                entity.StreetPrefix = s.Prefix?.Description;
                entity.StreetSuffix = s.Suffix?.Description;
            }

            return entity;
        }

        #endregion
    }

    public class SearchValveImage : SearchSet<ValveImage>
    {
        #region Properties

        [DisplayName("Id")]
        public int? EntityId { get; set; }

        public SearchString ValveNumber { get; set; }

        public string Location { get; set; }

        [DropDown]
        public int? OpenDirection { get; set; }

        public string NumberOfTurns { get; set; }

        [DropDown]
        public int? NormalPosition { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("Town", "T", "State.Id")]
        public int? State { get; set; }

        [DropDown]
        public int? OperatingCenter { get; set; }

        [SearchAlias("Town", "T", "County.Id")]
        [DropDown("", "County", "ByStateId", DependsOn = "State", PromptText = "Select a state above")]
        public int? County { get; set; }

        [DropDown("", "Town", "ByCountyId", DependsOn = "County", PromptText = "Select a county above")]
        public int? Town { get; set; }
        public string TownSection { get; set; }

        public SearchString StreetNumber { get; set; }
        public SearchString ApartmentNumber { get; set; }
        public string StreetPrefix { get; set; }
        public string Street { get; set; }
        public string StreetSuffix { get; set; }
        public string CrossStreet { get; set; }

        // Not a date field in the db
        public string DateCompleted { get; set; }
        public string ValveSize { get; set; }

        public bool? HasAsset { get; set; }
        public bool? HasValidValveNumber { get; set; }
        public bool? OfficeReviewRequired { get; set; }

        public DateRange CreatedAt { get; set; }
        
        #endregion
    }

    public class SearchValveImageLink : SearchSet<ValveImageLinkReportItem>
    {
        [DropDown]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int? OperatingCenter { get; set; }
    }
}