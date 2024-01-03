using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.StructureMap;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ServiceTest : InMemoryDatabaseTest<Service>
    {
        [TestMethod]
        public void TestDescriptionReturnsServiceNumberAndPremiseNumber()
        {
            var target = new Service();
            target.ServiceNumber = 9876543210;
            target.PremiseNumber = "Okay";
            target.ServiceType = new ServiceType {
                Description = "Neat"
            };
            Assert.AreEqual("[Id] 0 [Service] 9876543210, [Premise] Okay, [Service Type] Neat", target.Description);
        }

        [TestMethod]
        public void TestDescriptionReturnsServiceNumberAndPremiseNumberServiceNumberNull()
        {
            var target = new Service();
            target.ServiceNumber = null;
            target.PremiseNumber = "Okay";
            target.ServiceType = new ServiceType {
                Description = "Neat"
            };
            Assert.AreEqual("[Id] 0 [Service] , [Premise] Okay, [Service Type] Neat", target.Description);
        }

        [TestMethod]
        public void TestToStringReturnsServiceNumberAndPremiseNumber()
        {
            var target = new Service();
            target.ServiceNumber = 9876543210;
            target.PremiseNumber = "Okay";
            target.ServiceType = new ServiceType {
                Description = "Neat"
            };
            Assert.AreEqual("[Id] 0 [Service] 9876543210, [Premise] Okay, [Service Type] Neat", target.ToString());
        }

        [TestMethod]
        public void TestToStringReturnsServiceNumberAndPremiseNumberNullServiceNumber()
        {
            var target = new Service();
            target.ServiceNumber = null;
            target.PremiseNumber = "Okay";
            target.ServiceType = new ServiceType {
                Description = "Neat"
            };
            Assert.AreEqual("[Id] 0 [Service] , [Premise] Okay, [Service Type] Neat", target.ToString());
        }

        [TestMethod]
        public void TestStatusMessages()
        {
            var target = new Service();
            var contactDate = DateTime.Now;

            Assert.AreEqual(string.Format(Service.StatusMessages.APPLICATION_NOT_SENT, string.Empty),
                target.StatusMessage);

            target.ContactDate = contactDate;
            Assert.AreEqual(string.Format(Service.StatusMessages.APPLICATION_NOT_SENT, contactDate),
                target.StatusMessage);

            var appSentOn = contactDate.AddDays(2);
            target.ApplicationSentOn = appSentOn;
            Assert.AreEqual(string.Format(Service.StatusMessages.APPLICATION_NOT_RETURNED, appSentOn),
                target.StatusMessage);

            var appReceivedOn = contactDate.AddDays(3);
            target.ApplicationReceivedOn = appReceivedOn;
            Assert.AreEqual(string.Format(Service.StatusMessages.APPLICATION_NOT_APPROVED, appReceivedOn),
                target.StatusMessage);

            var permitSentDate = contactDate.AddDays(4);
            target.PermitSentDate = permitSentDate;
            Assert.AreEqual(string.Format(Service.StatusMessages.PERMIT_PENDING, permitSentDate), target.StatusMessage);

            var permitReceivedDate = contactDate.AddDays(5);
            target.PermitReceivedDate = permitReceivedDate;
            Assert.AreEqual(string.Format(Service.StatusMessages.PERMIT_RECEIVED, permitReceivedDate),
                target.StatusMessage);

            var issuedDate = contactDate.AddDays(6);
            target.DateIssuedToField = issuedDate;
            Assert.AreEqual(string.Format(Service.StatusMessages.ISSUED, issuedDate), target.StatusMessage);

            var dateInstalled = contactDate.AddDays(10);
            target.DateInstalled = dateInstalled;
            Assert.AreEqual(string.Format(Service.StatusMessages.INSTALLED, dateInstalled), target.StatusMessage);
        }

        [TestMethod]
        public void TestSAPErrorCodeType()
        {
            var target = new Service();

            target.SAPErrorCode = null;
            Assert.AreEqual(ServiceSAPErrorCodeType.Unknown, target.SAPErrorCodeType);
            target.SAPErrorCode = string.Empty;
            Assert.AreEqual(ServiceSAPErrorCodeType.Unknown, target.SAPErrorCodeType);
            target.SAPErrorCode = "Blippity blop";
            Assert.AreEqual(ServiceSAPErrorCodeType.Unknown, target.SAPErrorCodeType);
            target.SAPErrorCode = "what the? InVaLiD DeVIce LOcAtIon   ";
            Assert.AreEqual(ServiceSAPErrorCodeType.InvalidDeviceLocation, target.SAPErrorCodeType);
        }

        #region WarrantyExpirationDate

        [TestMethod]
        public void TestWarrantyExpirationDateReturnsNullIfCustomerSideReplacementDateIsNull()
        {
            var target = new Service();
            target.CustomerSideReplacementDate = null;
            Assert.IsNull(target.WarrantyExpirationDate);
        }

        [TestMethod]
        public void TestWarrantyExpirationDateReturnsCustomerSideReplacementDateWithOneYearAdded()
        {
            var targetDate = DateTime.Now;
            var expectedDate = targetDate.AddYears(1);
            var target = new Service();
            target.CustomerSideReplacementDate = targetDate;
            Assert.AreEqual(expectedDate, target.WarrantyExpirationDate);
        }


        #endregion

        [TestMethod]
        public void TestDepthMainReturnsCorrectlyFormattedInAllCases()
        {
            var target = new Service();
            Assert.AreEqual(String.Empty, target.DepthMain);

            target.DepthMainFeet = 1;
            Assert.AreEqual(target.DepthMainFeet + "'", target.DepthMain);

            target.DepthMainInches = 3;
            Assert.AreEqual($"{target.DepthMainFeet}' {target.DepthMainInches}\"", target.DepthMain);

            target.DepthMainFeet = null;
            Assert.AreEqual($"{target.DepthMainInches}\"", target.DepthMain);
        }

        [TestMethod]
        public void TestRelatedTapImages()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var target = new Service {
                Id = 12,
                OperatingCenter = opc,
                Town = town,
                PremiseNumber = "1234567"
            };
            var image1 = GetFactory<TapImageFactory>().Create(new {
                IsDefaultImageForService = true,
                OperatingCenter = opc,
                Town = town,
                PremiseNumber = "1234567"
            });
            var image2 = GetFactory<TapImageFactory>().Create(new {
                IsDefaultImageForService = false,
                OperatingCenter = opc,
                Town = town
            });
            target.TapImageRepository =
                new TapImageRepository(GetSession(new ContainerToContextAdapter(_container)), _container, new ImageToPdfConverter());
            
            Assert.AreEqual(1, target.RelatedTapImages.Count);
            Assert.AreEqual(image1.Id, target.RelatedTapImages[0].Id);

            target.PremiseNumber = null;

            Assert.AreEqual(0, target.RelatedTapImages.Count);
        }
    }
}
