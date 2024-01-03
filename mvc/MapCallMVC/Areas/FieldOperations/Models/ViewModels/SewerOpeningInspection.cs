using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SewerOpeningInspectionViewModel : ViewModel<SewerOpeningInspection>
    {
        #region Properties

        [DoesNotAutoMap]
        public SewerOpeningInspection Display
        {
            get
            {
                var repo = _container.GetInstance<IRepository<SewerOpeningInspection>>();
                return repo.Find(Id);
            }
        }

        [DropDown, EntityMap, EntityMustExist(typeof(SewerOpening)), Secured]
        public int? SewerOpening { get; set; }
        [DoesNotAutoMap]
        public bool IsMapPopup { get; set; }
        [DoesNotAutoMap, View("SewerOpening")]
        public SewerOpening SewerOpeningDisplay
        {
            get
            {
                return _container.GetInstance<IRepository<SewerOpening>>().Find(SewerOpening.GetValueOrDefault());
            }
        }
        [Required, Secured(AppliesToAdmins = false)]
        public DateTime? DateInspected { get; set; }
        [Required]
        [Range(0, 99.99)]
        public decimal? RimToWaterLevelDepth { get; set; }
        [Required]
        [Range(0, 99.99)]
        public decimal? RimHeightAboveBelowGrade { get; set; }
        [StringLength(SewerOpeningInspection.StringLengths.PIPES)]
        public string PipesIn { get; set; }
        [StringLength(SewerOpeningInspection.StringLengths.PIPES)]
        public string PipesOut { get; set; }
        [Range(0, 99999.99)]
        public decimal? AmountOfDebrisGritCubicFeet { get; set; }
        [StringLength(SewerOpeningInspection.StringLengths.REMARKS)]
        public string Remarks { get; set; }

        #endregion
        #region Constructor

        public SewerOpeningInspectionViewModel(IContainer container) : base(container)
        {
            
        }

        #endregion
    }

    public class CreateSewerOpeningInspection : SewerOpeningInspectionViewModel
    {
        public CreateSewerOpeningInspection(IContainer container) : base(container)
        {
            
        }
        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            DateInspected = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
        }

        public override SewerOpeningInspection MapToEntity(SewerOpeningInspection entity)
        {
            entity = base.MapToEntity(entity);
            entity.InspectedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            return entity;
        }

        #endregion
    }

    public class EditSewerOpeningInspection : SewerOpeningInspectionViewModel
    {
        public User InspectedBy
        {
            get { return _container.GetInstance<IRepository<SewerOpeningInspection>>().Find(Id).InspectedBy; }
        }
        
        public EditSewerOpeningInspection(IContainer container) : base(container) { }
    }

    public class SearchSewerOpeningInspection : SearchSet<SewerOpeningInspectionSearchResultViewModel>, ISearchSewerOpeningInspection
    {
        [Required, DropDown, EntityMustExist(typeof(OperatingCenter)), EntityMap()]
        public int? OperatingCenter { get; set; }
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }
        [DropDown("FieldOperations", "SewerOpening", "RouteByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? Route { get; set; }
        [DropDown("", "User", "GetAllByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? InspectedBy { get; set; }
        public DateRange DateInspected { get; set; }
        public int? OpeningSuffix { get; set; }
    }
}
