using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.HumanResources.Models.ViewModels
{
    public class SearchEmployeeAccountabilityAction : SearchSet<EmployeeAccountabilityAction>
    {
        #region Properties

        #region Original

        [EntityMap, EntityMustExist(typeof(Employee))]
        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        public virtual int? Employee { get; set; }
        [DropDown, EntityMustExist(typeof(Employee)), EntityMap]
        public virtual int? DisciplineAdministeredBy { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(AccountabilityActionTakenType))]
        public int? AccountabilityActionTakenType { get; set; }
        public DateRange DateAdministered { get; set; }
        public DateRange StartDate { get; set; }
        public DateRange EndDate { get; set; }
        public int? NumberOfWorkDays { get; set; }
        [DropDown]
        [EntityMap, EntityMustExist(typeof(Incident))]
        public int? Incident { get; set; }

        [DropDown]
        [EntityMap, EntityMustExist(typeof(Grievance))]
        public int? Grievance { get; set; }

        #endregion

        #region Modified

        public bool? HasModifiedDiscipline { get; set; }
        [DropDown, EntityMustExist(typeof(Employee)), EntityMap]
        public int? ModifiedDisciplineAdministeredBy { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(AccountabilityActionTakenType))]
        public int? ModifiedAccountabilityActionTakenType { get; set; }
        public DateRange DateModified { get; set; }
        public DateRange ModifiedStartDate { get; set; }
        public DateRange ModifiedEndDate { get; set; }
        public int? ModifiedNumberOfWorkDays { get; set; }
        public bool? BackPayRequired { get; set; }

        #endregion

        #endregion
    }
}