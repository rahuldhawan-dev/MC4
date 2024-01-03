using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace Contractors.Models.ViewModels
{
    public class SearchTapImage : SearchSet<TapImage>
    {
        #region Properties

        [View("Id")]
        public int? EntityId { get; set; }

        [DropDown]
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
        public string StreetPrefix { get; set; }
        public SearchString Street { get; set; }
        public SearchString CrossStreet { get; set; }
        public string StreetSuffix { get; set; }
        public string ApartmentNumber { get; set; }
        public string ServiceType { get; set; }
        public string Lot { get; set; }
        public string Block { get; set; }
        [DropDown, View(Service.DisplayNames.SERVICE_SIZE)]
        public int? ServiceSize { get; set; }
        [DropDown, View(Service.DisplayNames.SERVICE_MATERIAL)]
        public int? ServiceMaterial { get; set; }
        public DateRange DateCompleted { get; set; }
        public SearchString ServiceNumber { get; set; }
        public string PremiseNumber { get; set; }

        public bool? HasAsset { get; set; }
        public bool? HasValidPremiseNumber { get; set; }
        public bool? OfficeReviewRequired { get; set; }

        public bool? ServiceMaterialIsNull { get; set; }

        public DateRange CreatedAt { get; set; }

        #endregion
    }

    public abstract class BaseTapImageViewModel : ViewModel<TapImage>
    {
        #region Properties

        [Required, DropDown]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required, DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        // The view requires a dropdown for streets, but if the street
        // can't be selected from there then the user is allowed to 
        // fill in the Street textbox instead.

        // Also because properties can't end in "Id" and I need this to be
        // an id that identifies something, we ge this name. -Ross 9/23/2014
        [EntityMustExist(typeof(Street))]
        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        [View("Street")]
        [DoesNotAutoMap]
        public int? StreetIdentifyingInteger { get; set; }

        [EntityMustExist(typeof(Street))]
        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        [View("Street")]
        [DoesNotAutoMap]
        public int? CrossStreetIdentifyingInteger { get; set; }

        // Needs a controller and a repo and junk
        [EntityMap, EntityMustExist(typeof(Service))]
        [DropDown("", "Service", "ByStreetId", DependsOn = "StreetIdentifyingInteger", PromptText = "Please select a street above")]
        public int? Service { get; set; }

        [DropDown,EntityMap, EntityMustExist(typeof(ServiceSize)), View(MapCall.Common.Model.Entities.Service.DisplayNames.PREVIOUS_SERVICE_SIZE)]
        public int? PreviousServiceSize { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial)), View(MapCall.Common.Model.Entities.Service.DisplayNames.PREVIOUS_SERVICE_MATERIAL)]
        public int? PreviousServiceMaterial { get; set; }

        [StringLength(TapImage.StringLengths.SERVICE_NUMBER)]
        public string ServiceNumber { get; set; }

        [StringLength(TapImage.StringLengths.STREET_PREFIX)]
        public string StreetPrefix { get; set; }

        [StringLength(TapImage.StringLengths.STREET_NUMBER)]
        public string StreetNumber { get; set; }

        [StringLength(TapImage.StringLengths.STREET)]
        public string Street { get; set; }

        [StringLength(TapImage.StringLengths.STREET_SUFFIX)]
        public string StreetSuffix { get; set; }

        [StringLength(TapImage.StringLengths.APARTMENT_NUMBER)]
        public string ApartmentNumber { get; set; }

        [StringLength(TapImage.StringLengths.TOWN_SECTION)]
        public string TownSection { get; set; }

        [StringLength(TapImage.StringLengths.PREMISE_NUMBER)]
        public string PremiseNumber { get; set; }

        [StringLength(TapImage.StringLengths.LOT)]
        public string Lot { get; set; }

        [StringLength(TapImage.StringLengths.BLOCK)]
        public string Block { get; set; }

        [Required, StringLength(TapImage.StringLengths.SERVICE_TYPE)]
        public string ServiceType { get; set; }       

        [StringLength(TapImage.StringLengths.LENGTH_OF_SERVICE)]
        public string LengthOfService { get; set; }

        [StringLength(TapImage.StringLengths.MAIN_SIZE)]
        public string MainSize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize)), View(MapCall.Common.Model.Entities.Service.DisplayNames.SERVICE_SIZE)]
        public int? ServiceSize { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial)), View(MapCall.Common.Model.Entities.Service.DisplayNames.SERVICE_MATERIAL)]
        public int? ServiceMaterial { get; set; }

        [Required]
        public bool? IsDefaultImageForService { get; set; }

        public DateTime? DateCompleted { get; set; }

        [DropDown, EntityMustExist(typeof(ServiceMaterial)), EntityMap]
        public int? CustomerSideMaterial { get; set; }

        [DropDown, EntityMustExist(typeof(ServiceSize)), EntityMap]
        public int? CustomerSideSize { get; set; }


        #endregion

        #region Constructor

        protected BaseTapImageViewModel(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override TapImage MapToEntity(TapImage entity)
        {
            base.MapToEntity(entity);

            if (StreetIdentifyingInteger.HasValue)
            {
                // Validation is going to assume that the street id is valid.
                var s = _container.GetInstance<IRepository<Street>>().Find(StreetIdentifyingInteger.Value);
                entity.Street = s.Name;
                entity.StreetPrefix = s.Prefix?.Description;
                entity.StreetSuffix = s.Suffix?.Description;
            }

            if (CrossStreetIdentifyingInteger.HasValue)
            {
                entity.CrossStreet = _container.GetInstance<IRepository<Street>>().Find(CrossStreetIdentifyingInteger.Value).FullStName;
            }

            return entity;
        }

        #endregion
    }

    public class CreateTapImage : BaseTapImageViewModel
    {
        #region Properties

        [Required, DoesNotAutoMap]
        [FileUpload(FileTypes.Pdf, FileTypes.Tiff)]
        public AjaxFileUpload FileUpload { get; set; }

        #endregion

        #region Constructors

        public CreateTapImage(IContainer container) : base(container) { }

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

            if (_container.GetInstance<ITapImageRepository>().FileExists(FileUpload.FileName, town))
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
            if (Service.HasValue)
            {
                var service = _container.GetInstance<IServiceRepository>().Find(Service.Value);
                if (service != null)
                {
                    if (service.OperatingCenter != null)
                        OperatingCenter = service.OperatingCenter.Id;
                    if (service.Town != null)
                        Town = service.Town.Id;
                    if (service.Street != null)
                        StreetIdentifyingInteger = service.Street.Id;
                    if (service.ServiceType != null)
                        ServiceType =
                            service.ServiceType.Description.TrimEnd(
                                (" " + service.OperatingCenter.OperatingCenterCode).ToCharArray());
                    if (service.LengthOfService.HasValue)
                        LengthOfService = service.LengthOfService.ToString();                   
                    if (service.PreviousServiceSize != null)
                        PreviousServiceSize = service.PreviousServiceSize.Id;
                    if (service.PreviousServiceMaterial != null)
                        PreviousServiceMaterial = service.PreviousServiceMaterial.Id;
                    if (service.ServiceSize != null)
                        ServiceSize = service.ServiceSize.Id;
                    if (service.ServiceMaterial != null)
                        ServiceMaterial = service.ServiceMaterial.Id;
                    if (service.CustomerSideMaterial != null)
                        CustomerSideMaterial = service.CustomerSideMaterial.Id;
                    if (service.CustomerSideSize != null)
                        CustomerSideSize = service.CustomerSideSize.Id;

                    CrossStreetIdentifyingInteger = service.CrossStreet?.Id;
                    StreetNumber = service.StreetNumber;
                    PremiseNumber = service.PremiseNumber;
                    ServiceNumber = service.ServiceNumber.ToString();
                    Service = service.Id;
                    Lot = service.Lot;
                    Block = service.Block;
                    DateCompleted = service.DateInstalled;
                    IsDefaultImageForService = false;
                }
                else
                {
                    throw new InvalidOperationException("Unable to locate the service: " + Service.Value);
                }
            }
        }

        public override TapImage MapToEntity(TapImage entity)
        {
            base.MapToEntity(entity);
            entity.FileName = FileUpload.FileName;
            entity.ImageData = FileUpload.BinaryData;

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateFileUpload());
        }

        #endregion
    }
}