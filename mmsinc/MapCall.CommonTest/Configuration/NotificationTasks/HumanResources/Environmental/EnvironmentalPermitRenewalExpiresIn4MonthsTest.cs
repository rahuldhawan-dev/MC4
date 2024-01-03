using System.Linq;
using MapCall.Common.Configuration.NotificationTasks.HumanResources.Environmental;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using Moq;

namespace MapCall.CommonTest.Configuration.NotificationTasks.HumanResources.Environmental
{
    [TestClass]
    public class EnvironmentalPermitRenewalExpiresIn4MonthsTest : EnvironmentalPermitExpirationBaseTest<EnvironmentalPermitRenewalExpiresIn4Months>
    {
        #region Tests

        [TestMethod]
        public void TestNotifyCalledForJustAnEntityPurposeAndNotAllPurposesInAConfiguration()
        {
            _target.SendNotification(_notExpiringChemicalPermit);

            _notificationService.Verify(x => x.Notify(_operatingCenter.Id,
                RoleModules.EnvironmentalPermitTypesExpiration,
                _chemicalPermitPurpose.Purpose,
                _notExpiringChemicalPermit,
                string.Format(
                    EnvironmentalPermitRenewalExpirationBase.EMAIL_SUBJECT_WITHOUT_EXPIRATION_FORMAT,
                    EnvironmentalPermitRenewalExpiresIn4Months.RENEWAL_TEXT),
                null,
                EnvironmentalPermitRenewalExpirationBase.TEMPLATE_NAME), Times.Once());

            _notificationService.Verify(x => x.Notify(_operatingCenter.Id,
                RoleModules.EnvironmentalPermitTypesExpiration,
                _allocationPermitPurpose.Purpose,
                _notExpiringChemicalPermit,
                string.Format(
                    EnvironmentalPermitRenewalExpirationBase.EMAIL_SUBJECT_WITHOUT_EXPIRATION_FORMAT,
                    EnvironmentalPermitRenewalExpiresIn4Months.RENEWAL_TEXT),
                null,
                EnvironmentalPermitRenewalExpirationBase.TEMPLATE_NAME), Times.Never());

            _notificationService.Verify(x => x.Notify(_operatingCenter.Id,
                RoleModules.EnvironmentalPermitTypesExpiration,
                _tankInspectionPermitPurpose.Purpose,
                _notExpiringChemicalPermit,
                string.Format(
                    EnvironmentalPermitRenewalExpirationBase.EMAIL_SUBJECT_WITHOUT_EXPIRATION_FORMAT,
                    EnvironmentalPermitRenewalExpiresIn4Months.RENEWAL_TEXT),
                null,
                EnvironmentalPermitRenewalExpirationBase.TEMPLATE_NAME), Times.Never());
        }

        [TestMethod]
        public void TestEmailNotificationSubjectContainsPermitExpiration()
        {
            _target.SendNotification(_expiringAllocationPermit);

            _notificationService.Verify(x => x.Notify(_operatingCenter.Id,
                RoleModules.EnvironmentalPermitTypesExpiration,
                _allocationPermitPurpose.Purpose,
                _expiringAllocationPermit,
                string.Format(
                    EnvironmentalPermitRenewalExpirationBase.EMAIL_SUBJECT_WITH_EXPIRATION_FORMAT,
                    EnvironmentalPermitRenewalExpiresIn4Months.RENEWAL_TEXT, 
                    _currentDateTime.ToShortDateString()),
                null,
                EnvironmentalPermitRenewalExpirationBase.TEMPLATE_NAME), Times.Once());
        }

        [TestMethod]
        public void TestEmailNotificationSubjectDoesNotContainPermitExpiration()
        {
            _target.SendNotification(_notExpiringChemicalPermit);

            _notificationService.Verify(x => x.Notify(_operatingCenter.Id,
                RoleModules.EnvironmentalPermitTypesExpiration,
                _chemicalPermitPurpose.Purpose,
                _notExpiringChemicalPermit,
                string.Format(EnvironmentalPermitRenewalExpirationBase.EMAIL_SUBJECT_WITHOUT_EXPIRATION_FORMAT,
                    EnvironmentalPermitRenewalExpiresIn4Months.RENEWAL_TEXT),
                null,
                EnvironmentalPermitRenewalExpirationBase.TEMPLATE_NAME), Times.Once());
        }

        [TestMethod]
        public void TestDoesNotReturnMasterPlanPermit()
        {
            var renewalDate = _currentDateTime.AddWeeks(16);
            var status = GetEntityFactory<EnvironmentalPermitStatus>().Create(new {
                Description = "Active"
            });

            var environmentalPermitTypes = GetFactory<EnvironmentalPermitTypeFactory>().CreateList(17);
            foreach (var permitType in environmentalPermitTypes)
            {
                GetFactory<EnvironmentalPermitFactory>().Create(new {
                    EnvironmentalPermitType = permitType,
                    PermitRenewalDate = renewalDate,
                    EnvironmentalPermitStatus = status
                });
            }

            var results = _target.GetData();

            Assert.IsNotNull(results);
            Assert.AreEqual(16, results.Count());
        }

        #endregion
    }
}
