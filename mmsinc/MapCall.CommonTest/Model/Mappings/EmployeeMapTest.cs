using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using NHibernate.Linq;
using StructureMap;
using System;
using System.Linq;
using MMSINC.Testing.ClassExtensions;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class EmployeeMapTest : MapCallMvcInMemoryDatabaseTestBase<Employee>
    {
        #region Private Members

        public Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock<IDateTimeProvider>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);
        }

        #endregion

        [TestMethod]
        public void TestOperatingCenterMapsFromOpCode()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var e = GetFactory<EmployeeFactory>().Create(new {OperatingCenter = opc});
            Session.Flush();
            Session.Evict(e);
            e = Session.Query<Employee>().Single(x => x.Id == e.Id);
            Assert.AreEqual(opc.Id, e.OperatingCenter.Id);
        }

        #region CDL

        [TestMethod]
        public void TestIsCDLCompliantReturnsTrueWhenCompliantAndFalseWhenNotCompliant()
        {
            var driversLicenseClass = GetEntityFactory<DriversLicenseClass>().Create(new {Description = "A"});
            var state = GetEntityFactory<State>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var e = GetEntityFactory<Employee>().Create(new {OperatingCenter = opc});

            Assert.IsFalse(e.IsCDLCompliant);

            GetEntityFactory<DriversLicense>()
               .Create(
                    new {
                        Employee = e,
                        DriversLicenseClass = driversLicenseClass,
                        State = state,
                        RenewalDate = DateTime.Now.AddMonths(-1)
                    });
            GetEntityFactory<MedicalCertificate>()
               .Create(new {Employee = e, ExpirationDate = DateTime.Now.AddMonths(-1)});
            Session.Clear();
            e = Session.Get<Employee>(e.Id);

            Assert.IsFalse(e.IsCDLCompliant);

            GetEntityFactory<DriversLicense>()
               .Create(
                    new {
                        Employee = e,
                        DriversLicenseClass = driversLicenseClass,
                        State = state,
                        RenewalDate = DateTime.Now.AddMonths(1)
                    });
            GetEntityFactory<MedicalCertificate>()
               .Create(new {Employee = e, ExpirationDate = DateTime.Now.AddMonths(1)});
            GetEntityFactory<ViolationCertificate>()
               .Create(new {Employee = e, CertificateDate = DateTime.Now.AddMonths(1)});
            Session.Clear();
            e = Session.Get<Employee>(e.Id);

            Assert.IsTrue(e.IsCDLCompliant);
        }

        #region Drivers Licenses

        [TestMethod]
        public void TestDriversLicenseRenewalDateReturnsLatestDriversLicenseRenewalDate()
        {
            var driversLicenseClass = GetEntityFactory<DriversLicenseClass>().Create(new {Description = "A"});
            var state = GetEntityFactory<State>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var e = GetEntityFactory<Employee>().Create(new {OperatingCenter = opc});
            var driversLicense =
                GetEntityFactory<DriversLicense>()
                   .Create(
                        new {
                            Employee = e,
                            DriversLicenseClass = driversLicenseClass,
                            State = state,
                            RenewalDate = DateTime.Now.AddMonths(-1)
                        });

            Session.Clear();
            e = Session.Get<Employee>(e.Id);

            MyAssert.AreClose(driversLicense.RenewalDate.Value, e.DriversLicenseRenewalDate.Value);
            Assert.AreEqual(driversLicense.RenewalDate.Value.BeginningOfDay(),
                e.DriversLicenseRenewalDate.Value.BeginningOfDay());

            var newDriversLicense =
                GetEntityFactory<DriversLicense>()
                   .Create(
                        new {
                            Employee = e,
                            DriversLicenseClass = driversLicenseClass,
                            State = state,
                            RenewalDate = DateTime.Now.AddMonths(1)
                        });

            Session.Clear();
            e = Session.Get<Employee>(e.Id);

            MyAssert.AreClose(newDriversLicense.RenewalDate.Value, e.DriversLicenseRenewalDate.Value);
            Assert.AreNotEqual(driversLicense.RenewalDate.Value.BeginningOfDay(),
                e.DriversLicenseRenewalDate.Value.BeginningOfDay());
        }

        [TestMethod]
        public void TestDriversLicenseIssuedDateReturnsLatestDriversLicenseIssuedDate()
        {
            var driversLicenseClass = GetEntityFactory<DriversLicenseClass>().Create(new {Description = "A"});
            var state = GetEntityFactory<State>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var e = GetEntityFactory<Employee>().Create(new {OperatingCenter = opc});
            var driversLicense =
                GetEntityFactory<DriversLicense>()
                   .Create(
                        new {
                            Employee = e,
                            DriversLicenseClass = driversLicenseClass,
                            State = state,
                            IssuedDate = DateTime.Today.AddYears(-1),
                            RenewalDate = DateTime.Today.AddMonths(-1)
                        });

            Session.Clear();
            e = Session.Get<Employee>(e.Id);

            MyAssert.AreClose(driversLicense.IssuedDate.Value, e.DriversLicenseIssuedDate.Value);
            Assert.AreEqual(driversLicense.IssuedDate.Value.BeginningOfDay(),
                e.DriversLicenseIssuedDate.Value.BeginningOfDay());

            var newDriversLicense =
                GetEntityFactory<DriversLicense>()
                   .Create(
                        new {
                            Employee = e,
                            DriversLicenseClass = driversLicenseClass,
                            State = state,
                            IssuedDate = DateTime.Today,
                            RenewalDate = DateTime.Today.AddMonths(1)
                        });

            Session.Clear();
            e = Session.Get<Employee>(e.Id);

            MyAssert.AreClose(newDriversLicense.IssuedDate.Value, e.DriversLicenseIssuedDate.Value);
            Assert.AreNotEqual(driversLicense.IssuedDate.Value.BeginningOfDay(),
                e.DriversLicenseIssuedDate.Value.BeginningOfDay());
        }

        [TestMethod]
        public void TestDriversLicenseRenewalDaysOverdueReturnsProperNumberOfDaysOverdue()
        {
            var driversLicenseClass = GetEntityFactory<DriversLicenseClass>().Create(new {Description = "A"});
            var state = GetEntityFactory<State>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var target = GetEntityFactory<Employee>().Create(new {OperatingCenter = opc});
            var days = 1;
            GetEntityFactory<DriversLicense>().Create(new {
                Employee = target,
                DriversLicenseClass = driversLicenseClass,
                State = state,
                RenewalDate = DateTime.Now.AddDays(-days)
            });

            Session.Clear();
            target = Session.Get<Employee>(target.Id);

            Assert.AreEqual(days, target.DriversLicenseRenewalDaysOverdue);

            GetEntityFactory<DriversLicense>().Create(new {
                Employee = target,
                DriversLicenseClass = driversLicenseClass,
                State = state,
                RenewalDate = DateTime.Now.AddDays(days)
            });
            Session.Clear();
            target = Session.Get<Employee>(target.Id);

            Assert.AreEqual(0, target.DriversLicenseRenewalDaysOverdue);
        }

        #endregion

        #region Medical Certificates

        [TestMethod]
        public void TestMedicalCertificateExpirationDateReturnsLatestMedicalCertificateExpirationDate()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var e = GetEntityFactory<Employee>().Create(new {OperatingCenter = opc});
            var medicalCertificate =
                GetEntityFactory<MedicalCertificate>()
                   .Create(new {Employee = e, ExpirationDate = DateTime.Now.AddMonths(-1)});
            Session.Clear();
            e = Session.Get<Employee>(e.Id);

            Assert.AreEqual(medicalCertificate.ExpirationDate.BeginningOfDay(),
                e.MedicalCertificateExpirationDate.Value.BeginningOfDay());

            var newMedicalCertificate =
                GetEntityFactory<MedicalCertificate>()
                   .Create(new {Employee = e, ExpirationDate = DateTime.Now.AddYears(1)});

            Session.Clear();
            e = Session.Get<Employee>(e.Id);

            Assert.AreNotEqual(medicalCertificate.ExpirationDate.BeginningOfDay(),
                e.MedicalCertificateExpirationDate.Value.BeginningOfDay());
            Assert.AreEqual(newMedicalCertificate.ExpirationDate.BeginningOfDay(),
                e.MedicalCertificateExpirationDate.Value.BeginningOfDay());
        }

        [TestMethod]
        public void TestMedicalCertificateDaysOverdueReturnsProperNumberOfDays()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var target = GetEntityFactory<Employee>().Create(new {OperatingCenter = opc});

            Assert.AreEqual(0, target.MedicalCertificateDaysOverdue);

            var days = 1;
            GetEntityFactory<MedicalCertificate>()
               .Create(new {Employee = target, ExpirationDate = DateTime.Now.AddDays(-days)});
            Session.Clear();
            target = Session.Get<Employee>(target.Id);

            Assert.AreEqual(days, target.MedicalCertificateDaysOverdue);

            GetEntityFactory<MedicalCertificate>()
               .Create(new {Employee = target, ExpirationDate = DateTime.Now.AddDays(days)});
            Session.Clear();
            target = Session.Get<Employee>(target.Id);

            Assert.AreEqual(0, target.MedicalCertificateDaysOverdue);
        }

        #endregion

        #region Certificates of Violation

        [TestMethod]
        public void TestViolationCertificateExpirationDateReturnsLatestViolationCertificateExpirationDate()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var target = GetEntityFactory<Employee>().Create(new {OperatingCenter = opc});
            var violationCertificate =
                GetEntityFactory<ViolationCertificate>()
                   .Create(new {Employee = target, CertificateDate = DateTime.Now.AddMonths(-1)});

            Session.Clear();
            target = Session.Get<Employee>(target.Id);

            Assert.AreEqual(violationCertificate.CertificateDate.BeginningOfDay().AddYears(1),
                target.ViolationCertificateExpirationDate.Value.BeginningOfDay());

            var newViolationCertificate =
                GetEntityFactory<ViolationCertificate>()
                   .Create(new {Employee = target, CertificateDate = DateTime.Now.AddMonths(1)});
            Session.Clear();
            target = Session.Get<Employee>(target.Id);

            Assert.AreNotEqual(violationCertificate.CertificateDate.BeginningOfDay().AddYears(1),
                target.ViolationCertificateExpirationDate.Value.BeginningOfDay());
            Assert.AreEqual(newViolationCertificate.CertificateDate.BeginningOfDay().AddYears(1),
                target.ViolationCertificateExpirationDate.Value.BeginningOfDay());
        }

        [TestMethod]
        public void TestViolationCertificateDaysOverDueReturnsProperNumberOfDays()
        {
            var now = DateTime.Now;

            // screw leap years.
            now = (now.Day == 29 && now.Month == 2) ? now.AddDays(1) : now;

            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var target = GetEntityFactory<Employee>().Create(new {OperatingCenter = opc});

            Assert.AreEqual(0, target.ViolationCertificateCertificateDaysOverdue);

            var days = 1;
            GetEntityFactory<ViolationCertificate>().Create(new
                {Employee = target, CertificateDate = now.AddYears(-1).AddDays(-days)});
            Session.Clear();
            target = Session.Get<Employee>(target.Id);

            Assert.AreEqual(days, target.ViolationCertificateCertificateDaysOverdue);

            GetEntityFactory<ViolationCertificate>().Create(new
                {Employee = target, CertificateDate = now.AddYears(-1).AddDays(days)});
            Session.Clear();
            target = Session.Get<Employee>(target.Id);

            Assert.AreEqual(0, target.ViolationCertificateCertificateDaysOverdue);
        }

        #endregion

        #endregion

        #region HasOneDayDoctorsNoteRestriction

        [TestMethod]
        public void TestHasOneDayDoctorsNoteRestrictionReturnsTrueIfRestrictionEndDateHasValueAndIsGreaterThanNow()
        {
            var now = DateTime.Now;
            var empWithRestriction = GetFactory<EmployeeFactory>()
               .Create(new {OneDayDoctorsNoteRestrictionEndDate = now.AddDays(2)});
            Session.Refresh(empWithRestriction);
            Assert.IsTrue(empWithRestriction.HasOneDayDoctorsNoteRestriction);

            var empWithExpiredRestriction = GetFactory<EmployeeFactory>()
               .Create(new {OneDayDoctorsNoteRestrictionEndDate = now.AddDays(-1)});
            Session.Refresh(empWithExpiredRestriction);
            Assert.IsFalse(empWithExpiredRestriction.HasOneDayDoctorsNoteRestriction);

            var empWithoutRestriction = GetFactory<EmployeeFactory>().Create();
            Session.Refresh(empWithoutRestriction);
            Assert.IsNull(empWithoutRestriction.OneDayDoctorsNoteRestrictionEndDate, "Sanity check");
            Assert.IsFalse(empWithoutRestriction.HasOneDayDoctorsNoteRestriction);
        }

        [TestMethod]
        public void TestHasOneDayDoctorsNoteRestrictionReturnsTrueForSameDay()
        {
            // NOTE: This test doesn't test anything useful because sqlite's date('now') function
            //       always returns midnight, getdate() for sql server returns the current time. In fact,
            //       by all accounts, this test should fail in sql server.
            var now = DateTime.Now;
            var empWithRestriction = GetFactory<EmployeeFactory>()
               .Create(new {OneDayDoctorsNoteRestrictionEndDate = now.AddHours(-1)});
            Session.Refresh(empWithRestriction);
            Assert.IsTrue(empWithRestriction.HasOneDayDoctorsNoteRestriction);
        }

        #endregion
    }
}
