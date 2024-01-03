using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class OperationsLockoutFormTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.Operations.LockoutForms.{0}.cshtml";

        #region Tests

        #region Training Record

        [TestMethod]
        public void TestUpdatedLockoutFormNotification()
        {
            var model = new LockoutForm {
                Equipment = new Equipment {Description = "eq 111"},
                ProductionWorkOrder = new ProductionWorkOrder {Id = 1},
                Facility = new Facility {
                    Street = new Street
                        {FullStName = "Main St", Name = "Main", Suffix = new StreetSuffix {Description = "St"}},
                    StreetNumber = "123", Town = new Town {ShortName = "Anytown"}
                },
                RecordUrl = "http://recordUrl",
                RecordUrlWorkOrder = "http://recordUrlWorkOrder"
            };

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "UpdatedLockoutForm", model);

            Assert.AreEqual(@"<h2>Updated Lockout Form</h2>

Operating Center: 
<br />
    <span>WorkOrder #: <a href=""http://recordUrlWorkOrder"">1</a></span>
    <br />
Lockout Form ID:<a href=""http://recordUrl"">0</a>
<br />
Equipment: eq 111<br />
Lockout Reason: 
<br />
Reason for Lockout: 
<br />
Lockout DateTime: 1/1/0001 12:00:00 AM
<br />
Address: 123  Main St Anytown,  <br />
Lockout Device: 
<br />

Out of Service Authorized Employee: 
<br />
Out of Service Date/Time: 1/1/0001 12:00:00 AM
<br />
<br />

Returned to Service Authorized Employee: 
<br />
Returned to Service Date/Time: 
<br />
Returned to Service Notes:
<br />
", template);
        }

        [TestMethod]
        public void TestNewLockoutFormNotification()
        {
            var model = new LockoutForm {
                Equipment = new Equipment {Description = "eq 111"},
                ProductionWorkOrder = new ProductionWorkOrder {Id = 1},
                RecordUrl = "http://recordUrl", RecordUrlWorkOrder = "http://recordUrlWorkOrder"
            };

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "NewLockoutForm", model);

            Assert.AreEqual(@"<h2>Lockout Form Created</h2>

Operating Center: 
<br />
<span>WorkOrder #: <a href=""http://recordUrlWorkOrder"">1</a></span>
<br />
Lockout Form ID:<a href=""http://recordUrl"">0</a> 
<br />
Equipment: eq 111<br />
Lockout Reason: 
<br />
Reason for Lockout: 
<br />
Lockout DateTime: 1/1/0001 12:00:00 AM
<br />
Address: <br />
Lockout Device: 
<br />

Out of Service Authorized Employee: 
<br />
Out of Service Date/Time: 1/1/0001 12:00:00 AM
<br />", template);
        }

        #endregion

        #endregion
    }
}
