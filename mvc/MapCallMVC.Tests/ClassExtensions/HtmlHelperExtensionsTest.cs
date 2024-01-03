using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.ClassExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.StructureMap;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.ClassExtensions
{
    [TestClass, DoNotParallelize]
    public class HtmlHelperExtensionsTest
    {
        #region Private Members

        private MapCallMvcApplicationTester _application;
        private HtmlHelper<CoordinatePickerTestModel> _target;
        private CoordinatePickerTestModel _model;
        private FakeMvcHttpHandler _request;
        private Coordinate _coordinate;
        private Mock<IAuthenticationService<User>> _authenticationService;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = MapCallMvcApplicationTester.InitializeDummyObjectFactory();
            _container.Inject((_authenticationService = new Mock<IAuthenticationService<User>>()).Object);
            _container.Inject<IAuthenticationService>(_authenticationService.Object);
            _container.Inject(new Mock<IRoleService>().Object);
            _container.Inject(new Mock<IRepository<Coordinate>>().Object);

            _application = _container.With(true).GetInstance<MapCallMvcApplicationTester>();
            _request = _application.CreateRequestHandler();
            _model = new CoordinatePickerTestModel();
            _target = _request.CreateHtmlHelper(_model);

            _coordinate = new Coordinate {
                Latitude = 12.345m,
                Longitude = 23.456m
            };
            _coordinate.SetPropertyValueByName("Id", 123);
            _model.DisplayCoordinate = _coordinate;
            _model.NotRequiredCoordinateId = _coordinate.Id;

            var mockRepo = new Mock<IRepository<Coordinate>>();
            mockRepo.Setup(x => x.Find(_coordinate.Id)).Returns(_coordinate);
            _container.Inject(mockRepo.Object);

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }



        [TestCleanup]
        public void TestCleanup()
        {
            _application.Dispose();
        }


        #endregion

        #region CoordinatePickerFor

        [TestMethod]
        public void TestCoordinatePickerReturnsAnImageAndHiddenInput()
        {
            _model.NotRequiredCoordinateId = null;

            var result = _target.CoordinatePicker("NotRequiredCoordinateId").ToString();

            MyAssert.Contains(
                $"<img class=\"{CoordinatePickerBuilder.ICON_CSS_CLASS}\" coordinateUrl=\"/Coordinate/New?valueFor=NotRequiredCoordinateId&amp;iconSet=0\" src=\"/Content/images/map-icon-red.png\" />",
                result);
            MyAssert.Contains(
                "<button coordinateUrl=\"/Coordinate/New?valueFor=NotRequiredCoordinateId&amp;iconSet=0&amp;manual=true\" id=\"coordinateManualEntryButton\" type=\"button\">Advanced</button>",
                result);
            MyAssert.Contains("<span class=\"cp-coordinate-values\"></span>",
                result);
            MyAssert.Contains(
                "<input data-val=\"true\" data-val-integer=\"NotRequiredCoordinateId must be a whole number.\" data-val-number=\"The field NotRequiredCoordinateId must be a number.\" id=\"NotRequiredCoordinateId\" name=\"NotRequiredCoordinateId\" type=\"hidden\" value=\"\" />",
                result);
        }

        [TestMethod]
        public void TestCoordinatePickerForReturnsAnImageAndHiddenInput()
        {
            _model.NotRequiredCoordinateId = null;

            var result = _target.CoordinatePickerFor(f => f.NotRequiredCoordinateId).ToString();

            MyAssert.Contains(
                $"<img class=\"{CoordinatePickerBuilder.ICON_CSS_CLASS}\" coordinateUrl=\"/Coordinate/New?valueFor=NotRequiredCoordinateId&amp;iconSet=0\" src=\"/Content/images/map-icon-red.png\" />",
                result);
            MyAssert.Contains(
                "<button coordinateUrl=\"/Coordinate/New?valueFor=NotRequiredCoordinateId&amp;iconSet=0&amp;manual=true\" id=\"coordinateManualEntryButton\" type=\"button\">Advanced</button>",
                result);
            MyAssert.Contains("<span class=\"cp-coordinate-values\"></span>",
                result);
            MyAssert.Contains(
                "<input data-val=\"true\" data-val-integer=\"NotRequiredCoordinateId must be a whole number.\" data-val-number=\"The field NotRequiredCoordinateId must be a number.\" id=\"NotRequiredCoordinateId\" name=\"NotRequiredCoordinateId\" type=\"hidden\" value=\"\" />",
                result);
        }

        [TestMethod]
        public void TestCoordinatePickerForReturnsCoordinateUrlWithIconSetBasedOnCoordinateAttribute()
        {
            _model.AttributedCoordinate = null;

            var result = _target.CoordinatePickerFor(f => f.AttributedCoordinate).ToString();

            MyAssert.Contains(
                $"<img class=\"{CoordinatePickerBuilder.ICON_CSS_CLASS}\" coordinateUrl=\"/Coordinate/New?valueFor=AttributedCoordinate&amp;iconSet=5\" src=\"/Content/images/map-icon-red.png\" />",
                result);
            MyAssert.Contains(
                "<button coordinateUrl=\"/Coordinate/New?valueFor=AttributedCoordinate&amp;iconSet=5&amp;manual=true\" id=\"coordinateManualEntryButton\" type=\"button\">Advanced</button>",
                result);

        }

        [TestMethod]
        public void TestCoordinatePickerReturnsCoordinateUrlWithIconSetBasedOnCoordinateAttribute()
        {
            _model.AttributedCoordinate = null;

            var result = _target.CoordinatePicker("AttributedCoordinate").ToString();

            MyAssert.Contains(
                $"<img class=\"{CoordinatePickerBuilder.ICON_CSS_CLASS}\" coordinateUrl=\"/Coordinate/New?valueFor=AttributedCoordinate&amp;iconSet=5\" src=\"/Content/images/map-icon-red.png\" />",
                result);
            MyAssert.Contains(
                "<button coordinateUrl=\"/Coordinate/New?valueFor=AttributedCoordinate&amp;iconSet=5&amp;manual=true\" id=\"coordinateManualEntryButton\" type=\"button\">Advanced</button>",
                result);
        }


        [TestMethod]
        public void TestCoordinatePickerForSetsUpValidationForRequiredValues()
        {
            _model.RequiredCoordinateId = null;
            using (_target.BeginForm("Create", "Coordinate", HttpMethod.Post))
            {
                var result = _target.CoordinatePickerFor(f => f.RequiredCoordinateId).ToString();

                MyAssert.Contains(
                    $"<img class=\"{CoordinatePickerBuilder.ICON_CSS_CLASS}\" coordinateUrl=\"/Coordinate/New?valueFor=RequiredCoordinateId&amp;iconSet=0\" src=\"/Content/images/map-icon-red.png\" />",
                    result);
                MyAssert.Contains(
                    "<button coordinateUrl=\"/Coordinate/New?valueFor=RequiredCoordinateId&amp;iconSet=0&amp;manual=true\" id=\"coordinateManualEntryButton\" type=\"button\">Advanced</button>",
                    result);
                MyAssert.Contains(
                    "<input data-val=\"true\" data-val-integer=\"RequiredCoordinateId must be a whole number.\" data-val-number=\"The field RequiredCoordinateId must be a number.\" data-val-required=\"The RequiredCoordinateId field is required.\" id=\"RequiredCoordinateId\" name=\"RequiredCoordinateId\" type=\"hidden\" value=\"\" />",
                    result);
            }
        }

        [TestMethod]
        public void TestCoordinatePickerSetsUpValidationForRequiredValues()
        {
            _model.RequiredCoordinateId = null;
            using (_target.BeginForm("Create", "Coordinate", HttpMethod.Post))
            {
                var result = _target.CoordinatePicker("RequiredCoordinateId").ToString();

                MyAssert.Contains(
                    $"<img class=\"{CoordinatePickerBuilder.ICON_CSS_CLASS}\" coordinateUrl=\"/Coordinate/New?valueFor=RequiredCoordinateId&amp;iconSet=0\" src=\"/Content/images/map-icon-red.png\" />",
                    result);
                MyAssert.Contains(
                    "<button coordinateUrl=\"/Coordinate/New?valueFor=RequiredCoordinateId&amp;iconSet=0&amp;manual=true\" id=\"coordinateManualEntryButton\" type=\"button\">Advanced</button>",
                    result);
                MyAssert.Contains(
                    "<input data-val=\"true\" data-val-integer=\"RequiredCoordinateId must be a whole number.\" data-val-number=\"The field RequiredCoordinateId must be a number.\" data-val-required=\"The RequiredCoordinateId field is required.\" id=\"RequiredCoordinateId\" name=\"RequiredCoordinateId\" type=\"hidden\" value=\"\" />",
                    result);
            }
        }

        [TestMethod]
        public void TestCoordinatePickerForUsesBlueIconForExistingCoordinate()
        {
            _model.NotRequiredCoordinateId = 123;

            var result = _target.CoordinatePickerFor(f => f.NotRequiredCoordinateId).ToString();

            MyAssert.Contains(
                $"<img class=\"{CoordinatePickerBuilder.ICON_CSS_CLASS}\" coordinateUrl=\"/Coordinate/New/123?valueFor=NotRequiredCoordinateId&amp;iconSet=0\" src=\"/Content/images/map-icon-blue.png\" />",
                result);
            MyAssert.Contains(
                "<button coordinateUrl=\"/Coordinate/New/123?valueFor=NotRequiredCoordinateId&amp;iconSet=0&amp;manual=true\" id=\"coordinateManualEntryButton\" type=\"button\">Advanced</button>",
                result);
            MyAssert.Contains("<span class=\"cp-coordinate-values\">12.345, 23.456</span>",
                result);
            MyAssert.Contains(
                "<input data-val=\"true\" data-val-integer=\"NotRequiredCoordinateId must be a whole number.\" data-val-number=\"The field NotRequiredCoordinateId must be a number.\" id=\"NotRequiredCoordinateId\" name=\"NotRequiredCoordinateId\" type=\"hidden\" value=\"123\" />",
                result);
        }

        [TestMethod]
        public void TestCoordinatePickerUsesBlueIconForExistingCoordinate()
        {
            _model.NotRequiredCoordinateId = 123;

            var result = _target.CoordinatePicker("NotRequiredCoordinateId").ToString();

            MyAssert.Contains(
                $"<img class=\"{CoordinatePickerBuilder.ICON_CSS_CLASS}\" coordinateUrl=\"/Coordinate/New/123?valueFor=NotRequiredCoordinateId&amp;iconSet=0\" src=\"/Content/images/map-icon-blue.png\" />",
                result);
            MyAssert.Contains(
                "<button coordinateUrl=\"/Coordinate/New/123?valueFor=NotRequiredCoordinateId&amp;iconSet=0&amp;manual=true\" id=\"coordinateManualEntryButton\" type=\"button\">Advanced</button>",
                result);
            MyAssert.Contains("<span class=\"cp-coordinate-values\">12.345, 23.456</span>",
                result);
            MyAssert.Contains(
                "<input data-val=\"true\" data-val-integer=\"NotRequiredCoordinateId must be a whole number.\" data-val-number=\"The field NotRequiredCoordinateId must be a number.\" id=\"NotRequiredCoordinateId\" name=\"NotRequiredCoordinateId\" type=\"hidden\" value=\"123\" />",
                result);
        }

        [TestMethod]
        public void TestCoordinatePickerReturnsImageWithDataAddressField()
        {
            _model.WithAddressField = null;

            var result = _target.CoordinatePicker("WithAddressField").ToString();

            MyAssert.Contains(
                $"<img class=\"{CoordinatePickerBuilder.ICON_CSS_CLASS}\" coordinateUrl=\"/Coordinate/New?valueFor=WithAddressField&amp;iconSet=0\" data-address-field=\"SomeAddressField\" src=\"/Content/images/map-icon-red.png\" />",
                result);
        }

        [TestMethod]
        public void TestCoordinatePickerForReturnsImageWithDataAddressField()
        {
            _model.WithAddressField = null;

            var result = _target.CoordinatePickerFor(f => f.WithAddressField).ToString();

            MyAssert.Contains(
                $"<img class=\"{CoordinatePickerBuilder.ICON_CSS_CLASS}\" coordinateUrl=\"/Coordinate/New?valueFor=WithAddressField&amp;iconSet=0\" data-address-field=\"SomeAddressField\" src=\"/Content/images/map-icon-red.png\" />",
                result);
        }

        [TestMethod]
        public void TestCoordinatePickerReturnsImageWithDataAddressCallback()
        {
            _model.WithAddressCallback = null;

            var result = _target.CoordinatePicker("WithAddressCallback").ToString();

            MyAssert.Contains(
                $"<img class=\"{CoordinatePickerBuilder.ICON_CSS_CLASS}\" coordinateUrl=\"/Coordinate/New?valueFor=WithAddressCallback&amp;iconSet=0\" data-address-callback=\"SomeAddressCallback\" src=\"/Content/images/map-icon-red.png\" />",
                result);
        }

        [TestMethod]
        public void TestCoordinatePickerForReturnsImageWithDataAddressCallback()
        {
            _model.WithAddressCallback = null;

            var result = _target.CoordinatePickerFor(f => f.WithAddressCallback).ToString();

            MyAssert.Contains(
                $"<img class=\"{CoordinatePickerBuilder.ICON_CSS_CLASS}\" coordinateUrl=\"/Coordinate/New?valueFor=WithAddressCallback&amp;iconSet=0\" data-address-callback=\"SomeAddressCallback\" src=\"/Content/images/map-icon-red.png\" />",
                result);
        }

        #endregion

        #region CoordinateDisplayFor

        [TestMethod]
        public void TestCoordinateDisplayForReturnsAnImageLink()
        {
            _model.NotRequiredCoordinateId = 123;

            Assert.AreEqual("<img class=\"coordinate-display-icon\" coordinateUrl=\"/Coordinate/Show/123\" src=\"/Content/images/map-icon-blue.png\" /><span class=\"cp-coordinate-values\">12.345, 23.456</span>",
                _target.CoordinateDisplayFor(f => f.NotRequiredCoordinateId).ToString());
        }

        [TestMethod]
        public void TestCoordinateDisplayReturnsAnImageLink()
        {
            _model.NotRequiredCoordinateId = 123;

            Assert.AreEqual("<img class=\"coordinate-display-icon\" coordinateUrl=\"/Coordinate/Show/123\" src=\"/Content/images/map-icon-blue.png\" /><span class=\"cp-coordinate-values\">12.345, 23.456</span>",
                _target.CoordinateDisplay("NotRequiredCoordinateId").ToString());
        }

        [TestMethod]
        public void TestCoordinateDisplayForReturnsAnImageWithTitleForRecordWithNoCoordinates()
        {
            _model.NotRequiredCoordinateId = null;
            Assert.AreEqual("<img class=\"coordinate-display-icon\" coordinateUrl=\"\" src=\"/Content/images/map-icon-red.png\" title=\"This record has no coordinates set.\" /><span class=\"cp-coordinate-values\"></span>",
                _target.CoordinateDisplayFor(f => f.NotRequiredCoordinateId).ToString());
        }

        [TestMethod]
        public void TestCoordinateDisplayReturnsAnImageWithTitleForRecordWithNoCoordinates()
        {
            _model.NotRequiredCoordinateId = null;
            Assert.AreEqual("<img class=\"coordinate-display-icon\" coordinateUrl=\"\" src=\"/Content/images/map-icon-red.png\" title=\"This record has no coordinates set.\" /><span class=\"cp-coordinate-values\"></span>",
                _target.CoordinateDisplay("NotRequiredCoordinateId").ToString());
        }

        [TestMethod]
        public void TestCoordinateDisplayForWorksCorrectlyWhenTheModelIsACoordinateInstanceAndNotItsId()
        {
            _model.DisplayCoordinate = _coordinate;

            Assert.AreEqual("<img class=\"coordinate-display-icon\" coordinateUrl=\"/Coordinate/Show/123\" src=\"/Content/images/map-icon-blue.png\" /><span class=\"cp-coordinate-values\">12.345, 23.456</span>",
                _target.CoordinateDisplayFor(f => f.DisplayCoordinate).ToString());

            Assert.AreEqual("<img class=\"coordinate-display-icon\" coordinateUrl=\"/Coordinate/Show/123\" src=\"/Content/images/map-icon-blue.png\" /><span class=\"cp-coordinate-values\">12.345, 23.456</span>",
                _target.CoordinateDisplay("DisplayCoordinate").ToString());
        }

        #endregion

        #region EditorForEquipmentCharacteristicField

        [TestMethod]
        public void TestStringFieldGetsRenderedForStringInput()
        {
            var fieldType = new EquipmentCharacteristicFieldType {
                DataType = "SomeDataType"
            }; 
            var field = new EquipmentCharacteristicField() {
                FieldType = fieldType,
                FieldName = "SomeField"
            };

            Assert.AreEqual(string.Format("<input data-val=\"true\" id=\"{0}\" name=\"{1}\" type=\"text\" value=\"\" />", _target.Id(field.FieldName), _target.Name(field.FieldName)),
                _target.EditorForEquipmentCharacteristicField(field, new List<EquipmentCharacteristic>()).ToString());
        }

        #endregion

        [TestMethod]
        public void TestCurrentUserIsEmployeeReturnsCorrectlyForEmployeeUser()
        {
            var user = new User();
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);
            var employee = new Employee {User = user};

            Assert.IsTrue(_target.CurrentUserIsEmployee(employee));

            _authenticationService.Setup(x => x.CurrentUser).Returns(new User());

            Assert.IsFalse(_target.CurrentUserIsEmployee(employee));
        }
    }

    public class CoordinatePickerTestModel
    {
        #region Properties

        [Required]
        public int? RequiredCoordinateId { get; set; }
        public int? NotRequiredCoordinateId { get; set; }

        [Coordinate(IconSet = IconSets.Pins)]
        public int? AttributedCoordinate { get; set; }

        public Coordinate DisplayCoordinate { get; set; }

        [Coordinate(AddressField = "SomeAddressField")]
        public int? WithAddressField { get; set; }

        [Coordinate(AddressCallback = "SomeAddressCallback")]
        public int? WithAddressCallback { get; set; }

        #endregion
    }
}
