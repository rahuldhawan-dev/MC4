using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class HepatitisBVaccinationViewModel : ViewModel<HepatitisBVaccination>
    {
        #region Properties

        [DropDown, Required, EntityMap, EntityMustExist(typeof(HepatitisBVaccineStatus))]
        public int? HepatitisBVaccineStatus { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public DateTime? ResponseDate { get; set; }

        #endregion
        
        #region Constructors

        public HepatitisBVaccinationViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateHepatitisBVaccination : HepatitisBVaccinationViewModel
    {
        #region Properties

        [DropDown, EntityMustExist(typeof(OperatingCenter)), Required]
        public int? OperatingCenter { get; set; }
        [DropDown("Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above."), EntityMustExist(typeof(Employee)), Required, EntityMap]
        public int? Employee { get; set; }

        #endregion

        #region Constructors

        public CreateHepatitisBVaccination(IContainer container) : base(container) { }

        #endregion
    }

    public class EditHepatitisBVaccination : HepatitisBVaccinationViewModel
    {
        #region Constructors

        public EditHepatitisBVaccination(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchHepatitisBVaccination : SearchSet<HepatitisBVaccination>
    {
        #region Properties

        [DropDown, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        [DropDown("Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above."), EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }
        [DropDown]
        public int? HepatitisBVaccineStatus { get; set; }

        #endregion
    }
}