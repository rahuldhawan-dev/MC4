using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MMSINC.Authentication;

namespace MapCall.Common.MvcTest.Configuration
{
    [TestClass]
    public class MapResultTest : MapCallMvcInMemoryDatabaseTestBase<Coordinate>
    {
        #region Fields

        private FakeMvcApplicationTester _application;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _application = new FakeMvcApplicationTester(_container);
            _user = GetFactory<UserFactory>().Create();

            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _application.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestCoordinatesConstructorCreatesNewCoordinateSetWithCoordinates()
        {
            var expected = new List<IThingWithCoordinate>();
            var result = _container.With((IEnumerable<IThingWithCoordinate>)expected)
                                   .GetInstance<TestMapResultWithCoordinates>();
            Assert.AreSame(expected, result.CoordinateSets.Single().Coordinates);
        }

        [TestMethod]
        public void TestCoordinatesConstructorThrowsIfParameterIsNull()
        {
            MyAssert.Throws(() => new TestMapResult(null, null));
        }

        [TestMethod]
        public void TestEmptyConstructorSetsInitialSerializableValues()
        {
            Assert.IsNotNull(_container.GetInstance<TestMapResult>().SerializableValues);
            Assert.IsNotNull(_container.GetInstance<TestMapResult>().CoordinateSets,
                "The CoordinateSets property can not be null, so this should be an empty enumerable by default.");
            Assert.IsTrue(_container.GetInstance<TestMapResult>().ModelStateIsValid);
        }

        [TestMethod]
        public void TestBaseDataPropertyIsSetToSerializableValuesDictionary()
        {
            var target = _container.GetInstance<TestMapResult>();
            var expected = target.SerializableValues;
            var getter = typeof(JsonResult).GetProperty("Data");
            var result = getter.GetValue(target, new object[] { });
            Assert.AreSame(expected, result);
        }

        [TestMethod]
        public void TestDataPropertyThrowsExceptionOnGetterAndSetter()
        {
            var result = _container.GetInstance<TestMapResult>();
            object getResult = null;
            MyAssert.Throws<NotSupportedException>(() => result.Data = "uh");
            MyAssert.Throws<NotSupportedException>(() => getResult = result.Data);
        }

        [TestMethod]
        public void TestExecuteResultAddsSerializedCoordinatesToSerializableValues()
        {
            var result = _container.GetInstance<TestMapResult>();
            var expectedModel = new TestMappableModel {
                Id = 99,
                Coordinate = new Coordinate {
                    Icon = new MapIcon(),
                    Latitude = 25.1m,
                    Longitude = 96.1m
                }
            };

            MMSINC.ClassExtensions.ObjectExtensions.ObjectExtensions.SetPropertyValueByName(
                expectedModel.Coordinate.Icon, "Id", 1999);
            result.CoordinateSets.Add(_container.With((IEnumerable<IThingWithCoordinate>)new[] {expectedModel})
                                                .GetInstance<MapResultCoordinateSetWithCoordinates>());

            var request = new FakeMvcHttpHandler(_container);
            var controller = request.CreateAndInitializeController<FakeCrudController>();
            Assert.IsFalse(result.SerializableValues.ContainsKey("coordinates"));
            result.ExecuteResult(controller.ControllerContext);

            var resultCoordinateSets = ((IEnumerable)result.SerializableValues["coordinateSets"])
               .Cast<Dictionary<string, object>>();
            var resultCoords = (IEnumerable)resultCoordinateSets.Single()["coordinates"];

            // There can be only one!
            dynamic serializedCoordinate = resultCoords.Cast<object>().Single();

            Assert.AreEqual(expectedModel.Id, serializedCoordinate.dataId);
            Assert.AreEqual(expectedModel.Coordinate.Icon.Id, serializedCoordinate.iconId);
            Assert.AreEqual(expectedModel.Coordinate.Latitude, serializedCoordinate.lat);
            Assert.AreEqual(expectedModel.Coordinate.Longitude, serializedCoordinate.lng);
        }

        [TestMethod]
        public void TestExecuteResultIgnoresRecordsWithoutACoordinate()
        {
            var result = _container.GetInstance<TestMapResult>();
            var expectedModel = new TestMappableModel {
                Id = 99,
                Coordinate = null
            };

            result.CoordinateSets.Add(_container.With(new[] {expectedModel}).GetInstance<MapResultCoordinateSet>());

            var request = new FakeMvcHttpHandler(_container);
            var controller = request.CreateAndInitializeController<FakeCrudController>();
            Assert.IsFalse(result.SerializableValues.ContainsKey("coordinates"));
            result.ExecuteResult(controller.ControllerContext);

            var resultCoordinateSets = ((IEnumerable)result.SerializableValues["coordinateSets"])
               .Cast<Dictionary<string, object>>();
            var resultCoords = (IEnumerable)resultCoordinateSets.Single()["coordinates"];

            Assert.AreEqual(0, resultCoords.Cast<object>().Count(), "No coordinates should have been serialized.");
        }

        [TestMethod]
        public void TestExecuteResultIgnoresRecordWithCoordinatesThatHaveZeroForLatitudeOrLongitude()
        {
            var result = _container.GetInstance<TestMapResult>();
            var expectedModel = new TestMappableModel {
                Id = 99,
                Coordinate = new Coordinate {
                    Longitude = 0,
                    Latitude = 0
                }
            };

            result.CoordinateSets.Add(_container.With(new[] {expectedModel}).GetInstance<MapResultCoordinateSet>());

            var request = new FakeMvcHttpHandler(_container);
            var controller = request.CreateAndInitializeController<FakeCrudController>();
            Assert.IsFalse(result.SerializableValues.ContainsKey("coordinates"));
            result.ExecuteResult(controller.ControllerContext);

            var resultCoordinateSets = ((IEnumerable)result.SerializableValues["coordinateSets"])
               .Cast<Dictionary<string, object>>();
            var resultCoords = (IEnumerable)resultCoordinateSets.Single()["coordinates"];

            Assert.AreEqual(0, resultCoords.Cast<object>().Count(), "No coordinates should have been serialized.");
        }

        [TestMethod]
        public void TestExecuteUsesIconAndNotCoordinateIconWhenSerializingCoordinates()
        {
            var result = _container.GetInstance<TestMapResult>();
            var wrongMapIcon = new MapIcon();
            MMSINC.ClassExtensions.ObjectExtensions.ObjectExtensions.SetPropertyValueByName(wrongMapIcon, "Id", 1999);
            var correctMapIcon = new MapIcon();
            MMSINC.ClassExtensions.ObjectExtensions.ObjectExtensions.SetPropertyValueByName(correctMapIcon, "Id", 42);

            var expectedModel = new TestDynamicIconModel {
                Id = 99,
                Coordinate = new Coordinate {
                    Icon = new MapIcon(),
                    Latitude = 25.1m,
                    Longitude = 96.1m
                },
                Icon = correctMapIcon
            };

            result.CoordinateSets.Add(_container.With((IEnumerable<IThingWithCoordinate>)new[] {expectedModel})
                                                .GetInstance<MapResultCoordinateSetWithCoordinates>());

            var request = new FakeMvcHttpHandler(_container);
            var controller = request.CreateAndInitializeController<FakeCrudController>();
            Assert.IsFalse(result.SerializableValues.ContainsKey("coordinates"));
            result.ExecuteResult(controller.ControllerContext);

            var resultCoordinateSets = ((IEnumerable)result.SerializableValues["coordinateSets"])
               .Cast<Dictionary<string, object>>();
            var resultCoords = (IEnumerable)resultCoordinateSets.Single()["coordinates"];

            // There can be only one!
            dynamic serializedCoordinate = resultCoords.Cast<object>().Single();

            Assert.AreEqual(expectedModel.Id, serializedCoordinate.dataId);
            Assert.AreEqual(expectedModel.Icon.Id, serializedCoordinate.iconId);
            Assert.AreEqual(expectedModel.Coordinate.Latitude, serializedCoordinate.lat);
            Assert.AreEqual(expectedModel.Coordinate.Longitude, serializedCoordinate.lng);
        }

        [TestMethod]
        public void TestExecuteUsesSingleDefaultIconForAnyCoordinateThatHasANullIcon()
        {
            var defaultMapIcon = new MapIcon();
            defaultMapIcon.SetPropertyValueByName("Id", 42);
            var iconSet = new IconSet();
            iconSet.DefaultIcon = defaultMapIcon;

            var mockIconSetRepo = new Mock<IIconSetRepository>();
            mockIconSetRepo.Setup(x => x.Find(IconSets.SingleDefaultIcon)).Returns(iconSet);
            _container.Inject(mockIconSetRepo.Object);

            var result = _container.GetInstance<TestMapResult>();

            var expectedModel = new TestDynamicIconModel {
                Id = 99,
                Coordinate = new Coordinate {
                    Latitude = 25.1m,
                    Longitude = 96.1m,
                    Icon = null
                },
            };

            result.CoordinateSets.Add(_container.With((IEnumerable<IThingWithCoordinate>)new[] {expectedModel})
                                                .GetInstance<MapResultCoordinateSetWithCoordinates>());

            var request = new FakeMvcHttpHandler(_container);
            var controller = request.CreateAndInitializeController<FakeCrudController>();
            Assert.IsFalse(result.SerializableValues.ContainsKey("coordinateSets"));
            result.ExecuteResult(controller.ControllerContext);

            var resultCoordinateSets = ((IEnumerable)result.SerializableValues["coordinateSets"])
               .Cast<Dictionary<string, object>>();
            var resultCoords = ((IEnumerable)resultCoordinateSets.Single()["coordinates"]).Cast<object>();

            dynamic coord = resultCoords.Single();
            Assert.AreEqual(42, coord.iconId);
        }

        [TestMethod]
        public void TestExecuteUsesCurrentRequestsRouteValuesToGenerateUrlWhenCoordinateRouteValuesPropertyIsNull()
        {
            var expectedModel = new TestMappableModel {
                Id = 99,
                Coordinate = new Coordinate {
                    Icon = new MapIcon(),
                    Latitude = 25.1m,
                    Longitude = 96.1m
                }
            };
            MMSINC.ClassExtensions.ObjectExtensions.ObjectExtensions.SetPropertyValueByName(
                expectedModel.Coordinate.Icon, "Id", 1999);
            var expectedUrl = string.Format("/SomeController/Show/{0}.{1}", expectedModel.Id,
                ResponseFormatter.KnownExtensions.FRAGMENT);

            var target = _container.GetInstance<TestMapResult>();
            target.CoordinateSets.Add(_container.With((IEnumerable<IThingWithCoordinate>)new[] {expectedModel})
                                                .GetInstance<MapResultCoordinateSetWithCoordinates>());

            var request = _application.CreateRequestHandler("~/SomeController/SomeActionThatIsNotShow/");
            var controller = request.CreateAndInitializeController<FakeCrudController>();
            target.ExecuteResult(controller.ControllerContext);
            var resultCoordinateSets = ((IEnumerable)target.SerializableValues["coordinateSets"])
               .Cast<Dictionary<string, object>>();
            var resultCoords = (IEnumerable)resultCoordinateSets.Single()["coordinates"];

            // There can be only one!
            dynamic serializedCoordinate = resultCoords.Cast<object>().Single();
            Assert.AreEqual(expectedUrl, serializedCoordinate.url,
                "Who knows what went wrong, but we're expecting the controller to stay the same but with the Show action");
        }

        [TestMethod]
        public void TestExecuteUsesCoordinateRouteValuesToGenerateUrlWhenThePropertyIsNotNull()
        {
            var expectedModel = new TestMappableModel {
                Id = 99,
                Coordinate = new Coordinate {
                    Icon = new MapIcon(),
                    Latitude = 25.1m,
                    Longitude = 96.1m
                }
            };
            MMSINC.ClassExtensions.ObjectExtensions.ObjectExtensions.SetPropertyValueByName(
                expectedModel.Coordinate.Icon, "Id", 1999);

            var target = _container.GetInstance<TestMapResult>();
            var set = _container.With((IEnumerable<IThingWithCoordinate>)new[] {expectedModel})
                                .GetInstance<MapResultCoordinateSetWithCoordinates>();
            set.CoordinateRouteValues = new RouteValueDictionary {
                {"controller", "Different"},
                {"action", "SomeAction"}
            };
            target.CoordinateSets.Add(set);

            var expectedUrl = string.Format("/Different/SomeAction/{0}", expectedModel.Id);
            var request = _application.CreateRequestHandler("~/SomeController/SomeAction/");
            var controller = request.CreateAndInitializeController<FakeCrudController>();
            target.ExecuteResult(controller.ControllerContext);
            var resultCoordinateSets = ((IEnumerable)target.SerializableValues["coordinateSets"])
               .Cast<Dictionary<string, object>>();
            var resultCoords = (IEnumerable)resultCoordinateSets.Single()["coordinates"];

            // There can be only one!
            dynamic serializedCoordinate = resultCoords.Cast<object>().Single();
            Assert.AreEqual(expectedUrl, serializedCoordinate.url,
                "Who knows what went wrong, but we're expecting the controller to stay the same but with the Show action");
        }

        [TestMethod]
        public void TestExecuteDoesNotSerializeCoordinateUrlWhenDrawLinesIsTrue()
        {
            var expectedModel = new TestMappableModel {
                Id = 99,
                Coordinate = new Coordinate {
                    Icon = new MapIcon(),
                    Latitude = 25.1m,
                    Longitude = 96.1m
                }
            };
            MMSINC.ClassExtensions.ObjectExtensions.ObjectExtensions.SetPropertyValueByName(
                expectedModel.Coordinate.Icon, "Id", 1999);

            var target = _container.GetInstance<TestMapResult>();
            var coordSet = _container.With((IEnumerable<IThingWithCoordinate>)new[] {expectedModel})
                                     .GetInstance<MapResultCoordinateSetWithCoordinates>();
            coordSet.DrawLinesBetweenPoints = true;
            target.CoordinateSets.Add(coordSet);

            var request = _application.CreateRequestHandler("~/SomeController/SomeActionThatIsNotShow/");
            var controller = request.CreateAndInitializeController<FakeCrudController>();
            target.ExecuteResult(controller.ControllerContext);
            var resultCoordinateSets = ((IEnumerable)target.SerializableValues["coordinateSets"])
               .Cast<Dictionary<string, object>>();
            var resultCoords = (IEnumerable)resultCoordinateSets.Single()["coordinates"];

            // There can be only one!
            dynamic serializedCoordinate = resultCoords.Cast<object>().Single();
            Assert.IsNull(serializedCoordinate.url);
        }

        [TestMethod]
        public void TestExecuteSetsModelStateIsValid()
        {
            var result = _container.GetInstance<TestMapResult>();
            var request = new FakeMvcHttpHandler(_container);
            var controller = request.CreateAndInitializeController<FakeCrudController>();
            result.ExecuteResult(controller.ControllerContext);

            Assert.IsTrue(controller.ModelState.IsValid);
            Assert.AreEqual(true, result.SerializableValues["modelStateIsValid"]);
        }

        [TestMethod]
        public void TestExecuteIncludesModelStateErrors()
        {
            var result = _container.GetInstance<TestMapResult>();
            var request = new FakeMvcHttpHandler(_container);
            var controller = request.CreateAndInitializeController<FakeCrudController>();
            controller.ModelState.AddModelError("Oopsy", "Doodle Doo");
            result.ExecuteResult(controller.ControllerContext);

            var errors = (Dictionary<string, string>)result.SerializableValues["modelStateErrors"];
            Assert.AreEqual("Doodle Doo", errors["Oopsy"]);
            Assert.AreEqual(1, errors.Count);
        }

        #region Test class

        private class TestMapResult : MapResult
        {
            public Dictionary<string, object> SerializableValues => _serializableValues;

            public TestMapResult(IAuthenticationService<User> authenticationService,
                IIconSetRepository iconSetRepository) : base(authenticationService, iconSetRepository) { }
        }

        private class TestMapResultWithCoordinates : TestMapResult
        {
            public TestMapResultWithCoordinates(IAuthenticationService<User> authenticationService,
                IIconSetRepository iconSetRepository, IEnumerable<IThingWithCoordinate> coordinates) : base(
                authenticationService, iconSetRepository)
            {
                CoordinateSets.Add(new MapResultCoordinateSetWithCoordinates(iconSetRepository, coordinates));
            }
        }

        private class TestMappableModel : IThingWithCoordinate
        {
            public Coordinate Coordinate { get; set; }
            public int Id { get; set; }
            public MapIcon Icon => Coordinate?.Icon;
        }

        private class TestDynamicIconModel : IThingWithCoordinate
        {
            public Coordinate Coordinate { get; set; }
            public int Id { get; set; }
            public MapIcon Icon { get; set; }
        }

        #endregion
    }
}
