using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.LIMS.Client;
using MapCall.LIMS.Configuration;
using MapCall.LIMS.Model.Entities;
using MapCallScheduler.JobHelpers.LIMSSynchronization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.LIMSSynchronization
{
    [TestClass]
    public class SampleSiteProfileSyncServiceTest
    {
        #region Private Fields

        private IContainer _container;
        private Mock<IRepository<SampleSiteProfile>> _sampleSiteRepositoryMock;
        private Mock<IRepository<PublicWaterSupply>> _publicWaterSupplyRepositoryMock;
        private Mock<IRepository<SampleSiteProfileAnalysisType>> _sampleSiteProfileAnalysisTypeRepositoryMock;
        private Mock<LIMSApiClient> _limsApiClientMock;

        private SampleSiteProfileSyncService _target;

        private SampleSiteProfileAnalysisType _bacterialAnalysisType;
        private SampleSiteProfileAnalysisType _chemicalAnalysisType;
        private IQueryable<SampleSiteProfileAnalysisType> _sampleSiteProfileAnalysisTypes;

        private PublicWaterSupply _publicWaterSupply01;
        private PublicWaterSupply _publicWaterSupply02;
        private PublicWaterSupply _publicWaterSupply03;
        private PublicWaterSupply _publicWaterSupply09;
        private PublicWaterSupply _publicWaterSupply09A;
        private IQueryable<PublicWaterSupply> _publicWaterSupplies;

        private SampleSiteProfile _sampleSiteProfile1;
        private SampleSiteProfile _sampleSiteProfile2;
        private SampleSiteProfile _sampleSiteProfile3;
        private IQueryable<SampleSiteProfile> _sampleSiteProfiles;
        private IList<SampleSiteProfile> _actualSyncedSampleProfiles;

        #endregion

        #region Test Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            // Set up our container and mocks

            _container = new Container();
            _container.Inject(new Mock<ILog>().Object);
            _container.Inject((new Mock<ILIMSClientConfiguration>()).Object);

            _container.Inject((_sampleSiteRepositoryMock = new Mock<IRepository<SampleSiteProfile>>()).Object);
            _container.Inject((_publicWaterSupplyRepositoryMock = new Mock<IRepository<PublicWaterSupply>>()).Object);
            _container.Inject((_sampleSiteProfileAnalysisTypeRepositoryMock =
                new Mock<IRepository<SampleSiteProfileAnalysisType>>()).Object);

            // Set up all our data

            _bacterialAnalysisType = new SampleSiteProfileAnalysisType {
                Id = SampleSiteProfileAnalysisType.Indices.BACTERIAL,
                Description = "BACT"
            };
            _chemicalAnalysisType = new SampleSiteProfileAnalysisType {
                Id = SampleSiteProfileAnalysisType.Indices.CHEMICAL,
                Description = "CHEM"
            };
            _sampleSiteProfileAnalysisTypes = new List<SampleSiteProfileAnalysisType> {
                _bacterialAnalysisType,
                _chemicalAnalysisType
            }.AsQueryable();

            _publicWaterSupply01 = new PublicWaterSupply { Id = 1, Identifier = "pws-01" };
            _publicWaterSupply02 = new PublicWaterSupply { Id = 2, Identifier = "pws-02" };
            _publicWaterSupply03 = new PublicWaterSupply { Id = 3, Identifier = "pws-03" };
            _publicWaterSupply09 = new PublicWaterSupply { Id = 4, Identifier = "pws-09" };
            _publicWaterSupply09A = new PublicWaterSupply { Id = 5, Identifier = "pws-09" };

            _publicWaterSupplies = new List<PublicWaterSupply> {
                _publicWaterSupply01,
                _publicWaterSupply02,
                _publicWaterSupply03,
                _publicWaterSupply09,
                _publicWaterSupply09A,
            }.AsQueryable();

            _sampleSiteProfile1 = new SampleSiteProfile {
                Id = 1,
                Number = 100,
                Name = "ssp-01",
                PublicWaterSupply = _publicWaterSupply01,
                SampleSiteProfileAnalysisType = _bacterialAnalysisType
            };

            _sampleSiteProfile2 = new SampleSiteProfile {
                Id = 2,
                Number = 200,
                Name = "ssp-02",
                PublicWaterSupply = _publicWaterSupply02,
                SampleSiteProfileAnalysisType = _chemicalAnalysisType
            };

            _sampleSiteProfile3 = new SampleSiteProfile {
                Id = 3,
                Number = 300,
                Name = "ssp-03",
                PublicWaterSupply = _publicWaterSupply03,
                SampleSiteProfileAnalysisType = _bacterialAnalysisType
            };

            _sampleSiteProfiles = new List<SampleSiteProfile> {
                _sampleSiteProfile1,
                _sampleSiteProfile2,
                _sampleSiteProfile3
            }.AsQueryable();

            // Finally set up our data with our mocked repositories

            _publicWaterSupplyRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<PublicWaterSupply, bool>>>()))
                                            .Returns(_publicWaterSupplies);

            _sampleSiteProfileAnalysisTypeRepositoryMock.Setup(x => x.GetAll())
                                                        .Returns(_sampleSiteProfileAnalysisTypes);

            _sampleSiteRepositoryMock.Setup(x => x.GetAll())
                                     .Returns(_sampleSiteProfiles);

            _sampleSiteRepositoryMock.Setup(x => x.Save(It.IsAny<IEnumerable<SampleSiteProfile>>()))
                                     .Callback<IEnumerable<SampleSiteProfile>>(r =>
                                          _actualSyncedSampleProfiles = r.ToList());
        }

        #endregion

        [TestMethod]
        public void TestProcessSyncsTheCorrectNumberOfProfilesAndSavesOnce()
        {
            var httpClientMock = new APIMClientMockFactory<Profile[]>(new[] {
                new Profile { Number = 100, Name = "ssp-01", PublicWaterSupplyIdentifier = "pws-01", AnalysisType = "CHEM" },
                new Profile { Number = 200, Name = "ssp-02", PublicWaterSupplyIdentifier = "pws-02", AnalysisType = "CHEM" },
                new Profile { Number = 900, Name = "ssp-09", PublicWaterSupplyIdentifier = "pws-09", AnalysisType = "BACT" },
                new Profile { Number = 999, Name = "ssp-99", PublicWaterSupplyIdentifier = "not-owned-by-mapcall", AnalysisType = "CHEM" },
                new Profile { Number = -1, Name = "ssp--1", PublicWaterSupplyIdentifier = "pws-XX", AnalysisType = "Fake-Analysis-Type" }
            });

            _limsApiClientMock = new Mock<LIMSApiClient>(
                _container.GetInstance<ILog>(),
                _container.GetInstance<LIMSClientConfiguration>(),
                httpClientMock);

            _container.Inject<ILIMSApiClient>(_limsApiClientMock.Object);

            _target = _container.GetInstance<SampleSiteProfileSyncService>();

            _target.Process();

            // Verify we're only calling save once, and verify the count of expected items to save while we're at it
            _sampleSiteRepositoryMock.Verify(x => x.Save(It.Is<IEnumerable<SampleSiteProfile>>(y => y.Count() == 2)),
                Times.Once,
                $"Save was not called with expected number of sample site profiles (2) - actual: {_actualSyncedSampleProfiles.Count}");
        }

        [TestMethod]
        public void TestProcessFindsAMatchingProfileAndUpdatesIt()
        {
            var httpClientMock = new APIMClientMockFactory<Profile[]>(new[] {
                new Profile {
                    Number = 100, 
                    Name = "ssp-01",
                    PublicWaterSupplyIdentifier = "pws-01", 
                    AnalysisType = "CHEM"
                }
            });

            _limsApiClientMock = new Mock<LIMSApiClient>(
                _container.GetInstance<ILog>(),
                _container.GetInstance<LIMSClientConfiguration>(),
                httpClientMock);

            _container.Inject<ILIMSApiClient>(_limsApiClientMock.Object);

            _target = _container.GetInstance<SampleSiteProfileSyncService>();

            _target.Process();

            // Case 1: One profile (100) that should match profile number and pwsid and be an "Update" (bacterial to chemical)
            Assert.AreEqual(_sampleSiteProfile1, _actualSyncedSampleProfiles[0]);
        }

        [TestMethod]
        public void TestProcessFindsAMatchAndIgnoresItBecausePropertyValuesAreTheSame()
        {
            var httpClientMock = new APIMClientMockFactory<Profile[]>(new[] {
                new Profile {
                    Number = 200, 
                    Name = "ssp-02",
                    PublicWaterSupplyIdentifier = "pws-02", 
                    AnalysisType = "CHEM"
                }
            });

            _limsApiClientMock = new Mock<LIMSApiClient>(
                _container.GetInstance<ILog>(),
                _container.GetInstance<LIMSClientConfiguration>(),
                httpClientMock);

            _container.Inject<ILIMSApiClient>(_limsApiClientMock.Object);

            _target = _container.GetInstance<SampleSiteProfileSyncService>();

            _target.Process();

            // Case 2: One profile (200) that should match on all, but is ignored because it has the same values for everything
            _sampleSiteRepositoryMock.Verify(x => x.Save(It.Is<IEnumerable<SampleSiteProfile>>(y => !y.Any())),
                Times.Once,
                $"Save was not called with expected number of sample site profiles (0) - actual: {_actualSyncedSampleProfiles.Count}");
        }

        [TestMethod]
        public void TestProcessAddsAProfileThatDoesNotExist()
        {
            var httpClientMock = new APIMClientMockFactory<Profile[]>(new[] {
                new Profile {
                    Number = 900, 
                    Name = "ssp-09",
                    PublicWaterSupplyIdentifier = "pws-09", 
                    AnalysisType = "BACT"
                }
            });

            _limsApiClientMock = new Mock<LIMSApiClient>(
                _container.GetInstance<ILog>(),
                _container.GetInstance<LIMSClientConfiguration>(),
                httpClientMock);

            _container.Inject<ILIMSApiClient>(_limsApiClientMock.Object);

            _target = _container.GetInstance<SampleSiteProfileSyncService>();

            _target.Process();

            // Case 3. One profile (900) that should not match profile number but will match pwsid and should be an "Add"
            Assert.AreEqual(0, _actualSyncedSampleProfiles[0].Id, "Expected id of sample site profile to add does not match.");
            Assert.AreEqual(900, _actualSyncedSampleProfiles[0].Number, "Expected number of sample site profile to add does not match.");
            Assert.AreEqual("ssp-09", _actualSyncedSampleProfiles[0].Name, "Expected name of sample site profile to add does not match.");
            Assert.AreEqual("pws-09", _actualSyncedSampleProfiles[0].PublicWaterSupply.Identifier, "Expected pwsid of sample site profile to add does not match.");
            Assert.AreEqual(_bacterialAnalysisType.Id, _actualSyncedSampleProfiles[0].SampleSiteProfileAnalysisType.Id, "Expected analysis type of sample site profile to add does not match.");
        }

        [TestMethod]
        public void TestProcessDoesNotFindAMatchingProfileNumberOrPublicWaterSupplyAndIgnoresIt()
        {
            var httpClientMock = new APIMClientMockFactory<Profile[]>(new[] {
                new Profile {
                    Number = 999, 
                    Name = "ssp-99",
                    PublicWaterSupplyIdentifier = "not-owned-by-mapcall", 
                    AnalysisType = "CHEM"
                }
            });

            _limsApiClientMock = new Mock<LIMSApiClient>(
                _container.GetInstance<ILog>(),
                _container.GetInstance<LIMSClientConfiguration>(),
                httpClientMock);

            _container.Inject<ILIMSApiClient>(_limsApiClientMock.Object);

            _target = _container.GetInstance<SampleSiteProfileSyncService>();

            _target.Process();

            // Case 4. One profile (999) that should not match profile number or pwsid and will be "Ignored" - this is the use case that MapCall does not own all profiles
            _sampleSiteRepositoryMock.Verify(x => x.Save(It.Is<IEnumerable<SampleSiteProfile>>(y => !y.Any())),
                Times.Once,
                $"Save was not called with expected number of sample site profiles (0) - actual: {_actualSyncedSampleProfiles.Count}");
        }
    }
}
