using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using MapCall.Common.Model.Entities;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using MMSINC.Data.V2;

namespace MapCallImporter.Models.Import.Equipment
{
    public class EquipmentCharacteristicMapper
    {
        #region Private Members

        private static readonly ConcurrentDictionary<(int typeId, string value), EquipmentCharacteristicDropDownValue>
            _dropDownValueCache = new ConcurrentDictionary<(int typeId, string value), EquipmentCharacteristicDropDownValue>();

        private readonly IUnitOfWork _uow;
        private readonly int _index;
        private readonly MapCall.Common.Model.Entities.Equipment _equipment;
        private readonly ExcelRecordItemHelperBase<MapCall.Common.Model.Entities.Equipment> _helper;

        #endregion

        #region Constructors

        public EquipmentCharacteristicMapper(MapCall.Common.Model.Entities.Equipment equipment, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<MapCall.Common.Model.Entities.Equipment> helper)
        {
            _uow = uow;
            _index = index;
            _equipment = equipment;
            _helper = helper;
        }

        #endregion

        #region Exposed Methods

        public EquipmentCharacteristic CreateCharacteristic(string value, int typeId)
        {
            var field = _uow.Find<EquipmentCharacteristicField>(typeId);

            if (field == null)
            {
                _helper.AddFailure($"Row {_index}: Could not find EquipmentCharacteristicField with Id {typeId}.");
                return null;
            }

            return new EquipmentCharacteristic {
                Value = value,
                Field = field,
                Equipment = _equipment
            };
        }

        public EquipmentCharacteristic Numerical(string value, string fieldName, int typeId)
        {
            var numberRegex = new Regex("^(\\d+(:?\\.\\d+)?)[^\\d]*.*");
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!numberRegex.IsMatch(value))
            {
                _helper.AddFailure($"Could not convert {fieldName} value '{value}' at row {_index} to a number.");
                return null;
            }

            var formatted = numberRegex.Match(
                _helper.RequiredStringValue(value, _index, fieldName)
            ).Groups[1].Value;

            return CreateCharacteristic(formatted, typeId);
        }

        public EquipmentCharacteristic String(string value, string fieldName, int typeId)
        {
            return string.IsNullOrWhiteSpace(value) ? null : CreateCharacteristic(value, typeId);
        }

        public EquipmentCharacteristic DropDown(string value, string fieldName, int typeId)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var dropDownValue = _dropDownValueCache.GetOrAdd((typeId, value),
                k => _uow.Where<EquipmentCharacteristicDropDownValue>(v => v.Field.Id == k.typeId && v.Value == value)
                         .SingleOrDefault());

            if (dropDownValue == null)
            {
                _helper.AddFailure($"Could not find matching drop down value '{value}' for {fieldName} at row {_index}.");
                return null;
            }

            return CreateCharacteristic(dropDownValue.Id.ToString(), typeId);
        }

        public EquipmentCharacteristic Currency(string value, string fieldName, int typeId)
        {
            return string.IsNullOrWhiteSpace(value) ? null : Numerical(value, fieldName, typeId);
        }

        public EquipmentCharacteristic Date(string value, string fieldName, int typeId)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!DateTime.TryParse(_helper.RequiredStringValue(value, _index, fieldName), out var _))
            {
                _helper.AddFailure($"Could not convert {fieldName} value '{value}' at row {_index} to a date.");
            }

            return CreateCharacteristic(value, typeId);
        }

        #endregion
    }
}