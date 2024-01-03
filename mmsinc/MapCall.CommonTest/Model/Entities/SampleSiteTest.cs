using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class SampleSiteTest : InMemoryDatabaseTest<SampleSite>
    {
        #region Fields

        private SampleSite _target;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject(_dateTimeProvider.Object);
            _target = new SampleSite {
                DateTimeProvider = _dateTimeProvider.Object
            };
        }

        #endregion

        [TestMethod]
        public void TestLongDescriptionReturnsRidiculouslyLongDescription()
        {
            var target = new SampleSite();

            // Make sure all nulls doesn't die
            Assert.AreEqual("------Non-Bacti-", target.LongDescription);

            var operatingCenter = new OperatingCenter {OperatingCenterCode = "NJ7"};
            var publicWaterSupply = new PublicWaterSupply {Identifier = "11-1"};
            var county = new County {Name = "Hell"};
            var town = new Town {ShortName = "Foodtown"};
            var status = new SampleSiteStatus {Description = "inactive"};

            target = new SampleSite {
                OperatingCenter = operatingCenter,
                PublicWaterSupply = publicWaterSupply,
                County = county,
                Town = town,
                CommonSiteName = "SSN",
                StreetNumber = "123 Easy St",
                BactiSite = false,
                Status = status
            };

            Assert.AreEqual(
                string.Format(SampleSite.LONG_DESCRIPTION_FORMAT_STRING,
                    operatingCenter.OperatingCenterCode,
                    publicWaterSupply.Identifier,
                    county.Name,
                    town.ShortName,
                    target.CommonSiteName,
                    target.StreetNumber,
                    "Non-Bacti",
                    status
                ),
                target.LongDescription);
        }

        [TestMethod]
        public void TestIconReturnsNullIfCoordinateIsNull()
        {
            Assert.IsNull(new SampleSite().Icon);
        }

        [TestMethod]
        public void TestIconReturnsIconFromCoordinateIfNotLeadCopperSite()
        {
            var icon = new MapIcon();
            var coordinate = new Coordinate {Icon = icon};

            Assert.AreSame(icon, new SampleSite {Coordinate = coordinate}.Icon);
        }

        [TestMethod]
        public void TestIconReturnsIconFromCoordinateIfLeadCopperSiteButStatusIsNotSet()
        {
            var icon = new MapIcon();
            var coordinate = new Coordinate {Icon = icon};

            Assert.AreSame(icon, new SampleSite {Coordinate = coordinate, LeadCopperSite = true}.Icon);
        }
        
        [TestMethod]
        public void TestIconReturnsCorrectIconForCorrectStatusIfLeadCopperSite()
        {
            var expected = new Dictionary<int, string> {
                {SampleSiteStatus.Indices.ACTIVE, "green"},
                {SampleSiteStatus.Indices.INACTIVE, "black"},
                {SampleSiteStatus.Indices.PENDING, "red"}
            };
            var set = new IconSet {
                Icons = new List<MapIcon>()
            };
            foreach (var item in expected)
            {
                set.Icons.Add(new MapIcon {FileName = item.Value});
            }
            set.Icons.Add(new MapIcon {FileName = "yellow"});

            // we don't want to create the icon again, but we want to test this one
            expected.Add(SampleSiteStatus.Indices.ARCHIVED_DUPLICATE_SITE, "red");

            var repository = new Mock<IIconSetRepository>();
            repository.Setup(x => x.Find(IconSets.Beakers)).Returns(set);
            _container.Inject(repository.Object);

            foreach (var item in expected)
            {
                var actual = new SampleSite {
                    Coordinate = new Coordinate(),
                    LeadCopperSite = true,
                    Status = new SampleSiteStatus {Id = item.Key}
                };
                _container.BuildUp(actual);
                Assert.AreSame(set.Icons.Single(i => i.FileName == item.Value), actual.Icon);
            }

            // alternate site, we want to show yellow instead of green or red.
            foreach (var item in expected)
            {
                var actual = new SampleSite {
                    Coordinate = new Coordinate(),
                    LeadCopperSite = true,
                    IsAlternateSite = true,
                    Status = new SampleSiteStatus {Id = item.Key}
                };
                _container.BuildUp(actual);
                Assert.AreEqual(
                    item.Value == "green" || item.Value == "red" ? "yellow" : set.Icons.Single(i => i.FileName == item.Value).FileName,
                    actual.Icon.FileName);
            }
        }

        [TestMethod]
        public void TestAllTheCodesReturnsCorrectCodes()
        {
            var sideMaterial = new ServiceMaterial {
                Code = "SI"
            };

            var service = new Service { 
                ServiceMaterial = new ServiceMaterial {
                    Code = "SM"
                }
            };

            var target = new SampleSite {
                CustomerPlumbingMaterial = new ServiceMaterial {
                    Code = "PM"
                },
                Premise = new Premise {
                    MostRecentService = new MostRecentlyInstalledService {
                        Service = service
                    } 
                }
            };

            Assert.AreEqual("SM,,PM", target.AllTheCodes);

            service.CustomerSideMaterial = sideMaterial;
            Assert.AreEqual("SM,SI,PM", target.AllTheCodes);
        }

        [TestMethod]
        public void TestIsRegularSiteReturnsExpectedValue()
        {
            var target = new SampleSite {
                IsAlternateSite = false
            };

            Assert.IsTrue(target.IsRegularSite);

            target.IsAlternateSite = false;
            Assert.IsTrue(target.IsRegularSite);
        }

        [TestMethod]
        public void TestCanBeCertifiedReturnsExpectedValue()
        {
            _target.CertifiedDate = null;
            Assert.IsTrue(_target.CanBeCertified,
                "Sample sites can be certified if they do not have a certified date.");

            var jan1st2020 = new DateTime(2020, 1, 1);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(jan1st2020);

            _target.CertifiedDate =
                new DateTime(2017, 1, 2); // Sets the value so we're one day off from three years ago.
            Assert.IsFalse(_target.CanBeCertified,
                "Sample sites can not be certified again until three years after the last certification.");

            _target.CertifiedDate = new DateTime(2017, 1, 1); // Sets the value so we're exactly three years ago. 
            Assert.IsTrue(_target.CanBeCertified,
                "Sample sites can be certified three years after the last certification.");
        }

        [TestMethod]
        public void TestNextCertificationDateReturnsExpectedValue()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);

            _target.CertifiedDate = null;
            Assert.AreEqual(now, _target.NextCertificationDate,
                "NextCertificationDate should be now when sample site has not been certified yet.");

            var expectedNext = now.AddYears(3).Date; // time is stripped off
            _target.CertifiedDate = now;
            Assert.AreEqual(expectedNext, _target.NextCertificationDate,
                "Next certification date should be three years after previous certification.");
        }

        [TestMethod]
        public void TestNextCertificationDateIgnoresTimes()
        {
            var certDate = new DateTime(1984, 4, 24, 4, 0, 4);
            var expected = new DateTime(1987, 4, 24);

            _target.CertifiedDate = certDate;
            Assert.AreEqual(expected, _target.NextCertificationDate,
                "NextCertificationDate should be three years after but with the time stripped off.");
        }
    }
}
