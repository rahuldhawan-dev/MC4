using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class RiskRegisterNotificationTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestRiskRegisterNotification()
        {
            var entity = new RiskRegisterAsset
            {
                Id = 101,
                OperatingCenter = new OperatingCenter {
                    OperatingCenterCode = "NJ7",
                    OperatingCenterName = "Shrewsbury",
                    State = new State {
                        Name = "New Jersey",
                        Abbreviation = "NJ"
                    }
                },
                RiskRegisterAssetGroup = new RiskRegisterAssetGroup { Description = "System" },
                RiskRegisterAssetCategory = new RiskRegisterAssetCategory { Description = "Safety" },
                RiskDescription = "Testing Risk Description",
                CofMax = 10,
                ImpactDescription = "Testing Impact Description",
                IdentifiedAt = new DateTime(2022, 2, 22),
                CompletionTargetDate = new DateTime(2022, 2, 28),
                Employee = new Employee {
                  FirstName  = "Sai Sunil",
                  LastName = "Papineni"
                },
                RecordUrl = "https://mapcall.amwater.com/modules/mvc/Engineering/RiskRegister/Show/42"
            };

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.Engineering.RiskRegister.RiskRegisterNotification.cshtml";
            var template = RenderTemplate(streamPath, entity);

            Assert.AreEqual(@"<h2>MapCall Notification - Risk Register Created</h2>

ID: <a href=""https://mapcall.amwater.com/modules/mvc/Engineering/RiskRegister/Show/42"">101</a><br/>
State: NJ<br/>
Operating Center: NJ7 - Shrewsbury<br/>
Grouping: System<br/>
Risk Category: Safety<br/>
Description of Risk: Testing Risk Description<br/>
Impact: 10<br/>
Description of Impact: Testing Impact Description<br/>
Date Risk Identified: 2/22/2022 12:00:00 AM<br/>
Risk Owner: Sai Sunil Papineni<br/>
Target Completion Date: 2/28/2022 12:00:00 AM<br/>
", template);

            entity.CompletionActualDate = new DateTime(2022, 2, 25);
            template = RenderTemplate(streamPath, entity);

            Assert.AreEqual(@"<h2>MapCall Notification - Risk Register Completed</h2>

ID: <a href=""https://mapcall.amwater.com/modules/mvc/Engineering/RiskRegister/Show/42"">101</a><br/>
State: NJ<br/>
Operating Center: NJ7 - Shrewsbury<br/>
Grouping: System<br/>
Risk Category: Safety<br/>
Description of Risk: Testing Risk Description<br/>
Impact: 10<br/>
Description of Impact: Testing Impact Description<br/>
Date Risk Identified: 2/22/2022 12:00:00 AM<br/>
Risk Owner: Sai Sunil Papineni<br/>
Target Completion Date: 2/28/2022 12:00:00 AM<br/>
Actual Completion Date: 2/25/2022 12:00:00 AM<br/>
", template);
        }
    }
}
