using System;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Models.Import.Equipment;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ExpressionExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;

namespace MapCallImporter.Library.Testing
{
    public class EquipmentCharacteristicMappingTester<TExcelRecord>
        where TExcelRecord : EquipmentExcelRecordBase<TExcelRecord>, new()
    {
        #region Properties

        public EquipmentExcelRecordTestBase<TExcelRecord> Test { get; }
        public TExcelRecord Target { get; }
        public ExcelRecordMappingTester<Equipment, MyCreateEquipment, TExcelRecord> MappingTester { get; }


        #endregion

        #region Constructors

        public EquipmentCharacteristicMappingTester(EquipmentExcelRecordTestBase<TExcelRecord> equipmentExcelRecordTestBase, ExcelRecordMappingTester<Equipment, MyCreateEquipment, TExcelRecord> mappingTester, TExcelRecord target)
        {
            MappingTester = mappingTester;
            Test = equipmentExcelRecordTestBase;
            Target = target;
        }

        #endregion

        #region Private Methods

        protected void OptionalValue(Expression<Func<TExcelRecord, string>> memberFn, string fieldName, string value, string expectedValue)
        {
            MappingTester.TestedElsewhere(memberFn);
            var prop = memberFn.SetProperty(Target, value);

            Test.WithUnitOfWork(uow => {
                var result = Target.MapToEntity(uow, 1, Test.MappingHelper);
                var characteristic = result.Characteristics
                    .SingleOrDefault(c => c.Field.FieldName == fieldName);
                
                Assert.IsNotNull(characteristic, $"No Characteristic '{fieldName}' was created for property '{prop.Name}'.");
                Assert.AreEqual(expectedValue, characteristic.Value);
            });
        }

        #endregion

        #region Exposed Methods

        public void Numerical(Expression<Func<TExcelRecord, string>> memberFn, string fieldName)
        {
            var value = "100.00 meh";

            OptionalValue(memberFn, fieldName, value, value.ReplaceRegex(@"^(\d+(?:\.\d+)?)[^\d]*.*", "$1"));
        }

        public void Currency(Expression<Func<TExcelRecord, string>> memberFn, string fieldName)
        {
            Numerical(memberFn, fieldName);
        }

        public void String(Expression<Func<TExcelRecord, string>> memberFn, string fieldName)
        {
            var value = "foo";

            OptionalValue(memberFn, fieldName, value, value);
        }

        public void DropDown(Expression<Func<TExcelRecord, string>> memberFn, string fieldName, (string Value, int Id) useThese = default((string Value, int Id)))
        {
            string value = null;
            var id = -1;
            if (useThese.Id == default(int))
            {
                var equipmentTypeId = (int)Target.GetHiddenPropertyValueByName("EquipmentTypeId");

                Test.WithUnitOfWork(uow => {
                    var dropDownValues = uow.Where<EquipmentCharacteristicDropDownValue>(v =>
                        v.Field.EquipmentType.Id == equipmentTypeId && v.Field.FieldName == fieldName);

                    if (!dropDownValues.Any())
                    {
                        Assert.Fail(
                            $"No {nameof(EquipmentCharacteristicDropDownValue)}s exist with EquipmentTypeId {equipmentTypeId} and FieldName {fieldName}");
                    }

                    var dropDownValue = dropDownValues.First();

                    value = dropDownValue.Value;
                    id = dropDownValue.Id;
                });
            }
            else
            {
                id = useThese.Id;
                value = useThese.Value;
            }

            OptionalValue(memberFn, fieldName, value, id.ToString());
        }

        public void Date(Expression<Func<TExcelRecord, string>> memberFn, string fieldName)
        {
            var date = new DateTime(2018, 2, 22);

            OptionalValue(memberFn, fieldName, date.ToString(), date.ToString());
        }

        public void NotMapped<TVal>(Expression<Func<TExcelRecord, TVal>> memberFn)
        {
            MappingTester.NotMapped(memberFn);
        }

        #endregion
    }
}