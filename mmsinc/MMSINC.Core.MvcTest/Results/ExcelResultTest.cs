using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Utilities.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Container = StructureMap.Container;
using IContainer = StructureMap.IContainer;

namespace MMSINC.Core.MvcTest.Results
{
    [TestClass]
    public class ExcelResultTest
    {
        #region Fields

        private ExcelResult _target;
        private List<Model> _models;
        private FakeMvcHttpHandler _pipeline;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container();
            _pipeline = new FakeMvcHttpHandler(_container);
            _models = new List<Model>();
            _models.Add(new Model());
            _target = CreateTarget();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            // Make sure we don't have a ton of temp files building up.
            File.Delete(_target.FileName);
        }

        #endregion

        #region Private Methods

        private ExcelResult CreateTarget()
        {
            return new ExcelResult("some name") {
                IsInTestMode = true
            };
        }

        private ExcelResultTester CreateTester()
        {
            return new ExcelResultTester(_container, _target);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConstructorDefaultsToExcel2003Format()
        {
            _target = new ExcelResult();
            Assert.AreEqual(ExcelFormat.Excel2003, _target.Format);
        }

        [TestMethod]
        public void TestConstructorAddsExcel2007ExtensionToFileName()
        {
            _target = new ExcelResult("blah");
            Assert.AreEqual("blah.xlsx", _target.FileDownloadName);
        }

        [TestMethod]
        public void TestAddSheetReturnsExcelResultInstanceAllFluently()
        {
            Assert.AreSame(_target, _target.AddSheet(_models, new ExcelExportSheetArgs {SheetName = "blah"}));
        }

        [TestMethod]
        public void TestAddSheetAddsSheetToExporter()
        {
            _target.AddSheet(_models, new ExcelExportSheetArgs {SheetName = "blah"});
            var sheet = (IExcelSheet<Model>)_target.Exporter.Sheets.Single();
            var result = sheet.Items;

            // Internally, the TypedSheet class copies the items to a new list
            // so we need to check for equality of items instead of if the list
            // is the same instance of our list.
            Assert.AreEqual(_models.Count, result.Count());
            Assert.AreSame(_models.Single(), result.Single());
        }

        [TestMethod]
        public void TestAddSheetWithNullSheetNameAutoGeneratesUniqueSheetName()
        {
            _target.AddSheet(_models);
            Assert.AreEqual("Sheet1", _target.Exporter.Sheets.Last().Name);
            _target.AddSheet(_models);
            Assert.AreEqual("Sheet2", _target.Exporter.Sheets.Last().Name);
        }

        [TestMethod]
        public void TestExportExports()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();

            var expectedModel = _models.First();
            expectedModel.BoolProp = true;
            expectedModel.DateTimeProp = new DateTime(2012, 12, 31);
            expectedModel.DecimalProp = (Decimal)12.3422322;
            expectedModel.DoubleProp = 23.45;
            expectedModel.FloatProp = (float)1.45;
            expectedModel.IntProp = int.MaxValue;
            expectedModel.StringProp = "neat";
            expectedModel.ObjectProperty = new SomeObject {Property = "I am a property value."};
            expectedModel.IntSixtyFour = (Int64)9999999999;

            _target.AddSheet(_models, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);
            _pipeline.Response.Verify(x => x.TransmitFile(_target.FileName));

            using (var excelTester = CreateTester())
            {
                excelTester.AreEqual(expectedModel.BoolProp, "BoolProp", 0);
                excelTester.AreEqual(expectedModel.DateTimeProp, "DateTimeProp", 0);
                excelTester.AreEqual(expectedModel.DecimalProp, "DecimalProp", 0);
                excelTester.AreEqual(expectedModel.DoubleProp, "DoubleProp", 0);
                excelTester.AreEqual(expectedModel.FloatProp, "FloatProp", 0);
                excelTester.AreEqual(expectedModel.IntProp, "IntProp", 0);
                excelTester.AreEqual(expectedModel.StringProp, "StringProp", 0);
                excelTester.AreEqual(expectedModel.ObjectProperty.Property, "ObjectProperty", 0);
                excelTester.AreEqual(expectedModel.IntSixtyFour, "IntSixtyFour", 0);
            }
        }

        #region Dictionary-specific exporting

        [TestMethod]
        public void TestExportExportsFromDictionaries()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();

            var expectedDict = new Dictionary<string, object>();
            expectedDict["BoolProp"] = true;
            expectedDict["DateTimeProp"] = new DateTime(2012, 12, 31);
            expectedDict["DecimalProp"] = (Decimal)12.3422322;
            expectedDict["DoubleProp"] = 23.45;
            expectedDict["FloatProp"] = (float)1.45;
            expectedDict["IntProp"] = int.MaxValue;
            expectedDict["StringProp"] = "neat";
            expectedDict["IntSixtyFour"] = (Int64)9999999999;

            _target.AddSheet(new[] {expectedDict}, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);
            _pipeline.Response.Verify(x => x.TransmitFile(_target.FileName));

            using (var excelTester = CreateTester())
            {
                excelTester.AreEqual(true, "BoolProp", 0);
                excelTester.AreEqual(new DateTime(2012, 12, 31), "DateTimeProp", 0);
                excelTester.AreEqual((Decimal)12.3422322, "DecimalProp", 0);
                excelTester.AreEqual(23.45, "DoubleProp", 0);
                excelTester.AreEqual((float)1.45, "FloatProp", 0);
                excelTester.AreEqual(int.MaxValue, "IntProp", 0);
                excelTester.AreEqual("neat", "StringProp", 0);
                excelTester.AreEqual((Int64)9999999999, "IntSixtyFour", 0);
                // NOTE: "ObjectProperty" isn't tested here as DictionarySheet assumes everything is a flat value.
            }
        }

        [TestMethod]
        public void
            TestExportingFromDictionariesGetsExpectedTypeConversionsWhenThereAreMultipleRecordsAndSomeHaveNullValues()
        {
            // The value type information has to be retrieved from the value since they can't be 
            // associated with a Type.PropertyInfo for them. This will only work when there's atleast
            // one row that has a non-null value for the column.

            var controller = _pipeline.CreateAndInitializeController<FakeController>();

            var expectedDictWithoutValues = new Dictionary<string, object>();
            expectedDictWithoutValues["BoolProp"] = null;
            expectedDictWithoutValues["DateTimeProp"] = null;
            expectedDictWithoutValues["DecimalProp"] = null;
            expectedDictWithoutValues["DoubleProp"] = null;
            expectedDictWithoutValues["FloatProp"] = null;
            expectedDictWithoutValues["IntProp"] = null;
            expectedDictWithoutValues["StringProp"] = null;
            expectedDictWithoutValues["IntSixtyFour"] = null;
            expectedDictWithoutValues["SomethingThatIsAlwaysNull"] = null;

            var expectedDict = new Dictionary<string, object>();
            expectedDict["BoolProp"] = true;
            expectedDict["DateTimeProp"] = new DateTime(2012, 12, 31);
            expectedDict["DecimalProp"] = (Decimal)12.3422322;
            expectedDict["DoubleProp"] = 23.45;
            expectedDict["FloatProp"] = (float)1.45;
            expectedDict["IntProp"] = int.MaxValue;
            expectedDict["StringProp"] = "neat";
            expectedDict["IntSixtyFour"] = (Int64)9999999999;
            expectedDict["SomethingThatIsAlwaysNull"] = null;

            _target.AddSheet(new[] {expectedDictWithoutValues, expectedDict},
                new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);
            _pipeline.Response.Verify(x => x.TransmitFile(_target.FileName));

            using (var excelTester = CreateTester())
            {
                excelTester.IsNull("BoolProp", 0);
                excelTester.IsNull("DateTimeProp", 0);
                excelTester.IsNull("DecimalProp", 0);
                excelTester.IsNull("DoubleProp", 0);
                excelTester.IsNull("FloatProp", 0);
                excelTester.IsNull("IntProp", 0);
                excelTester.IsNull("StringProp", 0);
                excelTester.IsNull("IntSixtyFour", 0);
                excelTester.IsNull("SomethingThatIsAlwaysNull", 0);

                excelTester.AreEqual(true, "BoolProp", 1);
                excelTester.AreEqual(new DateTime(2012, 12, 31), "DateTimeProp", 1);
                excelTester.AreEqual((Decimal)12.3422322, "DecimalProp", 1);
                excelTester.AreEqual(23.45, "DoubleProp", 1);
                excelTester.AreEqual((float)1.45, "FloatProp", 1);
                excelTester.AreEqual(int.MaxValue, "IntProp", 1);
                excelTester.AreEqual("neat", "StringProp", 1);
                excelTester.AreEqual((Int64)9999999999, "IntSixtyFour", 1);
                excelTester.IsNull("SomethingThatIsAlwaysNull", 1);
                // NOTE: "ObjectProperty" isn't tested here as DictionarySheet assumes everything is a flat value.
            }
        }

        [TestMethod]
        public void TestExportExportsFromDictionariesWhenThereAreNoRows()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();

            _target.AddSheet(new Dictionary<string, object>[] { }, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);
            _pipeline.Response.Verify(x => x.TransmitFile(_target.FileName));

            using (var excelTester = CreateTester())
            {
                Assert.AreEqual(0, excelTester.GetRowCount("blah"));
            }
        }

        [TestMethod]
        public void TestExportExportsFromDictionariesWhenDictionariesHaveDifferentSetsOfKeys()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();

            var expectedDict = new Dictionary<string, object>();
            expectedDict["BoolProp"] = true;
            expectedDict["AllDictsHaveThisIntProp"] = 42;

            var expectedDict2 = new Dictionary<string, object>();
            expectedDict2["SomeOtherBoolProp"] = false;
            expectedDict2["AllDictsHaveThisIntProp"] = 43;

            var expectedDict3 = new Dictionary<string, object>();
            expectedDict2["YetAnotherBoolProp"] = false;
            expectedDict3["AllDictsHaveThisIntProp"] = 44;

            _target.AddSheet(new[] {expectedDict, expectedDict2, expectedDict3},
                new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);
            _pipeline.Response.Verify(x => x.TransmitFile(_target.FileName));

            using (var excelTester = CreateTester())
            {
                excelTester.AreEqual(true, "BoolProp", 0);
                excelTester.IsNull("BoolProp", 1);
                excelTester.IsNull("BoolProp", 2);

                excelTester.AreEqual(42, "AllDictsHaveThisIntProp", 0);
                excelTester.AreEqual(43, "AllDictsHaveThisIntProp", 1);
                excelTester.AreEqual(44, "AllDictsHaveThisIntProp", 2);

                excelTester.IsNull("SomeOtherBoolProp", 0);
                excelTester.AreEqual(false, "SomeOtherBoolProp", 1);
                excelTester.IsNull("SomeOtherBoolProp", 2);
            }
        }

        #endregion

        [TestMethod]
        public void TestExportDoesNotExportIEnumerableProperties()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();

            var expectedModel = _models.First();
            expectedModel.EnumerableProperty = new List<string>();
            _target.AddSheet(_models, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);
            _pipeline.Response.Verify(x => x.TransmitFile(_target.FileName));

            using (var excelTester = new ExcelResultTester(new StructureMap.Container(), _target))
            {
                excelTester.DoesNotContainColumn("EnumerableProperty");
            }
        }

        [TestMethod]
        public void TestExportDoesNotScrewUpDecimalAccuracyAndPrecision()
        {
            var decimals = new[] {
                (decimal)2,
                (decimal)1.23,
                (decimal)1.24134,
                (decimal)3258235702352.2532582351818537857913791359132597,
                (decimal)2.0000000000000000000000000
            };

            foreach (var expectedDecimal in decimals)
            {
                _target = CreateTarget();
                var controller = _pipeline.CreateAndInitializeController<FakeController>();

                var expectedModel = _models.First();
                expectedModel.DecimalProp = expectedDecimal;
                _target.AddSheet(_models, new ExcelExportSheetArgs {SheetName = "blah"});
                _target.ExecuteResult(controller.ControllerContext);
                _pipeline.Response.Verify(x => x.TransmitFile(_target.FileName));

                using (var excelTester = new ExcelResultTester(_container, _target))
                {
                    excelTester.AreEqual(expectedModel.DecimalProp, "DecimalProp", 0);
                }
            }
        }

        [TestMethod]
        public void TestExportReturnsNullsForNullObjectProperties()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();

            var expectedModel = _models.First();
            expectedModel.ObjectProperty = null;
            _target.AddSheet(_models, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);
            _pipeline.Response.Verify(x => x.TransmitFile(_target.FileName));

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.IsNull("ObjectProperty", 0);
            }
        }

        [TestMethod]
        public void TestExportCanExportNullableProperties()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();

            var nullables = new List<NullableModel>();
            nullables.Add(new NullableModel());

            var expectedModel = nullables.First();
            expectedModel.BoolProp = true;
            expectedModel.DateTimeProp = new DateTime(2012, 12, 31);
            expectedModel.DecimalProp = (Decimal)12.34;
            expectedModel.DoubleProp = 23.45;
            expectedModel.FloatProp = (float)1.45;
            expectedModel.IntProp = int.MaxValue;

            _target.AddSheet(nullables, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);
            _pipeline.Response.Verify(x => x.TransmitFile(_target.FileName));

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.AreEqual(expectedModel.BoolProp, "BoolProp", 0);
                excelTester.AreEqual(expectedModel.DateTimeProp, "DateTimeProp", 0);
                excelTester.AreEqual(expectedModel.DecimalProp, "DecimalProp", 0);
                excelTester.AreEqual(expectedModel.DoubleProp, "DoubleProp", 0);
                excelTester.AreEqual(expectedModel.FloatProp, "FloatProp", 0);
                excelTester.AreEqual(expectedModel.IntProp, "IntProp", 0);
            }
        }

        [TestMethod]
        public void TestExportStripsMillisecondsFromDateTime()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();

            var nullables = new List<NullableModel>();
            nullables.Add(new NullableModel());

            var dateWithTime = new DateTime(1984, 12, 31, 12, 0, 1, 1);
            var expectedDate = new DateTime(1984, 12, 31, 12, 0, 1);

            var expectedModel = nullables.First();
            expectedModel.DateTimeProp = dateWithTime;

            _target.AddSheet(nullables, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);
            _pipeline.Response.Verify(x => x.TransmitFile(_target.FileName));

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.AreEqual(expectedModel.DateTimeProp, "DateTimeProp", 0,
                    "The milliseconds have to be stripped because Excel 2003 can not handle it.");
            }
        }

        [TestMethod]
        public void TestExportCanExportNullablePropertiesWhenTheyAreActuallyNull()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();

            var nullables = new List<NullableModel>();
            nullables.Add(new NullableModel());

            var expectedModel = nullables.First();
            expectedModel.BoolProp = null;
            expectedModel.DateTimeProp = null;
            expectedModel.DecimalProp = null;
            expectedModel.DoubleProp = null;
            expectedModel.FloatProp = null;
            expectedModel.IntProp = null;

            _target.AddSheet(nullables, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);
            _pipeline.Response.Verify(x => x.TransmitFile(_target.FileName));

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.CopyFileTo(@"C:\Solutions\aaaaaaaa.xlsx");
                excelTester.IsNull("BoolProp", 0);
                excelTester.IsNull("DateTimeProp", 0);
                excelTester.IsNull("DecimalProp", 0);
                excelTester.IsNull("DoubleProp", 0);
                excelTester.IsNull("FloatProp", 0);
                excelTester.IsNull("IntProp", 0);
            }
        }

        [TestMethod]
        public void TestExportCanExportAnEmptyExcelFileWithProperColumnNames()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            _models.Clear();
            _target.AddSheet(_models, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);
            _pipeline.Response.Verify(x => x.TransmitFile(_target.FileName));

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                var columnNames = excelTester.GetColumnNamesAndIndex("blah").Keys.ToArray();
                Assert.IsTrue(columnNames.Contains("BoolProp"));
                Assert.IsTrue(columnNames.Contains("DateTimeProp"));
                Assert.IsTrue(columnNames.Contains("DecimalProp"));
                Assert.IsTrue(columnNames.Contains("DoubleProp"));
                Assert.IsTrue(columnNames.Contains("FloatProp"));
                Assert.IsTrue(columnNames.Contains("IntProp"));
                Assert.IsTrue(columnNames.Contains("StringProp"));
                Assert.IsTrue(columnNames.Contains("ObjectProperty"));
            }
        }

        [TestMethod]
        public void TestExportDoesNotExportPropertieseWithDoesNotExportAttribute()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var expectedModel = new ExportAttributeModel();
            var nullables = new List<ExportAttributeModel>();
            nullables.Add(expectedModel);

            _target.AddSheet(nullables, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.ContainsColumn("ExportableProperty");
                excelTester.DoesNotContainColumn("DoesNotExportProperty");
            }
        }

        [TestMethod]
        public void TestExportFlattensFlattenableProperties()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var expectedModel = new ExportAttributeModel {
                FlattenableProperty = new SomeObject {
                    Property = "SomeValue"
                }
            };
            var nullables = new List<ExportAttributeModel> {expectedModel};

            _target.AddSheet(nullables, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.ContainsColumn("FlattenedCustomColumnName");
                excelTester.AreEqual(expectedModel.FlattenableProperty.Property, "FlattenedCustomColumnName");
                excelTester.DoesNotContainColumn("FlattenedProperty");
            }
        }

        [TestMethod]
        public void TestExportFlattensMultipleFlattenableValuesOnSameProperty()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var expectedModel = new ExportAttributeModel {
                MultipleFlattenableProperties = new Model {
                    IntProp = 32,
                    StringProp = "Some String Value"
                }
            };
            var nullables = new List<ExportAttributeModel> {expectedModel};

            _target.AddSheet(nullables, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.ContainsColumn("IntProp");
                excelTester.AreEqual(expectedModel.MultipleFlattenableProperties.IntProp, "IntProp");
                excelTester.ContainsColumn("StringProp");
                excelTester.AreEqual(expectedModel.MultipleFlattenableProperties.StringProp, "StringProp");
            }
        }

        [TestMethod]
        public void TestExportReturnsNullForFlattenedPropertiesWhenIntermediatePropertyIsNull()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var expectedModel = new ExportAttributeModel {
                FlattenableProperty = null
            };
            var nullables = new List<ExportAttributeModel> {expectedModel};

            _target.AddSheet(nullables, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.ContainsColumn("FlattenedCustomColumnName");
                excelTester.IsNull("FlattenedCustomColumnName");
            }
        }

        [TestMethod]
        public void TestExportCanFlattenDeepProperties()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var expectedModel = new ExportAttributeModel {
                DeepFlattenableProperty = new SomeObject {
                    ModelChild = new Model {
                        StringProp = "Hurrah!"
                    }
                }
            };
            var nullables = new List<ExportAttributeModel> {expectedModel};

            _target.AddSheet(nullables, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.ContainsColumn("DeepStringProp");
                excelTester.AreEqual(expectedModel.DeepFlattenableProperty.ModelChild.StringProp, "DeepStringProp");
            }
        }

        [TestMethod]
        public void TestExportReturnsNullWhenAnIntemediatePropertyIsNullInADeepFlatteningProperties()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var expectedModel = new ExportAttributeModel {
                DeepFlattenableProperty = new SomeObject {
                    ModelChild = null
                }
            };
            var nullables = new List<ExportAttributeModel> {expectedModel};

            _target.AddSheet(nullables, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.ContainsColumn("DeepStringProp");
                excelTester.IsNull("DeepStringProp");
            }
        }

        [TestMethod]
        public void TestExportUsesDisplayNameAttributeOfPropertyForColumnName()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var expectedModel = new SomeObject();

            _target.AddSheet(new[] {expectedModel}, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.ContainsColumn("Display Name");
            }
        }

        [TestMethod]
        public void TestExportUsesFlattenAttributeColumnNameIfTheFlattenedPropertyHasADisplayNameAttribute()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var expectedModel = new ExportAttributeModel {
                FlattenablePropertyWithDisplayName = new SomeObject()
            };

            _target.AddSheet(new[] {expectedModel}, new ExcelExportSheetArgs {SheetName = "blah"});
            _target.ExecuteResult(controller.ControllerContext);

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.ContainsColumn("UseMyFlattenedColumnName");
            }
        }

        #region Exportable Properties

        [TestMethod]
        public void TestExportExportsAllPropertiesWhenExportablePropertiesArgumentIsNull()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var expectedModel = new ExportablePropertyModel();
            var data = new List<ExportablePropertyModel>();
            data.Add(expectedModel);

            _target.AddSheet(data, new ExcelExportSheetArgs {ExportableProperties = null});
            _target.ExecuteResult(controller.ControllerContext);

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.ContainsColumn("AlwaysExportableProperty");
                excelTester.ContainsColumn("SometimesExportableProperty");
                excelTester.DoesNotContainColumn("NeverExportableProperty");
            }
        }

        [TestMethod]
        public void TestExportExportsOnlyExportablePropertiesWhenExportablePropertiesHasValues()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var expectedModel = new ExportablePropertyModel();
            var data = new List<ExportablePropertyModel>();
            data.Add(expectedModel);

            _target.AddSheet(data,
                new ExcelExportSheetArgs {ExportableProperties = new string[] {"AlwaysExportableProperty"}});
            _target.ExecuteResult(controller.ControllerContext);

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.ContainsColumn("AlwaysExportableProperty");
                excelTester.DoesNotContainColumn("SometimesExportableProperty");
                excelTester.DoesNotContainColumn("NeverExportableProperty");
            }
        }

        [TestMethod]
        public void TestExportWithExportablePropertiesDoesNotIncludePropertiesWithDoesNotExport()
        {
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var expectedModel = new ExportablePropertyModel();
            var data = new List<ExportablePropertyModel>();
            data.Add(expectedModel);

            _target.AddSheet(data, new ExcelExportSheetArgs {
                ExportableProperties = new string[] {
                    "AlwaysExportableProperty",
                    "NeverExportableProperty" // This should never show up in the export because it has as DoesNotExport attribute.
                }
            });
            _target.ExecuteResult(controller.ControllerContext);

            using (var excelTester = new ExcelResultTester(_container, _target))
            {
                excelTester.ContainsColumn("AlwaysExportableProperty");
                excelTester.DoesNotContainColumn("SometimesExportableProperty");
                excelTester.DoesNotContainColumn("NeverExportableProperty");
            }
        }

        #endregion

        #endregion

        #region Helper classes

        private class Model
        {
            public string StringProp { get; set; }
            public int IntProp { get; set; }
            public DateTime DateTimeProp { get; set; }
            public decimal DecimalProp { get; set; }
            public double DoubleProp { get; set; }
            public bool BoolProp { get; set; }
            public float FloatProp { get; set; }
            public SomeObject ObjectProperty { get; set; }
            public Int64 IntSixtyFour { get; set; }

            public IEnumerable EnumerableProperty { get; set; }
        }

        private class NullableModel
        {
            public int? IntProp { get; set; }
            public DateTime? DateTimeProp { get; set; }
            public decimal? DecimalProp { get; set; }
            public double? DoubleProp { get; set; }
            public bool? BoolProp { get; set; }
            public float? FloatProp { get; set; }
        }

        private class SomeObject
        {
            public string Property { get; set; }
            public Model ModelChild { get; set; }

            [DisplayName("Display Name")]
            public string PropertyWithDisplayName { get; set; }

            public override string ToString()
            {
                return Property;
            }
        }

        private class ExportAttributeModel
        {
            public object ExportableProperty { get; set; }

            [DoesNotExport]
            public object DoesNotExportProperty { get; set; }

            [FlattenAtExport("Property", ColumnName = "FlattenedCustomColumnName")]
            public SomeObject FlattenableProperty { get; set; }

            [FlattenAtExport("PropertyWithDisplayName", "UseMyFlattenedColumnName")]
            public SomeObject FlattenablePropertyWithDisplayName { get; set; }

            [FlattenAtExport("IntProp")]
            [FlattenAtExport("StringProp")]
            public Model MultipleFlattenableProperties { get; set; }

            [FlattenAtExport("ModelChild.StringProp", "DeepStringProp")]
            public SomeObject DeepFlattenableProperty { get; set; }
        }

        private class ExportablePropertyModel
        {
            public object AlwaysExportableProperty { get; set; }

            public object SometimesExportableProperty { get; set; }

            [DoesNotExport]
            public object NeverExportableProperty { get; set; }
        }

        #endregion
    }
}
