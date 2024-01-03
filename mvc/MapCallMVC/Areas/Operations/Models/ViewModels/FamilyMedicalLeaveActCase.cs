using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Operations.Models.ViewModels
{
    public class FamilyMedicalLeaveActCaseViewModel : ViewModel<FamilyMedicalLeaveActCase>
    {
        #region Properties

        [Required, DropDown]
        [EntityMap("Employee"), EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [DisplayName("Frequency")]
        public string FrequencyDays { get; set; }
        public bool CertificationExtended { get; set; }
        public bool SendPackage { get; set; }
        public DateTime? PackageDateSent { get; set; }
        public DateTime? PackageDateReceived { get; set; }
        public DateTime? PackageDateDue { get; set; }
        [DropDown]
        [EntityMap("CompanyAbsenceCertification"), EntityMustExist(typeof(CompanyAbsenceCertification))]
        public int? CompanyAbsenceCertification { get; set; }
        public bool ChronicCondition { get; set; }
        [DisplayName("FMLA Absense Id"), StringLength(AddAbsenseIDToFMLATableForBug2621.STRING_LENGTH)]
        public string AbsenseId { get; set; }
        [StringLength(AddDurationToFamilyMedicalLeaveActForBug2623.STRING_LENGTH)]
        public string Duration { get; set; }

        #endregion

        #region Constructors

        public FamilyMedicalLeaveActCaseViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateFamilyMedicalLeaveActCase : FamilyMedicalLeaveActCaseViewModel
    {
        #region Constructors

		public CreateFamilyMedicalLeaveActCase(IContainer container) : base(container) {}

        #endregion
	}

    public class EditFamilyMedicalLeaveActCase : FamilyMedicalLeaveActCaseViewModel
    {
        #region Constructors

		public EditFamilyMedicalLeaveActCase(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchFamilyMedicalLeaveActCase : SearchSet<FamilyMedicalLeaveActCase>
    {
        #region Properties

        [DropDown, EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("e.OperatingCenter", "oc", "Id")]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Employee", "GetByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above."), EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }

        public DateRange StartDate { get; set; }
        public DateRange EndDate { get; set; }
        public int? CompanyAbsenceCertification { get; set; }

        #endregion
	}
}