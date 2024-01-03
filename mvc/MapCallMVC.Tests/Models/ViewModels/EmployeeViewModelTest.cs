using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    public abstract class BaseEmployeeViewModelTest<TViewModel> : ViewModelTestBase<Employee, TViewModel> where TViewModel : EmployeeViewModel
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Address);
            _vmTester.CanMapBothWays(x => x.Address2);
            _vmTester.CanMapBothWays(x => x.CDL);
            _vmTester.CanMapBothWays(x => x.City);
            _vmTester.CanMapBothWays(x => x.CLicense);
            _vmTester.CanMapBothWays(x => x.CommercialDriversLicenseProgramStatus, GetEntityFactory<CommercialDriversLicenseProgramStatus>().Create()); // I think this will fail because CoordinateId
            _vmTester.CanMapBothWays(x => x.DateHired);
            _vmTester.CanMapBothWays(x => x.DateOfCLicense);
            _vmTester.CanMapBothWays(x => x.DateOfNLicense);
            _vmTester.CanMapBothWays(x => x.DateOfSLicense);
            _vmTester.CanMapBothWays(x => x.DateOfTLicense);
            _vmTester.CanMapBothWays(x => x.DateOfWLicense);
            _vmTester.CanMapBothWays(x => x.Department, GetEntityFactory<EmployeeDepartment>().Create());
            _vmTester.CanMapBothWays(x => x.DPCCStatus, GetEntityFactory<DPCCStatus>().Create());
            _vmTester.CanMapBothWays(x => x.DriversLicense);
            _vmTester.CanMapBothWays(x => x.EmergencyContactName);
            _vmTester.CanMapBothWays(x => x.EmergencyContactPhone);
            _vmTester.CanMapBothWays(x => x.EmailAddress); 
            _vmTester.CanMapBothWays(x => x.EmployeeId);
            _vmTester.CanMapBothWays(x => x.FirstName);
            _vmTester.CanMapBothWays(x => x.Gender, GetEntityFactory<Gender>().Create());
            _vmTester.CanMapBothWays(x => x.GETSWPSCard);
            //_vmTester.CanMapBothWays(x => x.HasOneDayDoctorsNoteRestriction); This is a formula field and it doesn't make sense to be in the view model as a mappable field.
            _vmTester.CanMapBothWays(x => x.InactiveDate);
            _vmTester.CanMapBothWays(x => x.InstitutionalKnowledge, GetEntityFactory<InstitutionalKnowledge>().Create());
            _vmTester.CanMapBothWays(x => x.InstitutionalKnowledgeDescription);
            _vmTester.CanMapBothWays(x => x.LastName);
            _vmTester.CanMapBothWays(x => x.LicenseIndustrialDischarge);
            _vmTester.CanMapBothWays(x => x.LicenseSewerCollection);
            _vmTester.CanMapBothWays(x => x.LicenseSewerTreatment);
            _vmTester.CanMapBothWays(x => x.LicenseWaterTreatment);
            _vmTester.CanMapBothWays(x => x.LicenseWaterDistribution);
            _vmTester.CanMapBothWays(x => x.MiddleName);
            _vmTester.CanMapBothWays(x => x.MonthlyDollarLimit);
            _vmTester.CanMapBothWays(x => x.NLicense);
            _vmTester.CanMapBothWays(x => x.OneDayDoctorsNoteRestrictionEndDate);
            _vmTester.CanMapBothWays(x => x.OperatingCenter, GetFactory<UniqueOperatingCenterFactory>().Create());
            _vmTester.CanMapBothWays(x => x.PersonnelArea, GetEntityFactory<PersonnelArea>().Create());
            _vmTester.CanMapBothWays(x => x.PhoneCellular);
            _vmTester.CanMapBothWays(x => x.PhoneHome);
            _vmTester.CanMapBothWays(x => x.PhonePager);
            _vmTester.CanMapBothWays(x => x.PhonePersonalCellular);
            _vmTester.CanMapBothWays(x => x.PhoneWork);
            _vmTester.CanMapBothWays(x => x.PositionGroup, GetEntityFactory<PositionGroup>().Create());
            _vmTester.CanMapBothWays(x => x.PurchaseCardApprover, GetEntityFactory<Employee>().Create());
            _vmTester.CanMapBothWays(x => x.PurchaseCardReviewer, GetEntityFactory<Employee>().Create());
            _vmTester.CanMapBothWays(x => x.PurchaseCardNumber);
            _vmTester.CanMapBothWays(x => x.ReasonForDeparture, GetEntityFactory<ReasonForDeparture>().Create());
            _vmTester.CanMapBothWays(x => x.ReportingFacility, GetEntityFactory<Facility>().Create());
            _vmTester.CanMapBothWays(x => x.ReportsTo, GetEntityFactory<Employee>().Create());
            _vmTester.CanMapBothWays(x => x.SeniorityDate);
            _vmTester.CanMapBothWays(x => x.SeniorityRanking);
            _vmTester.CanMapBothWays(x => x.ScheduleType, GetEntityFactory<ScheduleType>().Create());
            _vmTester.CanMapBothWays(x => x.SingleDollarLimit);
            _vmTester.CanMapBothWays(x => x.SLicense);
            _vmTester.CanMapBothWays(x => x.State);
            _vmTester.CanMapBothWays(x => x.Status, GetEntityFactory<EmployeeStatus>().Create());
            _vmTester.CanMapBothWays(x => x.TCPAStatus, GetEntityFactory<TCPAStatus>().Create());
            _vmTester.CanMapBothWays(x => x.TLicense);
            _vmTester.CanMapBothWays(x => x.UnionAffiliation, GetEntityFactory<UnionAffiliation>().Create());
            _vmTester.CanMapBothWays(x => x.ValidEssentialEmployeeCard);
            _vmTester.CanMapBothWays(x => x.WLicense);
            _vmTester.CanMapBothWays(x => x.ZipCode);
        }

        [TestMethod]
        public void TestCoordinateCanMapBothWays()
        {
            var coord = GetEntityFactory<Coordinate>().Create();
            _entity.Coordinate = coord;

            _vmTester.MapToViewModel();

            Assert.AreEqual(coord.Id, _viewModel.CoordinateId);

            _entity.Coordinate = null;
            _vmTester.MapToEntity();

            Assert.AreSame(coord, _entity.Coordinate);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.CommercialDriversLicenseProgramStatus, GetEntityFactory<CommercialDriversLicenseProgramStatus>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.CoordinateId, GetEntityFactory<Coordinate>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.DPCCStatus, GetEntityFactory<DPCCStatus>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Department, GetEntityFactory<EmployeeDepartment>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Gender, GetEntityFactory<Gender>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.InstitutionalKnowledge, GetEntityFactory<InstitutionalKnowledge>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetFactory<UniqueOperatingCenterFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.PersonnelArea, GetEntityFactory<PersonnelArea>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.PositionGroup, GetEntityFactory<PositionGroup>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.PurchaseCardApprover, GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.PurchaseCardReviewer, GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.ReasonForDeparture, GetEntityFactory<ReasonForDeparture>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.ReportingFacility, GetEntityFactory<Facility>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.ReportsTo, GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.ScheduleType, GetEntityFactory<ScheduleType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Status, GetEntityFactory<EmployeeStatus>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.TCPAStatus, GetEntityFactory<TCPAStatus>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.UnionAffiliation, GetEntityFactory<UnionAffiliation>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EmployeeId);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FirstName);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LastName);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PositionGroup);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.OneDayDoctorsNoteRestrictionEndDate,
                DateTime.Now, x => x.HasOneDayDoctorsNoteRestriction, true, null, "You must set the restriction end date if the employee has a one day doctor's note restriction.");
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Address, Employee.StringLengths.ADDRESS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Address2, Employee.StringLengths.ADDRESS2);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.CDL, Employee.StringLengths.CDL);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.City, Employee.StringLengths.CITY);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.CLicense, Employee.StringLengths.C_LICENSE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.DriversLicense, Employee.StringLengths.DRIVERS_LICENSE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.EmergencyContactName, Employee.StringLengths.EMERGENCY_CONTACT_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.EmergencyContactPhone, Employee.StringLengths.EMERGENCY_CONTACT_PHONE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.EmployeeId, Employee.StringLengths.EMPLOYEE_ID);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.FirstName, Employee.StringLengths.FIRST_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LastName, Employee.StringLengths.LAST_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LicenseIndustrialDischarge, Employee.StringLengths.LICENSE_INDUSTRIAL_DISCHARGE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LicenseSewerCollection, Employee.StringLengths.LICENSE_SEWER_COLLECTION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LicenseSewerTreatment, Employee.StringLengths.LICENSE_SEWER_TREATMENT);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LicenseWaterDistribution, Employee.StringLengths.LICENSE_WATER_DISTRIBUTION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LicenseWaterTreatment, Employee.StringLengths.LICENSE_WATER_TREATMENT);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MiddleName, Employee.StringLengths.MIDDLE_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.NLicense, Employee.StringLengths.N_LICENSE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PhoneCellular, Employee.StringLengths.PHONE_CELLULAR);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PhoneHome, Employee.StringLengths.PHONE_HOME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PhonePager, Employee.StringLengths.PHONE_PAGER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PhonePersonalCellular, Employee.StringLengths.PHONE_PERSONAL_CELLULAR);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PhoneWork, Employee.StringLengths.PHONE_WORK);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PurchaseCardNumber, Employee.StringLengths.PURCHASE_CARD_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.SLicense, Employee.StringLengths.S_LICENSE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.State, Employee.StringLengths.STATE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.TLicense, Employee.StringLengths.T_LICENSE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.WLicense, Employee.StringLengths.W_LICENSE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ZipCode, Employee.StringLengths.ZIP_CODE);

            _viewModel.EmailAddress = "some@email.com".PadRight(Employee.StringLengths.EMAIL_ADDRESS, 'a');
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.EmailAddress, Employee.StringLengths.EMAIL_ADDRESS, true);
        }

        [TestMethod]
        public void TestValidationFailsIfEmployeeIdIsNotUnique()
        {
            var otherEmployee = GetEntityFactory<Employee>().Create(new { EmployeeId = "ABCDEF" });
            _viewModel.EmployeeId = "ABCDEF";

            ValidationAssert.ModelStateHasError(_viewModel, x => x.EmployeeId, "The given employee ID is already being used by another employee.");

            _viewModel.EmployeeId = "ZZZZ";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.EmployeeId);
        }

        #endregion

        #region SetDefaults

        #endregion

        #endregion
    }

    [TestClass]
    public class CreateEmployeeTest : BaseEmployeeViewModelTest<CreateEmployee> { }

    [TestClass]
    public class EditEmployeeTest : BaseEmployeeViewModelTest<EditEmployee> { }
}
