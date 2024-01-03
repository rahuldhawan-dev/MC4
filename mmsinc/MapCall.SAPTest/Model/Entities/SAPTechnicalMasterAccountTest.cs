using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.SAP.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.TechnicalMasterWS;

namespace SAP.DataTest.Model.Entities
{
    [TestClass()]
    public class SAPTechnicalMasterAccountTest
    {
        [TestInitialize]
        public void TestInitialize() { }

        [TestMethod]
        public void TestConstructorSetsValues()
        {
            var thing = new TechnicalMaster_AccountDetailsInfoRecord {
                Device = "86184173",
                DeviceLocation = "6003250360",
                Installation = "7001673835",
                AccountNo = "210023470973",
                AccountStatusAfterReview = "Active",
                Owner = "Holy trinity house of Paper",
                BillingClassification = "New Jersey Residential	",
                CustomerEmail = "holytrinity2@foo.net",
                Phone = "7324390116",
                MobilePhone = "7328675309",
                MeterSize = "5/8",
                InstallationType = "Domestic Water",
                ServiceSize = "3/8",
                Equipment = "52076295",
                ManufacturerSerialNo = "EW0086184173",
                CriticalCareType = "X",
                Customer = "Y"
            };

            var target = new SAPTechnicalMasterAccount(thing);

            Assert.AreEqual(thing.Device, target.DeviceSerialNumber);
            Assert.AreEqual(thing.DeviceLocation, target.DeviceLocation);
            Assert.AreEqual(thing.Installation, target.Installation);
            Assert.AreEqual(thing.AccountNo, target.AccountNumber);
            Assert.AreEqual(thing.AccountStatusAfterReview, target.AccountStatusAfterReview);
            Assert.AreEqual(thing.Customer, target.Customer);
            Assert.AreEqual(thing.Owner, target.Owner);
            Assert.AreEqual(thing.BillingClassification, target.BillingClassificationMapCall);
            Assert.AreEqual(thing.CustomerEmail, target.CustomerEmail);
            Assert.AreEqual(thing.Phone, target.Phone);
            Assert.AreEqual(thing.MobilePhone, target.MobilePhone);
            Assert.AreEqual(thing.MeterSize, target.MeterSizeSAPCode);
            Assert.AreEqual(thing.InstallationType, target.InstallationTypeSAP);
            Assert.AreEqual(thing.ServiceSize, target.ServiceSize);
            Assert.AreEqual(thing.Equipment, target.SAPEquipmentNumber);
            Assert.AreEqual(thing.ManufacturerSerialNo, target.MeterSerialNumber);
            Assert.AreEqual("Successful", target.SAPError);
            Assert.AreEqual(thing.CriticalCareType, target.CriticalCareType);
            Assert.AreEqual(thing.Customer, target.Customer);
        }

        public SearchSapTechnicalMaster GetTechnicalMasterDataBasedOnEquipment()
        {
            var sapTechnicalMasterSearch = new SearchSapTechnicalMaster {
                Equipment = "53518014",
                PremiseNumber = "",
                InstallationType = ""
            };
            return sapTechnicalMasterSearch;
        }

        public SearchSapTechnicalMaster GetTechnicalMasterDataBasedOnPremiseNumber()
        {
            var sapTechnicalMasterSearch = new SearchSapTechnicalMaster {
                Equipment = "",
                PremiseNumber = "9180463515", //9090329362
                InstallationType = ""
            };
            return sapTechnicalMasterSearch;
        }

        public SearchSapTechnicalMaster GetTechnicalMasterDataBasedOnEquipmentAndPremise()
        {
            var sapTechnicalMasterSearch = new SearchSapTechnicalMaster {
                Equipment = "50005831",
                PremiseNumber = "9090329075",
                InstallationType = ""
            };
            return sapTechnicalMasterSearch;
        }

        public SearchSapTechnicalMaster GetTechnicalMasterDataBasedOnEquipmentAndPremiseAndInstallation()
        {
            var sapTechnicalMasterSearch = new SearchSapTechnicalMaster {
                Equipment = "",
                PremiseNumber = "9180452748",
                InstallationType = ""
            };
            return sapTechnicalMasterSearch;
        }

        public SearchSapTechnicalMaster GetTechnicalMasterIncorrectData()
        {
            var sapTechnicalMasterSearch = new SearchSapTechnicalMaster {
                Equipment = "500058311",
                PremiseNumber = "",
                InstallationType = ""
            };
            return sapTechnicalMasterSearch;
        }

        public SearchSapTechnicalMaster GetTechnicalMasterAllNULL()
        {
            var sapTechnicalMasterSearch = new SearchSapTechnicalMaster {
                Equipment = null,
                PremiseNumber = null,
                InstallationType = null
            };
            return sapTechnicalMasterSearch;
        }
    }
}
