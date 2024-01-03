using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallImporter.Tests.Models.Import
{
    [TestClass]
    public class ServiceExcelRecordTest : ExcelRecordTestBase<Service, MyCreateService, ServiceExcelRecord>
    {
        protected override ServiceExcelRecord CreateTarget()
        {
            return new ServiceExcelRecord {
                PremiseNumber = "1234567890",
                Installation = "0987654321",
                StreetNumber = "123",
                OperatingCenter = $"{OperatingCenters.NJ7.CODE} - {OperatingCenters.NJ7.NAME}",
                CategoryofService = "Water Service New Domestic",
                State = NJState.ABBREVIATION,
                Street = AberdeenMonmouthNJStreets.ChurchStreet.NAME,
                Town = AberdeenMonmouthNJTown.TOWN,
                DeviceLocation = "6012458832"
            };
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<Service, MyCreateService, ServiceExcelRecord> test)
        {
            test.RequiredString(x => x.PremiseNumber, x => x.PremiseNumber);
            test.RequiredString(x => x.Installation, x => x.Installation);
            test.RequiredString(x => x.StreetNumber, x => x.StreetNumber);

            test.String(x => x.Block, x => x.Block);
            test.String(x => x.Lot, x => x.Lot);
            test.Decimal(x => x.AdditionalServiceInstallationFee, x => x.ServiceInstallationFee);
            test.Decimal(x => x.AmountReceived, x => x.AmountReceived);
            test.String(x => x.AptBldg, x => x.ApartmentNumber);
            test.Boolean(x => x.AssociatedDepositAgreement, x => x.Agreement);
            test.Boolean(x => x.BureauOfSafeDrinkingWaterPermitRequired,
                x => x.BureauOfSafeDrinkingWaterPermitRequired);
            test.Boolean(x => x.CleanedCoordinates, x => x.CleanedCoordinates);
            test.DateTime(x => x.CustomerSideReplacementDate, x => x.CustomerSideReplacementDate);
            test.Decimal(x => x.CustomerSideSLReplacementCost, x => x.CustomerSideSLReplacementCost);
            test.DateTime(x => x.CustomerSideSLWarrantyExpiration, x => x.CustomerSideSLWarrantyExpiration);
            test.DateTime(x => x.DateClosed, x => x.DateClosed);
            test.DateTime(x => x.DateCreditProcessed, x => x.DateCreditProcessed);
            test.DateTime(x => x.DateIssuedToField, x => x.DateIssuedToField);
            test.Int(x => x.DepthMainFeet, x => x.DepthMainFeet);
            test.Int(x => x.DepthMainInches, x => x.DepthMainInches);
            test.Boolean(x => x.DeveloperServicesDriven, x => x.DeveloperServicesDriven);
            test.String(x => x.Development, x => x.Development);
            test.String(x => x.DeviceLocation, x => x.DeviceLocation);
            test.Boolean(x => x.DeviceLocationUnavailable, x => x.DeviceLocationUnavailable);
            test.String(x => x.GeoEFunctionalLocation, x => x.GeoEFunctionalLocation);
            test.Int(x => x.ImageActionID, x => x.ImageActionID);
            test.DateTime(x => x.InitialContactDate, x => x.ContactDate);
            test.Decimal(x => x.InstallationCost, x => x.InstallationCost);
            test.DateTime(x => x.InstallationInvoiceDate, x => x.InstallationInvoiceDate);
            test.String(x => x.InstallationInvoiceNumber, x => x.InstallationInvoiceNumber);
            test.String(x => x.JobNotes, x => x.JobNotes);
            test.Boolean(x => x.LeadAndCopperCommunicationProvided, x => x.LeadAndCopperCommunicationProvided);
            test.Int(x => x.LengthOfCustomerSideSLReplaced, x => x.LengthOfCustomerSideSLReplaced);
            test.Decimal(x => x.LengthOfService, x => x.LengthOfService);
            test.String(x => x.MeterSetAccount, x => x.RetireMeterSet);
            test.Boolean(x => x.MeterSettingRequirement, x => x.MeterSettingRequirement);
            test.String(x => x.Name, x => x.Name);
            test.Int(x => x.NSINumber, x => x.NSINumber);
            test.Int(x => x.ObjectID, x => x.ObjectId);
            test.DateTime(x => x.OriginalInstallationDate, x => x.OriginalInstallationDate);
            test.String(x => x.ParentTaskNumber, x => x.ParentTaskNumber);
            test.String(x => x.PaymentReferenceNumber, x => x.PaymentReferenceNumber);
            test.DateTime(x => x.PermitExpirationDate, x => x.PermitExpirationDate);
            test.String(x => x.PermitNumber, x => x.PermitNumber);
            test.String(x => x.PaymentReferenceNumber, x => x.PaymentReferenceNumber);
            test.DateTime(x => x.PermitSentDate, x => x.PermitSentDate);
            test.String(x => x.PhoneNumber, x => x.PhoneNumber);
            test.Boolean(x => x.PitInstalled, x => x.PitInstalled);
            test.String(x => x.PurchaseOrderNumber, x => x.PurchaseOrderNumber);
            test.Decimal(x => x.RoadOpeningFee, x => x.RoadOpeningFee);
            test.String(x => x.SAPStatus, x => x.SAPErrorCode);
            test.Long(x => x.SAPNotificationNumber, x => x.SAPNotificationNumber, 100000000);
            test.String(x => x.ServiceAccount, x => x.RetiredAccountNumber);
            test.Long(x => x.ServiceNumber, x => x.ServiceNumber);
            test.String(x => x.StreetAddress, x => x.StreetAddress);
            test.Long(x => x.SAPWorkOrderNumber, x => x.SAPWorkOrderNumber, 100000000);
            test.String(x => x.TapOrderNotes, x => x.TapOrderNotes);
            test.String(x => x.TaskNumber2, x => x.TaskNumber2);
            test.String(x => x.WBS, x => x.TaskNumber1);
            test.String(x => x.Zip, x => x.Zip);

            test.TestedElsewhere(x => x.OperatingCenter);
            test.TestedElsewhere(x => x.CategoryofService);
            test.TestedElsewhere(x => x.State);
            test.TestedElsewhere(x => x.Town);
            test.TestedElsewhere(x => x.Street);
            test.TestedElsewhere(x => x.CrossStreet);
            test.TestedElsewhere(x => x.CustomerSideMaterial);
            test.TestedElsewhere(x => x.CreatedOn);
            test.TestedElsewhere(x => x.CustomerSideReplacementWBSNumber);
            test.TestedElsewhere(x => x.CustomerSideSize);
            test.TestedElsewhere(x => x.LastUpdated);
            test.TestedElsewhere(x => x.PermitReceivedDate);
            test.TestedElsewhere(x => x.RetiredDate);
            test.TestedElsewhere(x => x.InstalledDate);
            test.TestedElsewhere(x => x.CustomerSideSLReplacedBy);
            test.TestedElsewhere(x => x.CustomerSideSLReplacementContractor);
            test.TestedElsewhere(x => x.CustomerSideSLReplacement);
            test.TestedElsewhere(x => x.FlushingofCustomerPlumbing);
            test.TestedElsewhere(x => x.MeterSettingSize);
            test.TestedElsewhere(x => x.PermitType);
            test.TestedElsewhere(x => x.PreviousServiceMaterial);
            test.TestedElsewhere(x => x.PreviousServiceSize);
            test.TestedElsewhere(x => x.Priority);
            test.TestedElsewhere(x => x.PurposeofInstallation);
            test.TestedElsewhere(x => x.ServiceMaterial);
            test.TestedElsewhere(x => x.ServiceSideType);
            test.TestedElsewhere(x => x.SizeofMain);
            test.TestedElsewhere(x => x.SizeofService);
            test.TestedElsewhere(x => x.StreetMaterial);
            test.TestedElsewhere(x => x.TypeofMain);
            test.TestedElsewhere(x => x.WorkIssuedTo);
            test.TestedElsewhere(x => x.TownSection);

            // this is set by mapcall internally
            test.NotMapped(x => x.CoordinateID);
            // this did not seem to have a matching field
            test.NotMapped(x => x.Contacted);
            // this is a logical combination of feet and inches
            test.NotMapped(x => x.DepthMain);
            // this is a logical field
            test.NotMapped(x => x.Description);
            // this is a formula field
            test.NotMapped(x => x.HasTapImages);
            test.NotMapped(x => x.Id);
            // this is a formula field
            test.NotMapped(x => x.Installed);
            // this is a formula field
            test.NotMapped(x => x.Invoiced);
            // this is a formula field
            test.NotMapped(x => x.IsActive);
            // this is a formula field
            test.NotMapped(x => x.IssuedToField);
            // this is a logical field
            test.NotMapped(x => x.JustStreetAddress);
            // this is a formula field
            test.NotMapped(x => x.Month);
            // this is a logical field
            test.NotMapped(x => x.StatusMessage);
            // this is a logical field
            test.NotMapped(x => x.TotalFee);
            // this is a formula field
            test.NotMapped(x => x.Year);
            // this is a formula field
            test.NotMapped(x => x.YearRetired);
            // this is set automatically
            test.NotMapped(x => x.Initiator);
            // not provided in sample
            test.NotMapped(x => x.RenewalOf);
            // this is a logical field
            test.NotMapped(x => x.ServiceState);
            // not provided in sample
            test.NotMapped(x => x.ServiceType);
        }

        #region OperatingCenter

        [TestMethod]
        public void TestOperatingCenterMapsFromString()
        {
            WithUnitOfWork(uow => {
                Assert.IsTrue(_target.OperatingCenter.StartsWith(_target
                                                                .MapToEntity(uow, 1, MappingHelper).OperatingCenter
                                                                .OperatingCenterCode));
            });
        }

        [TestMethod]
        public void TestThrowsWhenOperatingCenterCannotBeFoundInDatabase()
        {
            _target.OperatingCenter = "MI666 - this is not a real operating center";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenOperatingCenterCannotBeParsed()
        {
            foreach (var value in new[] { "blah", "blah - blah" })
            {
                _target.OperatingCenter = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        [TestMethod]
        public void TestThrowsWhenOperatingCenterNotProvided()
        {
            foreach (var value in new[] { null, " ", string.Empty })
            {
                _target.OperatingCenter = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        [TestMethod]
        public void TestDoesNotChokeOnOperatingCenterCodeWithoutNumbers()
        {
            WithUnitOfWork(uow => {
                var frequencyUnit = new RecurringFrequencyUnit {
                    Id = RecurringFrequencyUnit.Indices.YEAR
                };
                var oc = uow.Insert(new OperatingCenter {
                    OperatingCenterCode = "ILIU",
                    OperatingCenterName = "Belleville/East St Louis",
                    HydrantInspectionFrequencyUnit = frequencyUnit,
                    LargeValveInspectionFrequencyUnit = frequencyUnit,
                    SmallValveInspectionFrequencyUnit = frequencyUnit,
                    SewerOpeningInspectionFrequencyUnit = frequencyUnit,
                    State = new State { Id = NJState.ID }
                });
                uow.Insert(new OperatingCenterTown {
                    Town = new Town {Id = AberdeenMonmouthNJTown.ID},
                    OperatingCenter = new OperatingCenter {Id = oc.Id}
                });

                _target.OperatingCenter = $"{oc.OperatingCenterCode} - {oc.OperatingCenterName}";

                Assert.AreEqual(oc.Id, _target.MapToEntity(uow, 1, MappingHelper).OperatingCenter.Id);
            });
        }

        #endregion

        #region CategoryofService

        [TestMethod]
        public void TestCategoryOfServiceMapsToServiceCategory()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(22, _target.MapToEntity(uow, 1, MappingHelper).ServiceCategory.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenCategoryOfServiceCannotBeFoundInDatabase()
        {
            _target.CategoryofService = "blah blah blah this doesn't real";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region State

        [TestMethod]
        public void TestStateMapsFromString()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(NJState.ID, _target.MapToEntity(uow, 1, MappingHelper).State.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenStateCannotBeFoundInDatabase()
        {
            _target.State = "NOT A REAL STATE";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region Street

        [TestMethod]
        public void TestStreetMapsFromString()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(AberdeenMonmouthNJStreets.ChurchStreet.ID, _target.MapToEntity(uow, 1, MappingHelper).Street.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenStreetCannotBeFoundInDatabase()
        {
            _target.Street = "NOT A REAL STREET";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region CrossStreet

        [TestMethod]
        public void TestCrossStreetMapsFromString()
        {
            _target.CrossStreet = AberdeenMonmouthNJStreets.ChurchStreet.NAME;

            WithUnitOfWork(uow => {
                Assert.AreEqual(AberdeenMonmouthNJStreets.ChurchStreet.ID, _target.MapToEntity(uow, 1, MappingHelper).CrossStreet.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenCrossStreetCannotBeFoundInDatabase()
        {
            _target.CrossStreet = "NOT A REAL STREET";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region Town

        [TestMethod]
        public void TestTownMapsFromString()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(AberdeenMonmouthNJTown.ID, _target.MapToEntity(uow, 1, MappingHelper).Town.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenTownCannotBeFoundInDatabase()
        {
            _target.Town = "NOT A REAL TOWN";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region CustomerSideMaterial

        [TestMethod]
        public void TestCustomerSideMaterialMapsFromString()
        {
            _target.CustomerSideMaterial = "Unknown";

            WithUnitOfWork(uow => {
                Assert.AreEqual(11, _target.MapToEntity(uow, 1, MappingHelper).CustomerSideMaterial.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenCustomerSideMaterialNotFound()
        {
            _target.CustomerSideMaterial = "IS NOT A VALID MATERIAL";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region CustomerSideReplacementWBSNumber

        [TestMethod]
        public void TestCustomerSideReplacementWBSNumberMapsFromString()
        {
            _target.CustomerSideReplacementWBSNumber = "B18-02-0001";

            WithUnitOfWork(uow => {
                Assert.AreEqual(1, _target.MapToEntity(uow, 1, MappingHelper).CustomerSideReplacementWBSNumber.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenCustomerSideReplacementWBSNumberNotFound()
        {
            _target.CustomerSideReplacementWBSNumber = "NOT A REAL WBS NUMBER";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region CustomerSideSLReplacedBy and friends

        [TestMethod]
        public void TestCustomerSideSLReplacedByMapsFromString()
        {
            _target.CustomerSideSLReplacedBy = "Contractor";
            _target.CustomerSideSLReplacementContractor = "Dave";
            _target.ServiceMaterial = "Unknown";

            WithUnitOfWork(uow => {
                Assert.AreEqual(2, _target.MapToEntity(uow, 1, MappingHelper).CustomerSideSLReplacedBy.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenCustomerSideSLReplaceByNotFound()
        {
            _target.CustomerSideSLReplacedBy = "NOT A REAL CONTRACTOR";
            _target.CustomerSideSLReplacementContractor = "Dave";
            _target.ServiceMaterial = "Unknown";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestCustomerSideSLReplacementContractorMapsFromString()
        {
            _target.CustomerSideSLReplacementContractor = "Dave";

            WithUnitOfWork(uow => {
                Assert.AreEqual(1, _target.MapToEntity(uow, 1, MappingHelper).CustomerSideSLReplacementContractor.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenCustomerSideSLReplacementContractor()
        {
            _target.CustomerSideSLReplacementContractor = "Dave ain't here man";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestServiceMaterialMapsFromString()
        {
            _target.ServiceMaterial = "Unknown";

            WithUnitOfWork(uow => {
                Assert.AreEqual(11, _target.MapToEntity(uow, 1, MappingHelper).ServiceMaterial.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenServiceMaterialNotFound()
        {
            _target.ServiceMaterial = "NOT A REAL MATERIAL";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region CustomerSideSLReplacement

        [TestMethod]
        public void TestCustomerSideSLReplacementMapsFromString()
        {
            _target.CustomerSideSLReplacement = "Offered-Rejected";

            WithUnitOfWork(uow => {
                Assert.AreEqual(3, _target.MapToEntity(uow, 1, MappingHelper).CustomerSideSLReplacement.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenCustomerSideSLReplacementNotFound()
        {
            _target.CustomerSideSLReplacement = "NOT A REAL REPLACEMENT";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region SizeOfMain/MainSize

        [TestMethod]
        public void TestSizeOfMainMapsToMainSize()
        {
            _target.SizeofMain = "1";

            WithUnitOfWork(uow => {
                Assert.AreEqual(7, _target.MapToEntity(uow, 1, MappingHelper).MainSize.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenSizeOfMainNotFound()
        {
            _target.SizeofMain = "NOT A REAL SIZE";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region MeterSettingSize

        [TestMethod]
        public void TestMeterSettingSizeMapsFromString()
        {
            _target.MeterSettingSize = "1";

            WithUnitOfWork(uow => {
                Assert.AreEqual(7, _target.MapToEntity(uow, 1, MappingHelper).MeterSettingSize.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenMeterSettingSizeNotFound()
        {
            _target.SizeofMain = "NOT A REAL SIZE";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region PreviousServiceMaterial

        [TestMethod]
        public void TestPreviousServiceMaterialMapsFromString()
        {
            _target.PreviousServiceMaterial = "Unknown";

            WithUnitOfWork(uow => {
                Assert.AreEqual(11, _target.MapToEntity(uow, 1, MappingHelper).PreviousServiceMaterial.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenPreviousServiceMaterialNotFound()
        {
            _target.PreviousServiceMaterial = "NOT A REAL MATERIAL";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region PreviousServiceSize

        [TestMethod]
        public void TestPreviousServiceSizeMapsFromString()
        {
            _target.PreviousServiceSize = "1";

            WithUnitOfWork(uow => {
                Assert.AreEqual(7, _target.MapToEntity(uow, 1, MappingHelper).PreviousServiceSize.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenPreviousServiceSizeNotFound()
        {
            _target.PreviousServiceSize = "NOT A REAL SIZE";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region PurposeofInstallation

        [TestMethod]
        public void TestPurposeOfInstallationMapsToServiceInstallationPurpose()
        {
            _target.PurposeofInstallation = "Retirement Only";

            WithUnitOfWork(uow => {
                Assert.AreEqual(7, _target.MapToEntity(uow, 1, MappingHelper).ServiceInstallationPurpose.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenPurposeOfInstallationNotFound()
        {
            _target.PurposeofInstallation = "NOT A REAL PURPOSE";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region Priority

        [TestMethod]
        public void TestPriorityMapsToServicePriority()
        {
            _target.Priority = "Routine";

            WithUnitOfWork(uow => {
                Assert.AreEqual(3, _target.MapToEntity(uow, 1, MappingHelper).ServicePriority.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenPriorityNotFound()
        {
            _target.Priority = "NOT A REAL PRIORITY";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region SizeofService

        [TestMethod]
        public void TestSizeOfServiceMapsToServiceSize()
        {
            _target.SizeofService = "1";

            WithUnitOfWork(uow => {
                Assert.AreEqual(7, _target.MapToEntity(uow, 1, MappingHelper).ServiceSize.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenSizeOfServiceNotFound()
        {
            _target.SizeofService = "NOT A REAL SIZE";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region WorkIssuedTo

        [TestMethod]
        public void TestWorkIssuedToMapsFromStringAndOperatingCenter()
        {
            _target.WorkIssuedTo = "Dave";

            WithUnitOfWork(uow => {
                Assert.AreEqual(1, _target.MapToEntity(uow, 1, MappingHelper).WorkIssuedTo.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenWorkIssuedToNotFound()
        {
            _target.WorkIssuedTo = "NOT A REAL WORK ISSUED TOODIAN";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region CustomerSideSize

        [TestMethod]
        public void TestCustomerSideSizeIsMappedFromString()
        {
            _target.CustomerSideSize = "1";

            WithUnitOfWork(uow => {
                Assert.AreEqual(7, _target.MapToEntity(uow, 1, MappingHelper).CustomerSideSize.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenCustomerSideSizeNotFound()
        {
            _target.CustomerSideSize = "NOT A REAL SIZE";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region PermitReceivedDate

        [TestMethod]
        public void TestPermitSentDateMustBeSetWhenPermitReceivedDate()
        {
            _target.PermitReceivedDate = _now;
            _target.PermitSentDate = null;

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });

            _target.PermitSentDate = _now;

            WithUnitOfWork(uow => {
                ExpectNoMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region DateInstalled/RetiredDate

        [TestMethod]
        public void TestInstalledDateMustbeSetWhenRetiredDateIsSet()
        {
            _target.RetiredDate = _now;
            _target.InstalledDate = null;
            _target.ServiceMaterial = "Unknown";
            _target.DateIssuedToField = DateTime.Now;
            _target.LengthOfService = 1;
            _target.OriginalInstallationDate = DateTime.Now;
            _target.WBS = "B18-02-0001";
            _target.Zip = "SHHH!";
            _target.CustomerSideMaterial = "Unknown";
            _target.CustomerSideSize = "1";
            _target.CrossStreet = AberdeenMonmouthNJStreets.ChurchStreet.NAME;
            _target.SizeofMain = "1";
            _target.MeterSettingSize = "1";
            _target.PreviousServiceMaterial = "Unknown";
            _target.PreviousServiceSize = "1";
            _target.PurposeofInstallation = "Retirement Only";
            _target.Priority = "Routine";
            _target.SizeofService = "1";
            _target.WorkIssuedTo = "Dave";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });

            _target.InstalledDate = _now;

            WithUnitOfWork(uow => {
                ExpectNoMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region FlushingOfCustomerPlumbing

        [TestMethod]
        public void TestFlushingOfCustomerPlumbingMapsFromString()
        {
            _target.FlushingofCustomerPlumbing = "Extended Flushing Instructions";

            WithUnitOfWork(uow => {
                Assert.AreEqual(2, _target.MapToEntity(uow, 1, MappingHelper).FlushingOfCustomerPlumbing.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenFlushingOfCustomerPlumbingNotFound()
        {
            _target.FlushingofCustomerPlumbing = "NOT A REAL WHATEVER";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region PermitType

        [TestMethod]
        public void TestPermitTypeMapsFromString()
        {
            _target.PermitType = "County";

            WithUnitOfWork(uow => {
                Assert.AreEqual(1, _target.MapToEntity(uow, 1, MappingHelper).PermitType.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenPermitTypeNotFound()
        {
            _target.PermitType = "NOT A REAL PERMIT TYPE";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region ServiceSideType

        [TestMethod]
        public void TestServiceSideTypeMapsFromString()
        {
            _target.ServiceSideType = "Long Side";

            WithUnitOfWork(uow => {
                Assert.AreEqual(2, _target.MapToEntity(uow, 1, MappingHelper).ServiceSideType.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenServiceSideTypeNotFound()
        {
            _target.ServiceSideType = "NOT A REAL SERVICE SIDE TYPE";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region StreetMaterial

        [TestMethod]
        public void TestStreetMaterialMapsFromString()
        {
            _target.StreetMaterial = "Black Top";

            WithUnitOfWork(uow => {
                Assert.AreEqual(1, _target.MapToEntity(uow, 1, MappingHelper).StreetMaterial.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenStreetMaterialNotFound()
        {
            _target.StreetMaterial = "NOT A REAL STREET MATERIAL";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region TypeofMain

        [TestMethod]
        public void TestTypeofMainMapsToMainType()
        {
            _target.TypeofMain = "Plastic";

            WithUnitOfWork(uow => {
                Assert.AreEqual(12, _target.MapToEntity(uow, 1, MappingHelper).MainType.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenTypeofMainNotFound()
        {
            _target.TypeofMain = "NOT A REAL STREET MATERIAL";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region TownSection

        [TestMethod]
        public void TestTownSectionMapsFromString()
        {
            var factory = GetEntityFactory<TownSection>();
            var townSection = factory.Create(new {
                Town = new Town {Id = AberdeenMonmouthNJTown.ID}
            });

            _target.TownSection = townSection.Name;

            WithUnitOfWork(uow => {
                Assert.AreEqual(townSection.Id, _target.MapToEntity(uow, 1, MappingHelper).TownSection.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenTownSectionNotFoundWithinTown()
        {
            _target.TownSection = "NOT A REAL TOWN SECTION";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IServiceRepository>().Use<ServiceRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateStuffForServicesInAberdeenNJ(_container);
        }

        #endregion
    }
}