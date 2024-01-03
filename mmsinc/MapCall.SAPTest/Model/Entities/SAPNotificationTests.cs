using MapCall.Common.Model.ViewModels;
using MapCall.SAP.service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.SAP.Model.Entities.Tests
{
    [TestClass()]
    public class SAPNotificationTests
    {
        #region Private Members

        private SearchSapNotification SearchSapNotification;
        private ReceiveNotificationNotifications ReceiveNotification;

        #endregion

        #region Private Methods

        public SearchSapNotification GetTestSearchSapNotification()
        {
            var SearchSapNotification = new SearchSapNotification {
                PlanningPlant = "D203",
                DateCreatedFrom = "20161105", //yyyyMMdd
                DateCreatedTo = "20170415", //yyyyMMdd
                NotificationType = "20,33,35,36,40",
                //Code = "I02",
                //CodeGroup = "N-D-PUR1",
                //Priority = "2",
            };

            return SearchSapNotification;
        }

        public SAPNotification GetTestSearchSapWorkOrder()
        {
            var SAPNotification = new SAPNotification {
                CreateWorkOrderNotificationNumber = "310839587"
            };

            return SAPNotification;
        }

        public SearchSapNotification GetTestSearchSapNotificationForNoData()
        {
            var SearchSapNotification = new SearchSapNotification {
                PlanningPlant = "D208",
                DateCreatedFrom = "20160101",
                DateCreatedTo = "20161231",
                NotificationType = "40"
            };

            return SearchSapNotification;
        }

        public ReceiveNotificationNotifications GetTestServiceResponse()
        {
            var ReceiveNotification = new ReceiveNotificationNotifications();

            ReceiveNotification.NotificationType = "20";
            ReceiveNotification.AssetType = "";
            ReceiveNotification.SAPNotificationNo = "000010001283";
            ReceiveNotification.ShortText = "df";
            ReceiveNotification.LongText = "OSNO";
            ReceiveNotification.ReportedBy = "";
            ReceiveNotification.CustomerName = "";
            ReceiveNotification.Telephone = "";
            ReceiveNotification.PlanningPlant = "D217";
            ReceiveNotification.FunctionalLOC = "NJMM-OT-HYDRT-0001";
            ReceiveNotification.Equipment = "";
            ReceiveNotification.Premise = "";
            ReceiveNotification.CodingGroupCodeDescription = "";
            ReceiveNotification.SAPCode = "I04";
            ReceiveNotification.SAPCodingGroup = "";
            ReceiveNotification.DateCreated = "20150811";
            ReceiveNotification.TimeCreated = "075231";
            ReceiveNotification.Priority = "4: Within 5 Days";
            ReceiveNotification.Address = "";
            ReceiveNotification.House = "";
            ReceiveNotification.Street1 = "";
            ReceiveNotification.Street2 = "";
            ReceiveNotification.Street5 = "";
            ReceiveNotification.City = "";
            ReceiveNotification.OtherCity = "";
            ReceiveNotification.State = "";
            ReceiveNotification.Country = "";
            ReceiveNotification.PostalCode = "";
            ReceiveNotification.Latitude = "";
            ReceiveNotification.Longitude = "";
            ReceiveNotification.SystemStatus = "NOPR";
            ReceiveNotification.UserStatus = "";
            ReceiveNotification.Installation = "TestingInstallation";
            ReceiveNotification.Locality = "TestingLocality";
            ReceiveNotification.LocalityDescription = "TestingLocalityDescription";
            ReceiveNotification.CriticalNotes = "Critical notes, special instructions";
            return ReceiveNotification;
        }

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            SearchSapNotification = GetTestSearchSapNotification();
            ReceiveNotification = GetTestServiceResponse();
        }

        [TestMethod()]
        public void TestConstructorSetsPropertiesForServiceResponse()
        {
            var target = new SAPNotification(GetTestServiceResponse());

            Assert.AreEqual(ReceiveNotification.NotificationType, target.NotificationType);
            Assert.AreEqual(ReceiveNotification.NotificationDescription, target.NotificationTypeText);
            Assert.AreEqual(ReceiveNotification.SAPNotificationNo?.TrimStart('0'), target.SAPNotificationNumber);
            Assert.AreEqual(ReceiveNotification.ShortText, target.NotificationShortText);
            Assert.AreEqual(ReceiveNotification.LongText, target.NotificationLongText);
            Assert.AreEqual(ReceiveNotification.ReportedBy, target.ReportedBy);
            Assert.AreEqual(ReceiveNotification.CustomerName, target.AccountNumberOfCustomer);
            Assert.AreEqual(ReceiveNotification.Telephone, target.Telephone);
            Assert.AreEqual(ReceiveNotification.PlanningPlant, target.PlanningPlant);
            Assert.AreEqual(ReceiveNotification.FunctionalLOC, target.FunctionalLocation);
            Assert.AreEqual(ReceiveNotification.Equipment, target.Equipment);
            Assert.AreEqual(ReceiveNotification.Premise, target.Premise);
            Assert.AreEqual(ReceiveNotification.CodingGroupCodeDescription, target.CodingGroupCodeDescription);
            Assert.AreEqual(ReceiveNotification.SAPCodingGroup, target.PurposeCodingGroup);
            Assert.AreEqual(ReceiveNotification.SAPCode, target.PurposeCodingCode);
            Assert.AreEqual(ReceiveNotification.DateCreated, target.DateCreated);
            Assert.AreEqual(ReceiveNotification.TimeCreated, target.TimeCreated);
            Assert.AreEqual(ReceiveNotification.Priority, target.Priority);
            Assert.AreEqual(ReceiveNotification.Address, target.AddressNumber);
            Assert.AreEqual(ReceiveNotification.House, target.House);
            Assert.AreEqual(ReceiveNotification.Street1, target.Street1);
            Assert.AreEqual(ReceiveNotification.Street2, target.Street2);
            Assert.AreEqual(ReceiveNotification.Street5, target.Street5);
            Assert.AreEqual(ReceiveNotification.City, target.City);
            Assert.AreEqual(ReceiveNotification.OtherCity, target.OtherCity);
            Assert.AreEqual(ReceiveNotification.State, target.State);
            Assert.AreEqual(ReceiveNotification.Country, target.Country);
            Assert.AreEqual(ReceiveNotification.PostalCode, target.CityPostalCode);
            Assert.AreEqual(ReceiveNotification.Latitude, target.Latitude);
            Assert.AreEqual(ReceiveNotification.Longitude, target.Longitude);
            Assert.AreEqual(ReceiveNotification.UserStatus, target.UserStatus);
            Assert.AreEqual(ReceiveNotification.SystemStatus, target.SystemStatus);
            Assert.AreEqual(ReceiveNotification.Installation, target.Installation);
            Assert.AreEqual(ReceiveNotification.Locality, target.Locality);
            Assert.AreEqual(ReceiveNotification.LocalityDescription, target.LocalityDescription);
            Assert.AreEqual(ReceiveNotification.CriticalNotes, target.SpecialInstructions);
        }

        [TestMethod]
        public void TestSetsPropertiesForPurpose()
        {
            var target = new SAPNotification();

            target.CodingGroupCodeDescription = "Customer (Complaint/Request)";
            Assert.AreEqual("Customer", target.Purpose);

            target.CodingGroupCodeDescription = "Equipment Reliability";
            Assert.AreEqual("Equipment Reliability", target.Purpose);

            target.CodingGroupCodeDescription = "Safety";
            Assert.AreEqual("Safety", target.Purpose);

            target.CodingGroupCodeDescription = "AW Compliance";
            Assert.AreEqual("Compliance", target.Purpose);

            target.CodingGroupCodeDescription = "Regulatory";
            Assert.AreEqual("Regulatory", target.Purpose);

            target.CodingGroupCodeDescription = "Seasonal";
            Assert.AreEqual("Seasonal", target.Purpose);

            target.CodingGroupCodeDescription = "Leak Detection";
            Assert.AreEqual("Leak Detection", target.Purpose);

            target.CodingGroupCodeDescription = "Revenue $150-$500";
            Assert.AreEqual("Revenue $150-$500", target.Purpose);

            target.CodingGroupCodeDescription = "Revenue $501-$1000";
            Assert.AreEqual("Revenue $501-$1000", target.Purpose);

            target.CodingGroupCodeDescription = "Revenue >$1000";
            Assert.AreEqual("Revenue >$1000", target.Purpose);

            target.CodingGroupCodeDescription = "Damaged/Billable";
            Assert.AreEqual("Damaged/Billable", target.Purpose);

            target.CodingGroupCodeDescription = "Estimates";
            Assert.AreEqual("Estimates", target.Purpose);

            target.CodingGroupCodeDescription = "Water Quality";
            Assert.AreEqual("Water Quality", target.Purpose);

            target.CodingGroupCodeDescription = "Asset Record Control";
            Assert.AreEqual("Asset Record Control", target.Purpose);

            target.CodingGroupCodeDescription = "Locate";
            Assert.AreEqual("Locate", target.Purpose);

            target.CodingGroupCodeDescription = "Clean Out";
            Assert.AreEqual("Clean Out", target.Purpose);

            target.CodingGroupCodeDescription = "Demolition";
            Assert.AreEqual("Demolition", target.Purpose);
        }

        [TestMethod]
        public void TestSetsPropertiesForMapCallPriority()
        {
            var target = new SAPNotification();

            target.Priority = "1: Emergency 1-2 Hrs";
            Assert.AreEqual("Emergency", target.MapCallPriority);

            target.Priority = "2: Emergency 24 Hrs.";
            Assert.AreEqual("Emergency", target.MapCallPriority);

            target.Priority = "3: One Business Day";
            Assert.AreEqual("High Priority", target.MapCallPriority);

            target.Priority = "4: Within 5 Days";
            Assert.AreEqual("High Priority", target.MapCallPriority);

            target.Priority = "5: Within 15 Days";
            Assert.AreEqual("High Priority", target.MapCallPriority);

            target.Priority = "6: Within 30 Days";
            Assert.AreEqual("Routine", target.MapCallPriority);

            target.Priority = "7: Within 90 Days";
            Assert.AreEqual("Routine", target.MapCallPriority);

            target.Priority = "8: Within 160 Days";
            Assert.AreEqual("Routine", target.MapCallPriority);
        }
    }
}
