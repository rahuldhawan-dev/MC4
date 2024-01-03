using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class PWSIDCreatedTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT_CREATED =
            "MapCall.Common.Resources.NotificationTemplates.Environmental.WaterSystems.PWSIDCreated.cshtml";

        [TestMethod]
        public void TestPWSIDTrigger()
        {
            var publicWaterSupply1 = new PublicWaterSupply {
                Identifier = "52033541",
                OperatingArea = "OA",
                System = "S"
            };
            var operatingCenter1 = new OperatingCenter {
                OperatingCenterCode = "QQ1",
                OperatingCenterName = "OCN"
            };
            var model = new PublicWaterSupply {
                Id = 473,
                State = new State {Abbreviation = "XX"},
                System = "Frish Water",
                Identifier = "52033541",
                OperatingCenterPublicWaterSupplies = new HashSet<OperatingCenterPublicWaterSupply> {
                    new OperatingCenterPublicWaterSupply {
                        Id = 123,
                        PublicWaterSupply = publicWaterSupply1,
                        OperatingCenter = operatingCenter1,
                        Abbreviation = "AZ"
                    }
                },
                Status = new PublicWaterSupplyStatus {Description = "Active", Id = 1},
                RecordUrl = "http://recordUrl"
            };
            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT_CREATED, model);

            Assert.AreEqual(
                @"<a href=""http://recordUrl"">View on MapCall</a><br />
State: XX<br />
System: Frish Water<br />
PWSID: 52033541<br />
Operating Center: QQ1 - 52033541<br />
Status: Active<br />
Anticipated Active Date: ", template);
        }

        [TestMethod]
        public void TestPSWIDTriggerWhenAllNullableValuesAreNull()
        {
            var publicWaterSupply1 = new PublicWaterSupply {
                Identifier = "",
                OperatingArea = "",
                System = ""
            };
            var operatingCenter1 = new OperatingCenter {
                OperatingCenterCode = "",
                OperatingCenterName = ""
            };
            var model1 = new PublicWaterSupply {
                Id = 473,
                System = "Frish Water",
                Identifier = "52033541",
                Status = new PublicWaterSupplyStatus {Description = "Active", Id = 1},
                RecordUrl = "http://recordUrl"
            };
            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT_CREATED, model1);

            Assert.AreEqual(
                @"<a href=""http://recordUrl"">View on MapCall</a><br />
State: <br />
System: Frish Water<br />
PWSID: 52033541<br />
Operating Center: <br />
Status: Active<br />
Anticipated Active Date: ", template);
        }

        [TestMethod]
        public void TestPWSIDMultiOperatingCenter()
        {
            var publicWaterSupply1 = new PublicWaterSupply {
                Identifier = "52033541",
                OperatingArea = "OA",
                System = "S"
            };
            var operatingCenter1 = new OperatingCenter {
                OperatingCenterCode = "QQ1",
                OperatingCenterName = "OCN"
            };
            var model2 = new PublicWaterSupply {
                Id = 473,
                State = new State {Abbreviation = "NY"},
                System = "Frish Water",
                Identifier = "52033541",
                OperatingCenterPublicWaterSupplies = new HashSet<OperatingCenterPublicWaterSupply> {
                    new OperatingCenterPublicWaterSupply {
                        Id = 123, PublicWaterSupply = publicWaterSupply1, OperatingCenter = operatingCenter1,
                        Abbreviation = "AZ"
                    },
                    new OperatingCenterPublicWaterSupply {
                        Id = 123, PublicWaterSupply = publicWaterSupply1, OperatingCenter = operatingCenter1,
                        Abbreviation = "AZ"
                    },
                    new OperatingCenterPublicWaterSupply {
                        Id = 123, PublicWaterSupply = publicWaterSupply1, OperatingCenter = operatingCenter1,
                        Abbreviation = "AZ"
                    }
                },
                Status = new PublicWaterSupplyStatus {Description = "Active", Id = 1},
                RecordUrl = "http://recordUrl"
            };
            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT_CREATED, model2);

            Assert.AreEqual(
                @"<a href=""http://recordUrl"">View on MapCall</a><br />
State: NY<br />
System: Frish Water<br />
PWSID: 52033541<br />
Operating Center: QQ1 - 52033541, QQ1 - 52033541, QQ1 - 52033541<br />
Status: Active<br />
Anticipated Active Date: ", template);
        }
    }
}
