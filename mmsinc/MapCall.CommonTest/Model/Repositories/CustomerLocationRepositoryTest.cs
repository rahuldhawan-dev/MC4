using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class CustomerLocationRepositoryTest : InMemoryDatabaseTest<CustomerLocation, CustomerLocationRepository>
    {
        #region Fields

        #endregion

        #region Tests

        [TestMethod]
        public void
            TestSearchWithHasVerifiedCoordinateReturnsItemsWithVerifiedCoordinatesIfHasVerifiedCoordinateIsTrue()
        {
            var verified = GetFactory<CustomerLocationFactory>().Create();
            GetFactory<CustomerCoordinateFactory>().Create(new {
                CustomerLocation = verified,
                Verified = true
            });
            var unverified = GetFactory<CustomerLocationFactory>().Create();
            GetFactory<CustomerCoordinateFactory>().Create(new {
                CustomerLocation = unverified,
                Verified = false
            });
            var noCoordinates = GetFactory<CustomerLocationFactory>().Create();

            var model = new TestSearchCustomerLocation();

            var result = Repository.SearchWithHasVerifiedCoordinate(new TestSearchCustomerLocation
                {HasVerifiedCoordinate = true});

            MyAssert.Contains(result, verified);
            MyAssert.DoesNotContain(result, unverified);
            MyAssert.DoesNotContain(result, noCoordinates);
        }

        [TestMethod]
        public void TestIndexReturnsItemsWithoutVerifiedCoordinatesIfHasVerifiedCoordinateIsFalse()
        {
            var verified = GetFactory<CustomerLocationFactory>().Create();
            GetFactory<CustomerCoordinateFactory>().Create(new {
                CustomerLocation = verified,
                Verified = true
            });
            var unverified = GetFactory<CustomerLocationFactory>().Create();
            GetFactory<CustomerCoordinateFactory>().Create(new {
                CustomerLocation = unverified,
                Verified = false
            });
            var noCoordinates = GetFactory<CustomerLocationFactory>().Create();

            var result = Repository.SearchWithHasVerifiedCoordinate(new TestSearchCustomerLocation
                {HasVerifiedCoordinate = false});

            MyAssert.Contains(result, unverified);
            MyAssert.DoesNotContain(result, verified);
            MyAssert.Contains(result, noCoordinates);
        }

        [TestMethod]
        public void TestIndexReturnsAllItemsIfHasVerifiedCoordinateIsUnset()
        {
            var verified = GetFactory<CustomerLocationFactory>().Create();
            GetFactory<CustomerCoordinateFactory>().Create(new {
                CustomerLocation = verified,
                Verified = true
            });
            var unverified = GetFactory<CustomerLocationFactory>().Create();
            GetFactory<CustomerCoordinateFactory>().Create(new {
                CustomerLocation = unverified,
                Verified = false
            });
            var noCoordinates = GetFactory<CustomerLocationFactory>().Create();

            var result = Repository.SearchWithHasVerifiedCoordinate(new TestSearchCustomerLocation
                {HasVerifiedCoordinate = null});

            MyAssert.Contains(result, unverified);
            MyAssert.Contains(result, verified);
            MyAssert.Contains(result, noCoordinates);
        }

        #endregion

        #region Test classes

        private class TestSearchCustomerLocation : SearchSet<CustomerLocation>, ISearchCustomerLocation
        {
            public bool? HasVerifiedCoordinate { get; set; }
        }

        #endregion
    }
}
