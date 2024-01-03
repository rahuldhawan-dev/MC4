using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.SampleSiteDeactivation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SampleSiteDeactivation
{
    [TestClass]
    public class SampleSiteDeactivationServiceTest
    {
        #region Private Fields

        private IContainer _container;
        private Mock<IDateTimeProvider> _dateTimeProviderMock;
        private Mock<IRepository<SampleSite>> _sampleSiteRepositoryMock;
        private Mock<IRepository<MostRecentlyInstalledService>> _recentlyInstalledServiceRepositoryMock;
        private SampleSiteDeactivationProcessorService _target;

        #endregion

        #region Test Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            // Set up our container and mocks
            _container = new Container();
            _container.Inject(new Mock<ILog>().Object);

            _container.Inject((_sampleSiteRepositoryMock = new Mock<IRepository<SampleSite>>()).Object);
            _container.Inject((_recentlyInstalledServiceRepositoryMock =
                new Mock<IRepository<MostRecentlyInstalledService>>()).Object);
            _container.Inject((_dateTimeProviderMock = new Mock<IDateTimeProvider>()).Object);
            _dateTimeProviderMock.Setup(s => s.GetCurrentDate()).Returns(DateTime.Now);
        }

        private static IQueryable<MostRecentlyInstalledService> GetSampleRecentlyInstalledServices(bool isActive,
            bool isLeadCopperSite)
        {
            var premise1 = new Premise {
                SampleSites = new List<SampleSite>() {
                    new SampleSite() {
                        LeadCopperSite = isLeadCopperSite,
                        Status = new SampleSiteStatus() {
                            Id = isActive ? SampleSiteStatus.Indices.ACTIVE : SampleSiteStatus.Indices.INACTIVE
                        }
                    }
                }
            };

            var premise2 = new Premise {
                SampleSites = new List<SampleSite>() {
                    new SampleSite() {
                        LeadCopperSite = isLeadCopperSite,
                        Status = new SampleSiteStatus() {
                            Id = isActive ? SampleSiteStatus.Indices.ACTIVE : SampleSiteStatus.Indices.INACTIVE
                        }
                    }
                }
            };

            var recentlyUpdatedServices = new List<MostRecentlyInstalledService> {
                new MostRecentlyInstalledService() { Premise = premise1 },
                new MostRecentlyInstalledService() { Premise = premise2 },
            }.AsQueryable();

            return recentlyUpdatedServices;
        }

        #endregion

        [TestMethod]
        public void TestSampleSitesWithStatusAsInactiveAreNotUpdated()
        {
            // add all sample site as inactive
            var recentlyUpdatedServices = GetSampleRecentlyInstalledServices(isActive: false, isLeadCopperSite: true);

            _recentlyInstalledServiceRepositoryMock
               .Setup(x => x.Where(It.IsAny<Expression<Func<MostRecentlyInstalledService, bool>>>()))
               .Returns(recentlyUpdatedServices);

            // act
            _target = _container.GetInstance<SampleSiteDeactivationProcessorService>();
            _target.Process();

            // verify save hasn't be called because all sample sites were inactive
            _sampleSiteRepositoryMock.Verify(x => x.Save(It.IsAny<SampleSite>()), Times.Never);
        }

        [TestMethod]
        public void TestSampleSitesNotAsLeadCopperSiteAreNotUpdated()
        {
            // add all sample site as not a leadCopperSite 
            var recentlyUpdatedServices = GetSampleRecentlyInstalledServices(isActive: true, isLeadCopperSite: false);

            _recentlyInstalledServiceRepositoryMock
               .Setup(x => x.Where(It.IsAny<Expression<Func<MostRecentlyInstalledService, bool>>>()))
               .Returns(recentlyUpdatedServices);

            // act
            _target = _container.GetInstance<SampleSiteDeactivationProcessorService>();
            _target.Process();

            // verify save hasn't be called because all sample sites were inactive
            _sampleSiteRepositoryMock.Verify(x => x.Save(It.IsAny<SampleSite>()), Times.Never);
        }

        [TestMethod]
        public void TestSampleSiteAsLeadCopperSiteIsDeactivatedWithNewServiceRecordReason()
        {
            // add all sample site as inactive
            var recentlyUpdatedServices = GetSampleRecentlyInstalledServices(isActive: true, isLeadCopperSite: true);

            _recentlyInstalledServiceRepositoryMock
               .Setup(x => x.Where(It.IsAny<Expression<Func<MostRecentlyInstalledService, bool>>>()))
               .Returns(recentlyUpdatedServices);

            var savedSampleSites = new List<SampleSite>();
            _sampleSiteRepositoryMock.Setup(x => x.Save(It.IsAny<SampleSite>()))
                                     .Callback<SampleSite>(r => savedSampleSites.Add(r));

            // act
            _target = _container.GetInstance<SampleSiteDeactivationProcessorService>();
            _target.Process();

            // verify save hasn't be called because all sample sites were inactive
            _sampleSiteRepositoryMock.Verify(x => x.Save(It.IsAny<SampleSite>()), Times.Exactly(2));
            Assert.IsTrue(savedSampleSites.All(s => s.Status.Id == SampleSiteStatus.Indices.INACTIVE &&
                                                    s.SampleSiteInactivationReason.Id == SampleSiteInactivationReason
                                                       .Indices.NEW_SERVICE_DETAILS));
        }
    }
}
