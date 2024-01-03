using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class DriversLicenseViewModel : ViewModel<DriversLicense>
    {
        #region Private Members

        private MMSINC.Data.NHibernate.IRepository<DriversLicenseRestriction> _restrictionRepo;
        private MMSINC.Data.NHibernate.IRepository<DriversLicenseEndorsement> _endorsementRepo;

        #endregion

        #region Properties

        protected MMSINC.Data.NHibernate.IRepository<DriversLicenseRestriction> RestrictionRepo
        {
            get
            {
                return _restrictionRepo ??
                       (_restrictionRepo = _container.GetInstance<MMSINC.Data.NHibernate.IRepository<DriversLicenseRestriction>>());
            }
        }

        protected MMSINC.Data.NHibernate.IRepository<DriversLicenseEndorsement> EndorsementRepo
        {
            get
            {
                return _endorsementRepo ??
                       (_endorsementRepo = _container.GetInstance<MMSINC.Data.NHibernate.IRepository<DriversLicenseEndorsement>>());
            }
        }

        [DropDown, Required, EntityMustExist(typeof(DriversLicenseClass)), EntityMap]
        public int? DriversLicenseClass { get; set; }
        [DropDown, Required, EntityMustExist(typeof(State)), EntityMap]
        public int? State { get; set; }

        [Required, StringLength(CreateTablesAndSuchForBug2030.StringLengths.LICENSE_NUMBER)]
        public string LicenseNumber { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public DateTime? IssuedDate { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public DateTime? RenewalDate { get; set; }

        [MultiSelect]
        public int[] Endorsements { get; set; }
        [MultiSelect]
        public int[] Restrictions { get; set; }

        #endregion

        #region Constructors

        public DriversLicenseViewModel(IContainer container) : base(container)
        {
            Endorsements = Endorsements ?? new int[0];
            Restrictions = Restrictions ?? new int[0];
        }

        #endregion

        #region Private Methods

        private void UpdateRestrictions(DriversLicense license)
        {
            var toIds = license.Restrictions.Map(x => x.DriversLicenseRestriction.Id);

            foreach (var id in toIds.Where(id => !Restrictions.Contains(id))) {
                license.Restrictions.RemoveSingle(x => x.DriversLicenseRestriction.Id == id);
            }

            toIds = license.Restrictions.Map(x => x.DriversLicenseRestriction.Id);

            foreach (var item in Restrictions.Where(item => !toIds.Contains(item))) {
                license.Restrictions.Add(new DriversLicensesRestriction {
                    DriversLicense = license,
                    DriversLicenseRestriction = RestrictionRepo.Find(item)
                });
            }
        }

        private void UpdateEndorsements(DriversLicense license)
        {
            var toIds = license.Endorsements.Map(x => x.DriversLicenseEndorsement.Id);

            foreach (var id in toIds.Where(id => !Endorsements.Contains(id))) {
                license.Endorsements.RemoveSingle(x => x.DriversLicenseEndorsement.Id == id);
            }

            toIds = license.Endorsements.Map(x => x.DriversLicenseEndorsement.Id);

            foreach (var item in Endorsements.Where(item => !toIds.Contains(item))) {
                license.Endorsements.Add(new DriversLicensesEndorsement {
                    DriversLicense = license,
                    DriversLicenseEndorsement = EndorsementRepo.Find(item)
                });
            }
        }

        #endregion

        #region Exposed Methods

        public override void Map(DriversLicense entity)
        {
            base.Map(entity);

            Endorsements = (from e in entity.Endorsements select e.DriversLicenseEndorsement.Id).ToArray();
            Restrictions = (from e in entity.Restrictions select e.DriversLicenseRestriction.Id).ToArray();
        }

        public override DriversLicense MapToEntity(DriversLicense entity)
        {
            entity = base.MapToEntity(entity);

            UpdateRestrictions(entity);
            UpdateEndorsements(entity);

            return entity;
        }

        #endregion
    }

    public class CreateDriversLicense : DriversLicenseViewModel
    {
        #region Properties

        // Need to map op center just to the view model for renewals.
        [EntityMap(MMSINC.Utilities.ObjectMapping.MapDirections.ToPrimary)]
        [DropDown, EntityMustExist(typeof(OperatingCenter)), Required]
        public int? OperatingCenter { get; set; }

        [DropDown("Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above."), EntityMustExist(typeof(Employee)), Required, EntityMap]
        public int? Employee { get; set; }

        #endregion

        #region Constructors

        public CreateDriversLicense(IContainer container) : base(container) {}

        #endregion
    }

    public class EditDriversLicense : DriversLicenseViewModel
    {
        #region Constructors

        public EditDriversLicense(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchDriversLicense : SearchSet<DriversLicense>
    {
        #region Properties

        [DropDown, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        [DropDown("Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above."), EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }
        [DropDown]
        public int? DriversLicenseClass { get; set; }

        //TODO: Make this work properly
        //[MultiSelect]
        //public int[] Endorsements { get; set; }
        //[MultiSelect]
        //public int[] Restrictions { get; set; }
        public bool? HasHazardMaterialEndorsement { get; set; }
        public bool? HasLiquidBulkTankCargoEndorsement { get; set; }
        public bool? HasHazardMaterialAndTankCombinedEndorsement { get; set; }

        public bool? HasMedicalWavierRequiredRestriction { get; set; }

        [DropDown]
        public int? State { get; set; }
        public string LicenseNumber { get; set; }
        public DateRange IssuedDate { get; set; }
        public DateRange RenewalDate { get; set; }
        public bool? Expired { get; set; }
        [DropDown, SearchAlias("Employee", "e", "Status.Id")]
        public int? EmployeeStatus { get; set; }

        #endregion
    }
}