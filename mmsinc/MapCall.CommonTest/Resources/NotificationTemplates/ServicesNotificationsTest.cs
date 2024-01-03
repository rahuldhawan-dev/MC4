using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class ServicesNotificationsTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestServiceNotificationDoesNotCrashAndBurnBecauseSomeObjectsAreNull()
        {
            var service = new Service {
                OperatingCenter = new OperatingCenter {
                    OperatingCenterCode = "NJ7", 
                    OperatingCenterName = "Shrewsbury"
                },
                Town = new Town {ShortName = "Long Branch"},
                Street = new Street {FullStName = "Easy St."},
                CrossStreet = new Street {FullStName = "Uptown Blvd."},
                PremiseNumber = "101",
                ServiceNumber = 1234,
                ServiceCategory = new ServiceCategory {Description = "Luxury Service"},
                State = new State {Name = "New Jersey"},
                StreetAddress = "1 Water St",
                Name = "Chuck",
                LeadAndCopperCommunicationProvided = false,
                Installation = "Nice Installation",
                Initiator = new User {FullName = "Bill Preston", UserName = "bpreston", Email = "bpreston@nowhere.com"}
            };
            service.SetPropertyValueByName("Id", 101);
            var model = new ServiceNotification {Service = service, UserEmail = "a@b.c"};

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.FieldServices.Assets.ServiceWithSampleSite.cshtml";
            var template = RenderTemplate(streamPath, model);

            Assert.AreEqual(@"
<h2>Service with Sample Site</h2>

Premise Number: <a>101</a>
<br />
Operating Center: NJ7 - Shrewsbury
<br />
Category of Service: Luxury Service
<br />
Date Created: <br />
Street: <a href=""https://mapcall.awapps.com/Map?ControllerName=Service&ActionName=Show&AreaName=FieldOperations&Search%5Bid%5D=101"">
    1 Water St 
</a>
<br />
Town: Long Branch
<br />
<br />
Customer Name: Chuck
<br />
Lead and Copper Communication Provided: False
<br />
Installation: Nice Installation
<br />
Size of Service:  <br />
Service Material: 
<br />
Previous NJAW Material: 
<br />
Previous NJAW Size: <br />
Customer Side Material: 
<br />
Customer Side Size: <br />
Previous Customer Material: 
<br />
Previous Customer Size: <br />
Created By: bpreston", template);
        }

        [TestMethod]
        public void TestServicesNotification()
        {
            var service = new Service {
                OperatingCenter = new OperatingCenter {
                    OperatingCenterCode = "NJ7", 
                    OperatingCenterName = "Shrewsbury"
                },
                Town = new Town {ShortName = "Long Branch"},
                Street = new Street {FullStName = "Easy St."},
                CrossStreet = new Street {FullStName = "Uptown Blvd."},
                PremiseNumber = "101",
                Premise = new Premise {
                    PremiseNumber = "101"
                },
                ServiceNumber = 1234,
                ServiceCategory = new ServiceCategory {Description = "Luxury Service"},
                DateInstalled = new DateTime(2043, 1, 1, 1, 1, 1),
                State = new State {Name = "New Jersey"},
                StreetAddress = "1 Water St",
                Name = "Chuck",
                LeadAndCopperCommunicationProvided = false,
                Installation = "Nice Installation",
                ServiceSize = new ServiceSize {Size = Convert.ToDecimal(3.33)},
                PreviousServiceSize = new ServiceSize {Size = Convert.ToDecimal(3.33)},
                PreviousServiceCustomerSize = new ServiceSize {Size = Convert.ToDecimal(3.33)},
                CustomerSideSize = new ServiceSize {Size = Convert.ToDecimal(3.33)},
                CustomerSideMaterial = new ServiceMaterial {Description = "Tin"},
                ServiceMaterial = new ServiceMaterial {Description = "Tin"},
                PreviousServiceMaterial = new ServiceMaterial {Description = "Tin"},
                PreviousServiceCustomerMaterial = new ServiceMaterial {Description = "Tin"},
                Initiator = new User {FullName = "Bill Preston", UserName = "bpreston", Email = "bpreston@nowhere.com"}
            };
            service.SetPropertyValueByName("Id", 101);
            var model = new ServiceNotification {Service = service, UserEmail = "a@b.c"};

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.FieldServices.Assets.ServiceWithSampleSite.cshtml";
            var template = RenderTemplate(streamPath, model);

            Assert.AreEqual(@"
<h2>Service with Sample Site</h2>

Premise Number: <a>101</a>
<br />
Operating Center: NJ7 - Shrewsbury
<br />
Category of Service: Luxury Service
<br />
Date Created: 1/1/2043<br />
Street: <a href=""https://mapcall.awapps.com/Map?ControllerName=Service&ActionName=Show&AreaName=FieldOperations&Search%5Bid%5D=101"">
    1 Water St 
</a>
<br />
Town: Long Branch
<br />
<br />
Customer Name: Chuck
<br />
Lead and Copper Communication Provided: False
<br />
Installation: Nice Installation
<br />
Size of Service:  3.33<br />
Service Material: Tin
<br />
Previous NJAW Material: Tin
<br />
Previous NJAW Size: 3.33<br />
Customer Side Material: Tin
<br />
Customer Side Size: 3.33<br />
Previous Customer Material: Tin
<br />
Previous Customer Size: 3.33<br />
Created By: bpreston", template);
        }
    }
}
