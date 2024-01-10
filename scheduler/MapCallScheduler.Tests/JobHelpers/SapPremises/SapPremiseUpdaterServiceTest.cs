using System;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallScheduler.JobHelpers.SapPremise;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using StructureMap;
using ObjectExtensions = MMSINC.ClassExtensions.ObjectExtensions.ObjectExtensions;

namespace MapCallScheduler.Tests.JobHelpers.SapPremises
{
    [TestClass]
    public class SapPremiseUpdaterServiceTest : SapEntityUpdaterServiceTestBase<SapPremiseFileRecord, ISapPremiseFileParser, Premise, IRepository<Premise>, SapPremiseUpdaterService>
    {
        private State _nj;

        [TestInitialize]
        public void TestInitialize()
        {
            _nj = GetEntityFactory<State>().Create(new {Abbreviation = "NJ"});
        }

        #region Private Methods

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);

            i.For<ITownRepository>().Use<TownRepository>();
            i.For<IMeterReadingRouteReadingDateRepository>().Use<MeterReadingRouteReadingDateRepository>();
            i.For<IDateTimeProvider>().Use<DateTimeProvider>();
        }

        protected override FileData SetupFileAndRecords(params SapPremiseFileRecord[] args)
        {
            var townRepo = _container.GetInstance<IRepository<Town>>();
            var stateRepo = _container.GetInstance<IRepository<State>>();
            foreach (var record in args)
            {
                if (string.IsNullOrWhiteSpace(record.RegionCode))
                {
                    record.RegionCode = "123";
                }

                if (string.IsNullOrWhiteSpace(record.ServiceState))
                {
                    record.ServiceState = _nj.Abbreviation;
                }

                if (string.IsNullOrWhiteSpace(record.PremiseNumber))
                {
                    record.PremiseNumber = "1234";
                }
            }

            foreach (var townThing in args
                                     .Select(a => new {DistrictId = a.RegionCode, State = a.ServiceState})
                                     .Where(t => !townRepo
                                                 .Where(t2 =>
                                                      t2.State.Abbreviation == t.State && t2.DistrictId == t.DistrictId)
                                                 .Any()).Distinct())
            {
                var newTown = new Town {
                    DistrictId = townThing.DistrictId,
                    State = stateRepo.Where(s => s.Abbreviation == townThing.State).Select(s => new State {Id = s.Id})
                                     .Single()
                };
                townRepo.Save(newTown);
            }

            return base.SetupFileAndRecords(args);
        }

        protected void TestCreatesChildObject<TChild>(SapPremiseFileRecord fileRecord, Expression<Func<Premise, TChild>> getChild, Action<TChild> extraAssertions = null)
        {
            var file = SetupFileAndRecords(fileRecord);
            var childRepo = _container.GetInstance<IRepository<TChild>>();

            MyAssert.CausesIncrease(() => _target.Process(file),
                () => childRepo.GetAll().Count());

            Session.Flush();

            var entity = Repository.GetAll().ToList().Last();
            var latestChild = childRepo.GetAll().ToList().Last();

            Assert.AreEqual(latestChild, getChild.Compile().Invoke(entity));

            extraAssertions?.Invoke(latestChild);
        }

        protected void TestDoesNotCreateChildObjectIfNoValueSent<TChild>(SapPremiseFileRecord fileRecord, Expression<Func<Premise, TChild>> getChild)
        {
            var file = SetupFileAndRecords(fileRecord);

            MyAssert.DoesNotCauseIncrease(() => _target.Process(file),
                () => _container.GetInstance<IRepository<TChild>>().GetAll().Count());

            var entity = Repository.GetAll().ToList().Last();

            Assert.IsNull(getChild.Compile().Invoke(entity));
        }

        protected void TestDoesNotCreateChildObjectIfNoValueSent<TChild>(
            Expression<Func<Premise, TChild>> getChild)
        {
            TestDoesNotCreateChildObjectIfNoValueSent(new SapPremiseFileRecord(), getChild);
        }

        protected void TestUsesExistingChildObject<TChild>(
            SapPremiseFileRecord fileRecord,
            object childData,
            Expression<Func<Premise, TChild>> getChild)
            where TChild : class, new()
        {
            var file = SetupFileAndRecords(fileRecord);
            var childRepo = _container.GetInstance<IRepository<TChild>>();
            if (childData != null)
            {
                GetEntityFactory<TChild>().Create(childData);
            }

            MyAssert.DoesNotCauseIncrease(() => _target.Process(file),
                () => childRepo.GetAll().Count());

            var entity = Repository.GetAll().ToList().Last();
            var expected = childRepo.GetAll().ToList().Last();
            var actual = getChild.Compile().Invoke(entity);

            Assert.AreEqual(expected.GetType(), actual.GetType());
            Assert.AreEqual(ObjectExtensions.GetPropertyValueByName(expected, "Id"),
                ObjectExtensions.GetPropertyValueByName(actual, "Id"));
        }

        #endregion

        #region District

        [TestMethod]
        public void TestProcessCreatesDistrictIfItDoesNotExist()
        {
            var sapCode = "123";
            var district = "district";

            TestCreatesChildObject(new SapPremiseFileRecord {
                ServiceDistrict = sapCode,
                ServiceDistrictDescription = district
            }, p => p.ServiceDistrict);
        }

        [TestMethod]
        public void TestProcessDoesNotSetDistrictIfNoValueSent()
        {
            TestDoesNotCreateChildObjectIfNoValueSent<PremiseDistrict>(p => p.ServiceDistrict);
        }

        [TestMethod]
        public void TestProcessUsesExistingDistrictIfItExists()
        {
            var sapCode = "123";
            var district = "district";

            TestUsesExistingChildObject(new SapPremiseFileRecord {
                    ServiceDistrict = sapCode,
                    ServiceDistrictDescription = district
                }, new {SAPCode = sapCode, Description = district},
                p => p.ServiceDistrict);
        }

        #endregion

        #region Route

        [TestMethod]
        public void TestProcessCreatesRouteIfItDoesNotExist()
        {
            var sapCode = "123";
            var routeNumber = "routeNumber";

            TestCreatesChildObject<MeterReadingRoute>(new SapPremiseFileRecord {
                RouteNumber = sapCode,
                RouteNumberDescription = routeNumber
            }, p => p.RouteNumber);
        }

        [TestMethod]
        public void TestProcessDoesNotSetRouteIfNoValueSent()
        {
            TestDoesNotCreateChildObjectIfNoValueSent<MeterReadingRoute>(p => p.RouteNumber);
        }

        [TestMethod]
        public void TestProcessUsesExistingRouteIfItExists()
        {
            var sapCode = "123";
            var routeNumber = "routeNumber";

            TestUsesExistingChildObject<MeterReadingRoute>(new SapPremiseFileRecord {
                    RouteNumber = sapCode,
                    RouteNumberDescription = routeNumber
                }, new {SAPCode = sapCode, Description = routeNumber},
                p => p.RouteNumber);
        }

        #endregion

        #region NextMeterReadingDate

        [TestMethod]
        public void TestProcessCreatesMeterReadingRoutesReadingDatesForRecordsWhichDoNotYetExist()
        {
            var sapCode = "123";
            var routeNumber = "routeNumber";
            var premiseNumber = "premiseNumber";
            var route = GetEntityFactory<MeterReadingRoute>().Create(new {Id = 1, SAPCode = sapCode, Description = routeNumber});
            var nextMeterReadingDate = "01/01/1970,01/02/1970,01/03/1970";
            var existingDate = GetEntityFactory<MeterReadingRouteReadingDate>().Create(new {
                Id = 1,
                MeterReadingRoute = route,
                ReadingDate = new DateTime(1970, 1, 1)
            });

            var file = SetupFileAndRecords(new SapPremiseFileRecord {
                PremiseNumber = premiseNumber,
                RouteNumber = sapCode,
                RouteNumberDescription = routeNumber,
                NextMeterReadingdate = nextMeterReadingDate
            });
            
            MyAssert.CausesIncrease(() => _target.Process(file),
                () => _container.GetInstance<IRepository<MeterReadingRouteReadingDate>>().GetAll().Count(), 2);
        }

        #endregion

        #region AreaCode

        [TestMethod]
        public void TestProcessCreatesAreaCodeIfItDoesNotExist()
        {
            var sapCode = "123";
            var areaCode = "areaCode";

            TestCreatesChildObject<PremiseAreaCode>(new SapPremiseFileRecord {
                AreaCode = sapCode,
                AreaCodeDescription = areaCode
            }, p => p.AreaCode);
        }

        [TestMethod]
        public void TestProcessDoesNotSetAreaCodeIfNoValueSent()
        {
            TestDoesNotCreateChildObjectIfNoValueSent<PremiseAreaCode>(p => p.AreaCode);
        }

        [TestMethod]
        public void TestProcessUsesExistingAreaCodeIfItExists()
        {
            var sapCode = "123";
            var areaCode = "areaCode";

            TestUsesExistingChildObject<PremiseAreaCode>(new SapPremiseFileRecord {
                AreaCode = sapCode,
                AreaCodeDescription = areaCode
            }, new {SAPCode = sapCode, Description = areaCode},
                p => p.AreaCode);
        }

        #endregion

        #region RegionCode

        [TestMethod]
        public void TestProcessCreatesRegionCodeIfItDoesNotExist()
        {
            var sapCode = "123";
            var regionCode = "regionCode";
            var operatingCentre = "operatingCentre";
            var operatingCenter = GetEntityFactory<OperatingCenter>()
               .Create(new {OperatingCenterCode = "abc", State = _nj});
            var planningPlant = GetEntityFactory<PlanningPlant>()
               .Create(new {OperatingCenter = operatingCenter, Code = operatingCentre});

            TestCreatesChildObject<RegionCode>(new SapPremiseFileRecord {
                RegionCode = sapCode,
                RegionCodeDescription = regionCode,
                OperatingCentre = operatingCentre
            }, p => p.RegionCode);
        }

        [TestMethod]
        public void TestProcessUsesExistingRegionCodeIfItExists()
        {
            var sapCode = "123";
            var regionCode = "regionCode";
            var operatingCentre = "operatingCentre";
            var operatingCenter = GetEntityFactory<OperatingCenter>()
               .Create(new {OperatingCenterCode = "abc", State = _nj});
            var planningPlant = GetEntityFactory<PlanningPlant>()
               .Create(new {OperatingCenter = operatingCenter, Code = operatingCentre});

            TestUsesExistingChildObject<RegionCode>(new SapPremiseFileRecord {
                    RegionCode = sapCode,
                    RegionCodeDescription = regionCode,
                    OperatingCentre = operatingCentre
                }, new {SAPCode = sapCode, Description = regionCode, State = _nj},
                p => p.RegionCode);
        }

        #endregion

        #region Town

        [TestMethod]
        public void TestProcessSetsTownByRegionCodeAndState()
        {
            var state = GetEntityFactory<State>().Create(new {Abbreviation = "PA"});

            TestUsesExistingChildObject(new SapPremiseFileRecord {
                    RegionCode = "345",
                    ServiceState = state.Abbreviation
                }, null /* town gets created automatically */,
                p => p.ServiceCity);
        }

        #endregion

        #region OperatingCentre

        [TestMethod]
        public void TestProcessSetsPlanningPlantFromOperatingCentre()
        {
            var operatingCentre = "operatingCentre";
            var operatingCenter = GetEntityFactory<OperatingCenter>()
               .Create(new {OperatingCenterCode = "abc", State = _nj});
            var planningPlant = GetEntityFactory<PlanningPlant>()
               .Create(new {OperatingCenter = operatingCenter, Code = operatingCentre});

            var file = SetupFileAndRecords(new SapPremiseFileRecord {
                OperatingCentre = operatingCentre
            });

            _target.Process(file);

            var premise = Repository.GetAll().OrderBy(p => p.Id).ToList().Last();

            Assert.AreEqual(planningPlant.Id, premise.PlanningPlant.Id);
        }

        [TestMethod]
        public void TestProcessSetsOperatingCenterFromOperatingCentreViaPlanningPlant()
        {
            var operatingCentre = "operatingCentre";
            var operatingCenter = GetEntityFactory<OperatingCenter>()
               .Create(new {OperatingCenterCode = "abc", State = _nj});
            var planningPlant = GetEntityFactory<PlanningPlant>()
               .Create(new {OperatingCenter = operatingCenter, Code = operatingCentre});

            var file = SetupFileAndRecords(new SapPremiseFileRecord {
                OperatingCentre = operatingCentre
            });

            _target.Process(file);

            var premise = Repository.GetAll().OrderBy(p => p.Id).ToList().Last();

            Assert.AreEqual(operatingCenter.Id, premise.OperatingCenter.Id);
        }

        #endregion

        #region ServiceUtilityType

        [TestMethod]
        public void TestProcessSetsServiceUtilityTypeIfSent()
        {
            var serviceUtilityType = "serviceUtilitType";

            TestUsesExistingChildObject(new SapPremiseFileRecord {
                    ServiceUtilityType = serviceUtilityType
                }, new {Type = serviceUtilityType},
                p => p.ServiceUtilityType);
        }

        #endregion

        #region StatusCode

        [TestMethod]
        public void TestProcessSetsStatusCodeIfSent()
        {
            GetFactory<ActivePremiseStatusCodeFactory>().Create();

            TestUsesExistingChildObject(
                new SapPremiseFileRecord {
                    StatusCode = "Active"
                },
                null,
                p => p.StatusCode);
        }

        [TestMethod]
        public void TestProcessSetsCriticalCareTypeIfSent()
        {
            TestUsesExistingChildObject(
                new SapPremiseFileRecord { CriticalCareType = "Hospital" },
                new { Description = "Hospital"},
                p => p.CriticalCareType);
        }

        [TestMethod]
        public void TestCriticalCareThrowsWhenTypeNotFound()
        {
            var file = SetupFileAndRecords(new SapPremiseFileRecord { CriticalCareType = "this does not exist" });

            MyAssert.Throws<ArgumentException>(() => _target.Process(file));
        }

        [TestMethod]
        public void TestProcessDoesNotSetStatusCodeIfNotSent()
        {
            TestDoesNotCreateChildObjectIfNoValueSent(new SapPremiseFileRecord(), p => p.StatusCode);
        }

        [TestMethod]
        public void TestProcessThrowsWhenStatusCodeNotFound()
        {
            var file = SetupFileAndRecords(new SapPremiseFileRecord {StatusCode = "this does not exist"});

            MyAssert.Throws<ArgumentException>(() => _target.Process(file));
        }

        #endregion

        #region MeterLocation

        [TestMethod]
        public void TestProcessSetsMeterLocationIfSent()
        {
            TestUsesExistingChildObject(new SapPremiseFileRecord {
                    MeterLocation = "0002"
                }, new {SAPCode = "0002", Description = "OUTSIDE"},
                p => p.MeterLocation);
        }

        [TestMethod]
        public void TestProcessHandlesMeterLocationValuesWithoutLeadingZeroes()
        {
            TestUsesExistingChildObject(new SapPremiseFileRecord {
                    MeterLocation = "1"
                }, new {SAPCode = "0001", Description = "OUTSIDE"},
                p => p.MeterLocation);
        }

        #endregion

        #region MeterSize

        [TestMethod]
        public void TestProcessSetsMeterSizeIfMeterSent()
        {
            var meterSize = "meterSize";

            TestUsesExistingChildObject(new SapPremiseFileRecord {
                    MeterSize = meterSize
                }, new {SAPCode = meterSize},
                p => p.MeterSize);
        }

        [TestMethod]
        public void TestProcessHandlesMeterSizeValuesWithoutLeadingZeroes()
        {
            TestUsesExistingChildObject(new SapPremiseFileRecord {
                    MeterSize = "1"
                }, new {SAPCode = "0001"},
                p => p.MeterSize);
        }

        #endregion

        #region Coordinates

        [TestMethod]
        public void TestProcessSetsCoordinatesFromLatAndLngIfSent()
        {
            var latitude = 12;
            var longitude = 32;

            TestCreatesChildObject(new SapPremiseFileRecord {
                Latitude = latitude.ToString(),
                Longitude = longitude.ToString()
            }, p => p.Coordinate, c => {
                Assert.AreEqual(latitude, c.Latitude);
                Assert.AreEqual(longitude, c.Longitude);
            });
        }

        [TestMethod]
        public void TestProcessDoesNotChokeOnNullAsAStringForLatOrLng()
        {
            TestDoesNotCreateChildObjectIfNoValueSent(new SapPremiseFileRecord {
                Latitude = "NULL",
                Longitude = "NULL"
            }, p => p.Coordinate);
        }

        [TestMethod]
        public void TestProcessIgnoresLatLngThatCannotBeParsed()
        {
            TestDoesNotCreateChildObjectIfNoValueSent(new SapPremiseFileRecord {
                Latitude = "38324641000000",
                Longitude = "38324641000000"
            }, p => p.Coordinate);
        }

        #endregion

        #region Public Water Supply

        [TestMethod]
        public void TestProcess_SetsPublicWaterSupply_IfPWSIDSent()
        {
            var pws = new PublicWaterSupply {
                Id = 1,
                Identifier = "abc123"
            };

            TestUsesExistingChildObject(new SapPremiseFileRecord {
                Pwsid = "abc123"
            }, new {Id = 1, Identifier = "abc123"}, p => p.PublicWaterSupply);
        }

        #endregion

        #region PremiseType

        [TestMethod]
        public void TestProcess_SetsPremiseType_IfPremiseTypeSent()
        {
            var pws = new PremiseType
            {
                Id = 1,
                Abbreviation = "Condo"
            };

            TestUsesExistingChildObject(new SapPremiseFileRecord
            {
                PremiseType = "Condo"
            }, new { Id = 1, Abbreviation = "Condo" }, p => p.PremiseType);
        }

        #endregion

        #region Easy Fields

        [TestMethod]
        public void TestProcessSetsTheEasyFields()
        {
            var premiseNumber = "premiseNumber";
            var connectionObject = "connectionObject";
            var deviceCategory = "deviceCategory";
            var deviceLocation = "deviceLocation";
            var equipment = "equipment";
            var deviceSerialNumber = "serialNumber";
            var serviceAddressHouseNumber = "serviceAddressHouseNumber";
            var serviceAddressFraction = "serviceAddressFraction";
            var serviceAddressApartment = "serviceAddressApartment";
            var serviceAddressStreet = "serviceAddressStreet";
            var serviceZip = "serviceZip";
            var meterLocationFreeText = "meterLocationFreeText";
            var meterSerialNumber = "meterSerialNumber";
            var isMajorAccount = false;
            var majorAccountManager = "majorAccountManager";
            var accountManagerContactNumber = "accountManagerContactNumber";
            var accountManagerEmail = "accountManagerEmail";

            var file = SetupFileAndRecords(new SapPremiseFileRecord {
                PremiseNumber = premiseNumber,
                ConnectionObject = connectionObject,
                DeviceCategory = deviceCategory,
                DeviceLocation = deviceLocation,
                Equipment = equipment,
                DeviceSerialNumber = deviceSerialNumber,
                ServiceAddressHouseNumber = serviceAddressHouseNumber,
                ServiceAddressFraction = serviceAddressFraction,
                ServiceAddressApartment = serviceAddressApartment,
                ServiceAddressStreet = serviceAddressStreet,
                ServiceZip = serviceZip,
                MeterLocationFreeText = meterLocationFreeText,
                MeterSerialNumber = meterSerialNumber,
                IsMajorAccount = isMajorAccount,
                MajorAccountManager = majorAccountManager,
                AccountManagerEmail = accountManagerEmail,
                AccountManagerContactNumber = accountManagerContactNumber
            });

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(premiseNumber, entity.PremiseNumber);
            Assert.AreEqual(connectionObject, entity.ConnectionObject);
            Assert.AreEqual(deviceCategory, entity.DeviceCategory);
            Assert.AreEqual(deviceLocation, entity.DeviceLocation);
            Assert.AreEqual(equipment, entity.Equipment);
            Assert.AreEqual(deviceSerialNumber, entity.DeviceSerialNumber);
            Assert.AreEqual(serviceAddressHouseNumber, entity.ServiceAddressHouseNumber);
            Assert.AreEqual(serviceAddressFraction, entity.ServiceAddressFraction);
            Assert.AreEqual(serviceAddressApartment, entity.ServiceAddressApartment);
            Assert.AreEqual(serviceAddressStreet, entity.ServiceAddressStreet);
            Assert.AreEqual(serviceZip, entity.ServiceZip);
            Assert.AreEqual(meterLocationFreeText, entity.MeterLocationFreeText);
            Assert.AreEqual(meterSerialNumber, entity.MeterSerialNumber);
            Assert.AreEqual(isMajorAccount, entity.IsMajorAccount);
            Assert.AreEqual(majorAccountManager, entity.MajorAccountManager);
            Assert.AreEqual(accountManagerEmail, entity.AccountManagerEmail);
            Assert.AreEqual(accountManagerContactNumber, entity.AccountManagerContactNumber);
        }

        #endregion

        #region Updating

        [TestMethod]
        public void TestProcessUpdatesRatherThanCreatingForExistingPremise()
        {
            var premiseNumber = "PremiseNumber";
            var deviceLocation = "DeviceLocation";
            var connectionObject = "ConnectionObject";
            var installation = "Installation";
            var existing = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = premiseNumber,
                DeviceLocation = deviceLocation,
                ConnectionObject = connectionObject,
                Installation = installation
            });

            var file = SetupFileAndRecords(new SapPremiseFileRecord {
                PremiseNumber = premiseNumber,
                DeviceLocation = deviceLocation,
                ConnectionObject = connectionObject,
                Installation = installation,
                Equipment = "blah"
            });

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(existing.Id, entity.Id);
            Assert.AreEqual("blah", entity.Equipment);
        }

        #endregion

        #region Duplicates

        [TestMethod]
        public void TestProcessDoesNotCreateDuplicates()
        {
            var premiseNumber = "PremiseNumber";
            var deviceLocation = "DeviceLocation";
            var connectionObject = "ConnectionObject";
            var installation = "Installation";
            var existing = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = premiseNumber,
                DeviceLocation = deviceLocation,
                ConnectionObject = connectionObject,
                Installation = installation
            });

            var file = SetupFileAndRecords(new SapPremiseFileRecord {
                PremiseNumber = premiseNumber,
                DeviceLocation = deviceLocation,
                ConnectionObject = connectionObject,
                Installation = installation
            }.Duplicate(SapPremiseUpdaterService.CLEAR_SESSION_INTERVAL).ToArray());

            MyAssert.DoesNotThrow(() => _target.Process(file));

            _target = _container.GetInstance<SapPremiseUpdaterService>();

            MyAssert.DoesNotThrow(() => _target.Process(file));
        }

        #endregion
    }
}
