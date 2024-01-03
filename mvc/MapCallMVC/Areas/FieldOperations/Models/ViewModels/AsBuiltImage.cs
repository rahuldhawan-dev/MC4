using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public abstract class BaseAsBuiltImageViewModel : ViewModel<AsBuiltImage>
    {
        #region Properties

        [StringLength(AsBuiltImage.StringLengths.COMMENTS)]
        public string Comments { get; set; }
        [Required]
        public DateTime? DateInstalled { get; set; }
        public DateTime? PhysicalInService { get; set; }

        [StringLength(AsBuiltImage.StringLengths.STREET_PREFIX)]
        public string StreetPrefix { get; set; }

        [StringLength(AsBuiltImage.StringLengths.APARTMENT_NUMBER)]
        public string ApartmentNumber { get; set; }

        [StringLength(AsBuiltImage.StringLengths.STREET)]
        public string Street { get; set; }

        [StringLength(AsBuiltImage.StringLengths.STREET_SUFFIX)]
        public string StreetSuffix { get; set; }

        [StringLength(AsBuiltImage.StringLengths.XSTREET_PREFIX)]
        public string CrossStreetPrefix { get; set; }

        [StringLength(AsBuiltImage.StringLengths.CROSS_STREET)]
        public string CrossStreet { get; set; }

        [StringLength(AsBuiltImage.StringLengths.XSTREET_SUFFIX)]
        public string CrossStreetSuffix { get; set; }

        [StringLength(AsBuiltImage.StringLengths.PROJECT_NAME)]
        public string ProjectName { get; set; }

        [StringLength(AsBuiltImage.StringLengths.MAP_PAGE)]
        public string MapPage { get; set; }

        [StringLength(AsBuiltImage.StringLengths.TASK_NUMBER)]
        public string TaskNumber { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required,
         DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter",
             PromptText = "Please select an operating center above")]
        [EntityMap]
        [EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [EntityMap]
        [EntityMustExist(typeof(TownSection))]
        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town",
            PromptText = "Please select a town above")]
        public int? TownSection { get; set; }

        [Coordinate(AddressCallback = "AsBuiltImage.getAddress", IconSet = IconSets.SingleDefaultIcon), EntityMap,
         Required]
        public int? Coordinate { get; set; }

        /// <summary>
        /// Set by MapToEntity if the Coordinate property has been modified.
        /// This is a hack for the notifications in the controller since 
        /// ActionHelper doesn't give you a way of checking to see if the
        /// model has been changed.
        /// </summary>
        internal bool CoordinateChanged { get; set; }

        #endregion

        #region Constructor

        protected BaseAsBuiltImageViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateAsBuiltImage : BaseAsBuiltImageViewModel
    {
        #region Properties

        // Files can only be uploaded during creation, not edit.
        [DoesNotAutoMap]
        [Required, FileUpload(FileTypes.Tiff, FileTypes.Pdf)]
        public AjaxFileUpload FileUpload { get; set; }

        #endregion

        #region Constructors

        public CreateAsBuiltImage(IContainer container) : base(container) { }

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

            if (_container.GetInstance<IAsBuiltImageRepository>().FileExists(FileUpload.FileName, town))
            {
                yield return
                    new ValidationResult("The file uploaded already exists. Please rename and upload again.",
                        new[] { "FileUpload.Key" });
            }
        }

        #endregion

        #region Public Methods

        public override AsBuiltImage MapToEntity(AsBuiltImage entity)
        {
            base.MapToEntity(entity);
            entity.FileName = FileUpload.FileName;
            entity.ImageData = FileUpload.BinaryData;

            // Assuming that the coordinate value is valid.
            if (Coordinate.HasValue)
            {
                entity.CoordinatesModifiedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                entity.CoordinatesModifiedBy =
                    _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;
                CoordinateChanged = true;
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateFileUpload());
        }

        #endregion
    }

    public class EditAsBuiltImage : BaseAsBuiltImageViewModel
    {
        #region Properties

        public bool OfficeReviewRequired { get; set; }

        #endregion

        #region Constructors

        public EditAsBuiltImage(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override AsBuiltImage MapToEntity(AsBuiltImage entity)
        {
            var previousCoord = entity.Coordinate;

            base.MapToEntity(entity);

            if (previousCoord != entity.Coordinate)
            {
                entity.CoordinatesModifiedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                entity.CoordinatesModifiedBy =
                    _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;
                CoordinateChanged = true;
            }

            return entity;
        }

        #endregion
    }

    public class SearchAsBuiltImage : SearchSet<AsBuiltImage>
    {
        #region Properties

        [DisplayName("Id")]
        public int? EntityId { get; set; }

        public virtual DateRange CoordinatesModifiedOn { get; set; }

        [View(AsBuiltImage.Display.TASK_NUMBER)]
        public string TaskNumber { get; set; }

        [DropDown, EntityMustExist(typeof(State)), EntityMap]
        [SearchAlias("Town", "T", "State.Id")]
        public int? State { get; set; }

        [SearchAlias("Town", "T", "County.Id")]
        [DropDown("", "County", "ByStateId", DependsOn = "State", PromptText = "Select a state above")]
        public int? County { get; set; }

        [DropDown("", "Town", "ByCountyId", DependsOn = "County", PromptText = "Select a county above")]
        public int? Town { get; set; }

        [View("Town Section")]
        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? TownSection { get; set; }

        // NOTE: Town does not filter on OperatingCenter because not all AsBuilts have their OperatingCenters set.
        [DropDown]
        public int? OperatingCenter { get; set; }
        public string ProjectName { get; set; }
        public string StreetPrefix { get; set; }
        public SearchString ApartmentNumber { get; set; }
        public string Street { get; set; }
        public string StreetSuffix { get; set; }
        public string CrossStreet { get; set; }
        public string MapPage { get; set; }
        [View("Date Installed", FormatStyle.Date)]
        public DateRange DateInstalled { get; set; }
        [View("Date Added", FormatStyle.Date)]
        public DateRange CreatedAt { get; set; }
        [View("Physical In Service Date")]
        public DateRange PhysicalInService { get; set; }

        [Search(CanMap = false)] // Done in ModifyValues override.
        public bool? HasCoordinate { get; set; }
        public bool? OfficeReviewRequired { get; set; }

        // Not part of the view, but needed to do mapping for HasCoordinate.
        public object Coordinate { get; private set; }

        public SearchString FileName { get; set; }

        #endregion

        #region Public Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            if (HasCoordinate.HasValue)
            {
                mapper.MappedProperties[nameof(Coordinate)].Value = HasCoordinate == true ? SearchMapperSpecialValues.IsNotNull : SearchMapperSpecialValues.IsNull;
            }
        }

        #endregion
    }
}