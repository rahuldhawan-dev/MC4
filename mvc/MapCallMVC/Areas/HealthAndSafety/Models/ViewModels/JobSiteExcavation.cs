using System;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class CreateJobSiteExcavation : ViewModel<JobSiteExcavation>
    {
        #region Properties

        [Required, Min(0)]
        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS)]
        public decimal? WidthInFeet { get; set; } // FEET
        [Required, Min(0)]
        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS)]
        public decimal? LengthInFeet { get; set; } // FEET

        [Required, Min(1, ErrorMessage="Depth must be in inches.")]
        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS)]
        public decimal? DepthInInches { get; set; } // INCHES

        [Required, DateTimePicker, DisplayFormat(DataFormatString=CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode=true)]
        public DateTime? ExcavationDate { get; set; }

        [Required, DropDown, EntityMustExist(typeof(JobSiteExcavationLocationType))]
        [EntityMap]
        public int? LocationType { get; set; }

        [DoesNotAutoMap("Display only")]
        public string LocationTypeDescription
        {
            // This is intentionally lazy so the controller doesn't have to re-set this during postback errors.
            get { return TryGetTypeDescription<JobSiteExcavationLocationType>(LocationType); }
        }

        [Required, DropDown, EntityMustExist(typeof(JobSiteExcavationSoilType))]
        [EntityMap]
        public int? SoilType { get; set; }

        [DoesNotAutoMap("Display only")]
        public string SoilTypeDescription
        {
            // This is intentionally lazy so the controller doesn't have to re-set this during postback errors.
            get { return TryGetTypeDescription<JobSiteExcavationSoilType>(SoilType); }
        }

        #endregion

        #region Constructors

        public CreateJobSiteExcavation(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private string TryGetTypeDescription<T>(int? id) where T : ReadOnlyEntityLookup
        {
            if (id.HasValue)
            {
                var entity =
                    _container.GetInstance<MMSINC.Data.NHibernate.IRepository<T>>()
                        .Find(id.Value);
                if (entity != null)
                {
                    return entity.Description;
                }
            }
            return null;
        }

        #endregion

        #region Public Methods

        public override JobSiteExcavation MapToEntity(JobSiteExcavation entity)
        {
            base.MapToEntity(entity);
            var currentUserName = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;
            entity.CreatedBy = currentUserName;
            return entity;
        }

        #endregion
    }
}