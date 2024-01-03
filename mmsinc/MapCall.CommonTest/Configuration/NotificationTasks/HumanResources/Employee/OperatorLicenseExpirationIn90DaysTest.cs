using MapCall.Common.Configuration.NotificationTasks.HumanResources.Employee;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.CommonTest.Configuration.NotificationTasks.HumanResources.Employee
{
    [TestClass]
    public class OperatorLicenseExpirationIn90DaysTest : OperatorLicenseExpirationBaseTest<OperatorLicenseExpirationIn90Days>
    {
        #region Tests

        [TestMethod]
        public void TestNotifyCalledForAnEmployeeAndAnyNotificationConfigurations()
        {
            var targetExpirationDate = _dateTimeProvider.Object.GetCurrentDate().AddDays(90);

            var operatorLicense = GetEntityFactory<OperatorLicense>().Create(new {
                OperatingCenter = _operatingCenter,
                OperatorLicenseType = GetEntityFactory<OperatorLicenseType>().Create(new {
                    Description = "Type 01"
                }),
                Employee = GetEntityFactory<MapCall.Common.Model.Entities.Employee>().Create(new {
                    CommercialDriversLicenseProgramStatus = GetEntityFactory<CommercialDriversLicenseProgramStatus>().Create()
                }),
                State = GetEntityFactory<State>().Create(),
                LicenseLevel = "license level for test",
                LicenseSubLevel = "license sub level for test",
                LicenseNumber = "license number for test",
                LicensedOperatorOfRecord = true,
                ExpirationDate = targetExpirationDate
            });

            Session.Save(operatorLicense);

            var subjectLine = $"Licensed Operator Renewal Due in 90 Days, Expiration Date: {targetExpirationDate}";

            _target.SendNotification(operatorLicense);

            _notifier.Verify(x => x.Notify(
                OperatorLicenseExpirationBase.APPLICATION,
                OperatorLicenseExpirationBase.MODULE,
                OperatorLicenseExpirationBase.PURPOSE,
                operatorLicense,
                It.IsAny<string>(),
                subjectLine,
                null,
                null
            ), Times.Exactly(2));
        }

        #endregion
    }
}
