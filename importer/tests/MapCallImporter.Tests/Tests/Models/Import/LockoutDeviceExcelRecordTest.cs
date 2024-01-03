using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallImporter.Tests.Models.Import
{
    public class
        LockoutDeviceExcelRecordTest : ExcelRecordTestBase<LockoutDevice, MyCreateLockoutDevice,
            LockoutDeviceExcelRecord>
    {
        protected override LockoutDeviceExcelRecord CreateTarget()
        {
            return new LockoutDeviceExcelRecord {
                OperatingCenter = "Pittsburgh",
                Person = "Anthony Kenney",
                SerialNumber = "3539AD",
                Description = "Kenney - Lock #1",
                LockoutDeviceColor = "Red"
            };
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<LockoutDevice, MyCreateLockoutDevice, LockoutDeviceExcelRecord> test)
        {
            test.RequiredString(x => x.OperatingCenter,
                x => x.OperatingCenter.Id);
            test.RequiredString(x => x.Person,
                x => x.Person.Id);
            test.String(x => x.SerialNumber, x => x.SerialNumber);
            test.String(x => x.Description, x => x.Description);
            test.RequiredString(x => x.LockoutDeviceColor, x => x.LockoutDeviceColor.Id);
        }

        #region OperatingCenter

        [TestMethod]
        public void TestOperatingCenterMapsFromString()
        {
            WithUnitOfWork(uow => {
                Assert.IsTrue(_target.OperatingCenter.StartsWith(_target
                                                                .MapToEntity(uow, 1, MappingHelper).OperatingCenter
                                                                .OperatingCenterCode));
            });
        }

        [TestMethod]
        public void TestThrowsWhenOperatingCenterCannotBeFoundInDatabase()
        {
            _target.OperatingCenter = "MI666 - this is not a real operating center";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenOperatingCenterCannotBeParsed()
        {
            foreach (var value in new[] { "blah", "blah - blah" })
            {
                _target.OperatingCenter = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        [TestMethod]
        public void TestThrowsWhenOperatingCenterNotProvided()
        {
            foreach (var value in new[] { null, " ", string.Empty })
            {
                _target.OperatingCenter = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region Person

        [TestMethod]
        public void TestPersonMapsFromString()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual("Anthony Kenney", _target.MapToEntity(uow, 1, MappingHelper).Person.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenPersonCannotBeFoundInDatabase()
        {
            _target.Person = "NOT A REAL PERSON";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion

        #region LockoutDeviceColor

        [TestMethod]
        public void TestLockoutDeviceColorMapsFromString()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual("Red", _target.MapToEntity(uow, 1, MappingHelper).LockoutDeviceColor.Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenLockoutDeviceColorCannotBeFoundInDatabase()
        {
            _target.Person = "NOT A REAL COLOR";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        #endregion
    }
}
