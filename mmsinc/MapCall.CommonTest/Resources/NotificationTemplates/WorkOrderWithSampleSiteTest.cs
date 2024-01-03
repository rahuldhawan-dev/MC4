using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class WorkOrderTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestWorkOrderCreatedNotification()
        {
            var _sampleSites = new SampleSite {
                Id = 9001,
                Street = new Street {Name = " 100000 water jr st"},
                Town = new Town {ShortName = "Anytown", State = new State {Name = "Newer Jersey"}}
            };
            var model = new WorkOrder {
                RecordUrl = "http://mapcall.awapps.com/Modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg=1",
                Id = 1,
                SAPWorkOrderNumber = 123345567,
                PremiseNumber = "9001",
                Installation = 9002,
                OperatingCenter = new OperatingCenter {OperatingCenterName = "TestOC"},
                WorkDescription = new WorkDescription {Description = "Work Work"},
                DateReceived = new DateTime(2043, 1, 1, 1, 1, 1),
                DateCompleted = new DateTime(2043, 1, 1, 1, 1, 1),
                CustomerServiceLineMaterial = new ServiceMaterial(),
                CreatedBy = new User {FullName = "test user"},
                Street = new Street {Name = " 100000 water jr st"},
                StreetNumber = "123",
                Town = new Town {ShortName = "Anytown", State = new State {Name = "Newer Jersey"}},
                SampleSites = {_sampleSites},
                CustomerName = "Stimpy"
            };
                
            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.FieldServices.WorkManagement.WorkOrderWithSampleSite.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>Work Order with Sample Site</h2>

Work Order Number #:
<a href=""http://mapcall.awapps.com/Modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&amp;arg=1"">1</a>
<br />
Premise #: 9001
<br />
Operating Center:  - TestOC
<br />
Description of Job: Work Work
<br />
Date Received: 1/1/2043<br />
Date Completed: 1/1/2043<br />
Street:<a href=""https://www.google.com/maps/place/123  Anytown Newer Jersey""> 123  Anytown Newer Jersey</a>
<br />
Town: Anytown
<br /><br />
Customer Name: Stimpy
<br /> 
Sample Site: 9001 Anytown  
<br />
Installation: 9002
<br /><br />
Service Material: 
<br />
Previous NJAW Material: 
<br />
Customer Side Size: 
<br />
Created By:  test user
<br />";
            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void TestWorkOrderWithMinimalDetailsCreatedNotification()
        {
            var _sampleSites = new SampleSite {
                Id = 9001,
                Street = new Street {Name = " 100000 water jr st"},
                Town = new Town {ShortName = "Anytown", State = new State {Name = "Newer Jersey"}}
            };
            var model = new WorkOrder {
                RecordUrl = "http://recordUrl",
                Id = 1,
                SAPWorkOrderNumber = 123345567,
                PremiseNumber = "9001",
                OperatingCenter = new OperatingCenter {OperatingCenterName = "TestOC"},
                DateReceived = new DateTime(2043, 1, 1, 1, 1, 1),
                DateCompleted = new DateTime(2043, 1, 1, 1, 1, 1),
                Street = new Street {Name = " 100000 water jr st"},
                Town = new Town {ShortName = "Anytown", State = new State {Name = "Newer Jersey"}},
                SampleSites = {_sampleSites},
                CustomerName = "Stimpy"
            };
            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.FieldServices.WorkManagement.WorkOrderWithSampleSite.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>Work Order with Sample Site</h2>

Work Order Number #:
<a href=""http://recordUrl"">1</a>
<br />
Premise #: 9001
<br />
Operating Center:  - TestOC
<br />
Description of Job: 
<br />
Date Received: 1/1/2043<br />
Date Completed: 1/1/2043<br />
Street:<a href=""https://www.google.com/maps/place/ Anytown Newer Jersey"">  Anytown Newer Jersey</a>
<br />
Town: Anytown
<br /><br />
Customer Name: Stimpy
<br /> 
Sample Site: 9001 Anytown  
<br />
Installation: 
<br /><br />
Service Material: 
<br />
Previous NJAW Material: 
<br />
Customer Side Size: 
<br />
Created By:  
<br />";
            Assert.AreEqual(expected, template);
        }
    }
}
