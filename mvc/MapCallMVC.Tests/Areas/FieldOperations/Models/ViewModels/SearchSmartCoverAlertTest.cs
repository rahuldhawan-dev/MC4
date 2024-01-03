using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class SearchSmartCoverAlertTest : InMemoryDatabaseTest<SmartCoverAlert, IRepository<SmartCoverAlert>>
    {
        #region Fields

        private SearchSmartCoverAlert _target;
        private OperatingCenter _nj4, _nj7;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IDateTimeProvider>().Mock();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new SearchSmartCoverAlert();
            _nj4 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ4" });
            _nj7 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ7" });
            Repository = _container.GetInstance<IRepository<SmartCoverAlert>>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestUserCanSearchByProperty()
        {
            var sewerOpening1 = GetFactory<SewerOpeningFactory>().Create(new { OperatingCenter = _nj4 });
            var sewerOpening2 = GetFactory<SewerOpeningFactory>().Create(new { OperatingCenter = _nj7 });
            var smartCoverAlertType1 = GetEntityFactory<SmartCoverAlertType>().Create();
            var smartCoverAlertType2 = GetEntityFactory<SmartCoverAlertType>().Create();

            var alert1 = GetEntityFactory<SmartCoverAlert>().Create(new {
                SewerOpening = sewerOpening1,
                SewerOpeningNumber = "1234567",
                ApplicationDescription = GetEntityFactory<SmartCoverAlertApplicationDescriptionType>().Create()
            });
            alert1.SmartCoverAlertSmartCoverAlertTypes = new List<SmartCoverAlertSmartCoverAlertType> {
                new SmartCoverAlertSmartCoverAlertType {
                    SmartCoverAlert = alert1,
                    SmartCoverAlertType = smartCoverAlertType1
                }
            };
            var alert2 = GetEntityFactory<SmartCoverAlert>().Create(new {
                SewerOpening = sewerOpening2,
                SewerOpeningNumber = "9876543",
                ApplicationDescription = GetEntityFactory<SmartCoverAlertApplicationDescriptionType>().Create()
            });
            alert2.SmartCoverAlertSmartCoverAlertTypes = new List<SmartCoverAlertSmartCoverAlertType> {
                new SmartCoverAlertSmartCoverAlertType {
                    SmartCoverAlert = alert2,
                    SmartCoverAlertType = smartCoverAlertType2
                }
            };
            var alert3 = GetEntityFactory<SmartCoverAlert>().Create(new { Acknowledged = true });
            alert3.SmartCoverAlertSmartCoverAlertTypes = new List<SmartCoverAlertSmartCoverAlertType> {
                new SmartCoverAlertSmartCoverAlertType {
                    SmartCoverAlert = alert3,
                    SmartCoverAlertType = smartCoverAlertType1
                },
                new SmartCoverAlertSmartCoverAlertType {
                    SmartCoverAlert = alert3,
                    SmartCoverAlertType = smartCoverAlertType2
                }
            };
            GetFactory<WorkOrderFactory>().Create(new { SmartCoverAlert = alert3 });
            Session.Save(alert1);
            Session.Save(alert2);
            Session.Save(alert3);
            Session.Flush();

            var results = Repository.Search(_target).ToArray();
            Assert.AreEqual(3, results.Length);

            _target.State = _nj4.State.Id;
            results = Repository.Search(_target).ToArray();
            Assert.AreEqual(2, results.Length);
            Assert.IsTrue(results.Contains(alert1));
            Assert.IsTrue(results.Contains(alert2));

            _target.OperatingCenter = _nj4.Id;
            results = Repository.Search(_target).ToArray();
            Assert.AreEqual(1, results.Length);
            Assert.IsTrue(results.Contains(alert1));

            _target.OperatingCenter = null;
            _target.State = null;
            _target.SewerOpeningNumber = new SearchString {
                Value = alert1.SewerOpeningNumber
            };
            results = Repository.Search(_target).ToArray();
            Assert.AreEqual(1, results.Length);
            Assert.IsTrue(results.Contains(alert1));

            _target.SewerOpeningNumber = null;
            _target.ApplicationDescription = new []{ alert1.ApplicationDescription.Id, alert2.ApplicationDescription.Id };
            results = Repository.Search(_target).ToArray();
            Assert.AreEqual(2, results.Length);
            Assert.IsTrue(results.Contains(alert1));
            Assert.IsTrue(results.Contains(alert2));

            _target.ApplicationDescription = null;
            _target.WorkOrders = true;
            results = Repository.Search(_target).ToArray();
            Assert.AreEqual(1, results.Length);
            Assert.IsTrue(results.Contains(alert3));

            _target.WorkOrders = null;
            _target.Acknowledged = true;
            results = Repository.Search(_target).ToArray();
            Assert.AreEqual(1, results.Length);
            Assert.IsTrue(results.Contains(alert3));

            _target.Acknowledged = null;
            _target.AlertType = new[] { smartCoverAlertType1.Id };
            results = Repository.Search(_target).ToArray();
            Assert.AreEqual(2, results.Length);
            Assert.IsTrue(results.Contains(alert1));
            Assert.IsTrue(results.Contains(alert3));

            _target.AlertType = new[] { smartCoverAlertType2.Id };
            results = Repository.Search(_target).ToArray();
            Assert.AreEqual(2, results.Length);
            Assert.IsTrue(results.Contains(alert2));
            Assert.IsTrue(results.Contains(alert3));
        }

        #endregion
    }
}
