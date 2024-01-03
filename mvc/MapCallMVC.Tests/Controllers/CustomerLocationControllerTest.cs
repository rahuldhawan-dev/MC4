using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class CustomerLocationControllerTest : MapCallMvcControllerTestBase<CustomerLocationController, CustomerLocation>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ICustomerLocationRepository>().Use<CustomerLocationRepository>();
            e.For<ICustomerCoordinateRepository>().Use<CustomerCoordinateRepository>();
        }

        #endregion

        #region Tests

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesImages;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/CustomerLocation/Search", module, RoleActions.Read);
                a.RequiresRole("~/CustomerLocation/Index", module, RoleActions.Read);
                a.RequiresRole("~/CustomerLocation/Edit", module, RoleActions.Read);
                a.RequiresRole("~/CustomerLocation/Update", module, RoleActions.Read);
                a.RequiresRole("~/CustomerLocation/CitiesByState", module, RoleActions.Read);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexReturnsItemsWithVerifiedCoordinatesIfHasVerifiedCoordinateIsTrue()
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

            var result = _target.Index(new SearchCustomerLocation {HasVerifiedCoordinate = true}) as ViewResult;
            var resultModel = (SearchCustomerLocation)result.Model;

            MyAssert.Contains(resultModel.Results, verified);
            MyAssert.DoesNotContain(resultModel.Results, unverified);
            MyAssert.DoesNotContain(resultModel.Results, noCoordinates);
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

            var result = _target.Index(new SearchCustomerLocation {HasVerifiedCoordinate = false}) as ViewResult;
            var resultModel = (SearchCustomerLocation)result.Model;

            MyAssert.Contains(resultModel.Results, unverified);
            MyAssert.DoesNotContain(resultModel.Results, verified);
            MyAssert.Contains(resultModel.Results, noCoordinates);
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

            var result = _target.Index(new SearchCustomerLocation {HasVerifiedCoordinate = null}) as ViewResult;
            var resultModel = (SearchCustomerLocation)result.Model;

            MyAssert.Contains(resultModel.Results, unverified);
            MyAssert.Contains(resultModel.Results, verified);
            MyAssert.Contains(resultModel.Results, noCoordinates);
        }

        [TestMethod]
        public void TestIndexRendersPartialWhenFragmentRequested()
        {
            var verified = GetFactory<CustomerLocationFactory>().Create();
            GetFactory<CustomerCoordinateFactory>().Create(new {
                CustomerLocation = verified,
                Verified = true
            });

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.FRAGMENT;
            var result = _target.Index(new SearchCustomerLocation {HasVerifiedCoordinate = true}) as ViewResult;
            var resultModel = (SearchCustomerLocation)result.Model;

            Assert.AreEqual(result.ViewName, "_Index");
            MyAssert.Contains(resultModel.Results, verified);
        }

        #endregion

        #region Search

        [TestMethod]
        public void TestSearchReturnsSearchPartialViewWhenFragmentRequested()
        {
            var search = new SearchCustomerLocation();

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.FRAGMENT;
            var result = _target.Search(search) as ViewResult;

            MvcAssert.IsViewNamed(result, "_Search");
            MvcAssert.IsViewWithModel(result, search);
        }

        [TestMethod]
        public void TestSearchSetsLookupData()
        {
            var states = new[] {"NJ", "NY", "PA"};
            foreach (var state in states)
            {
                GetFactory<CustomerLocationFactory>().Create(new {State = state});
            }

            _target.Search(null);

            foreach (var item in (IEnumerable<SelectListItem>)_target.ViewData["State"])
            {
                MyAssert.Contains(states, item.Value);
                Assert.AreEqual(item.Value, item.Text);
            }
        }

        #endregion

        #region Edit

        [TestMethod]
        public void TestEditRendersModelInPartialViewIfFragmentWasRequested()
        {
            var unverified = GetFactory<CustomerLocationFactory>().Create();
            GetFactory<CustomerCoordinateFactory>().Create(new {
                CustomerLocation = unverified,
                Verified = false
            });
            
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.FRAGMENT;
            var result = _target.Edit(unverified.Id) as ViewResult;
            var resultModel = result.Model as UpdateCustomerLocation;

            Assert.AreEqual(resultModel.Id, unverified.Id);
            Assert.AreEqual(result.ViewName, "_Edit");
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            // noop: redirects to Index and does a million other things that won't be setup for this test.
        }

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            // noop: Action doesn't do this, however:
            Assert.Inconclusive("I don't check if I'm valid or not");
        }

        [TestMethod]
        public void TestUpdateCreatesNewCustomerCoordinateAndRedirectsToIndex()
        {
            var unverified = GetFactory<CustomerLocationFactory>().Create();
            GetFactory<CustomerCoordinateFactory>().Create(new {
                CustomerLocation = unverified,
                Verified = false
            });
            RedirectToRouteResult result = null;

            MyAssert.CausesIncrease(() => result = _target.Update(
                _viewModelFactory.BuildWithOverrides<UpdateCustomerLocation, CustomerLocation>(unverified, new {
                    Latitude = 1,
                    Longitude = 2
                })) as RedirectToRouteResult, _container.GetInstance<CustomerCoordinateRepository>().GetAll().Count);

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME],
                ResponseFormatter.KnownExtensions.FRAGMENT);
        }

        [TestMethod]
        public void TestUpdateUpdatesExistingVerifiedCustomerCoordinateForLocationWhichAlreadyHasOne()
        {
            var verified = GetFactory<CustomerLocationFactory>().Create();
            GetFactory<CustomerCoordinateFactory>().Create(new {
                CustomerLocation = verified,
                Verified = true
            });
            RedirectToRouteResult result = null;
            Session.Clear();

            MyAssert.DoesNotCauseIncrease(() =>
                    result = _target.Update(
                        _viewModelFactory.BuildWithOverrides<UpdateCustomerLocation, CustomerLocation>(verified, new {
                            Latitude = 1,
                            Longitude = 2
                        })) as RedirectToRouteResult,
                _container.GetInstance<CustomerCoordinateRepository>().GetAll().Count);

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME],
                ResponseFormatter.KnownExtensions.FRAGMENT);
        }

        #endregion

        [TestMethod]
        public void TestCitiesByStateReturnsUniqueLocationsCitiesByState()
        {
            var i = 1;
            Func<string> getCityName = () => string.Format("City{0}", i++);
            var expected = GetFactory<CustomerLocationFactory>().CreateList(3, new {
                State = "NJ",
                City = getCityName
            }).Map(cl => cl.City);
            // extra
            GetFactory<CustomerLocationFactory>().CreateList(3, new {
                State = "NY",
                City = getCityName
            });

            var result = _target.CitiesByState("NJ") as CascadingActionResult;

            foreach (dynamic item in result.Data)
            {
                MyAssert.Contains(expected, item.City);
            }
        }

        #endregion
    }
}
