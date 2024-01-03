using System;
using System.Collections.Generic;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class FilterMediaViewModelTest : MapCallMvcInMemoryDatabaseTestBase<FilterMedia>
    {
        #region Setup/Teardown

        protected IRepository<FilterMedia> Repository
        {
            get { return _container.GetInstance<IRepository<FilterMedia>>(); }
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
        }

        #endregion

        [TestMethod]
        public void TestSettingFilterNumberToNumberOfExistingFilterAdjustsNeighboringFiltersAccordingly()
        {
            // going down
            int filterNumber = 0;
            Func<int> getFilterNumber = () => ++filterNumber;
            var equipment = GetFactory<EquipmentFactory>().Create();
            IList<FilterMedia> filterMediae = GetFactory<FilterMediaFactory>().CreateList(5, new {Equipment = equipment, FilterNumber = getFilterNumber});
            Session.Flush();
            Session.Clear();

            var target = _viewModelFactory.Build<FilterMediaViewModel, FilterMedia>(filterMediae[3]);
            target.FilterNumber = 2;

            Repository.Save(target.MapToEntity(filterMediae[3]));
            Session.Flush();
            Session.Clear();

            filterMediae = filterMediae.Map(fm => Session.Load<FilterMedia>(fm.Id));

            Assert.AreEqual(1, filterMediae[0].FilterNumber);
            Assert.AreEqual(3, filterMediae[1].FilterNumber);
            Assert.AreEqual(4, filterMediae[2].FilterNumber);
            Assert.AreEqual(2, filterMediae[3].FilterNumber);
            Assert.AreEqual(5, filterMediae[4].FilterNumber);

            // going up
            filterNumber = 0;
            filterMediae = GetFactory<FilterMediaFactory>().CreateList(5, new {Equipment = equipment, FilterNumber = getFilterNumber});
            Session.Flush();
            Session.Clear();

            target = _viewModelFactory.Build<FilterMediaViewModel, FilterMedia>(filterMediae[1]);
            target.FilterNumber = 4;

            Repository.Save(target.MapToEntity(filterMediae[1]));
            Session.Flush();
            Session.Clear();

            filterMediae = filterMediae.Map(fm => Session.Load<FilterMedia>(fm.Id));

            Assert.AreEqual(1, filterMediae[0].FilterNumber);
            Assert.AreEqual(4, filterMediae[1].FilterNumber);
            Assert.AreEqual(2, filterMediae[2].FilterNumber);
            Assert.AreEqual(3, filterMediae[3].FilterNumber);
            Assert.AreEqual(5, filterMediae[4].FilterNumber);
        }
    }
}
