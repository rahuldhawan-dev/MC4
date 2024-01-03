var EquipmentCharacteristicField = {
  SELECTORS: {
    addFieldButton: '#addFieldButton',
    cancelAddFieldButton: '#cancelAddFieldButton',
    addFieldWrap: '#addEquipmentPurposeFieldWrap',
    characteristicFieldTable: '#characteristicFieldTable',
    fieldType: '#FieldType',
    createForm: '#createEquipmentCharacteristicForm',
    dropDownValues: 'div#DropDownValues'
  },

  initialize: function () {
    $(EquipmentCharacteristicField.SELECTORS.addFieldWrap).hide();
    EquipmentCharacteristicField.getDropDownValuesRow().hide();
    EquipmentCharacteristicField.initEvents();
  },

  initEvents: function() {
    $(EquipmentCharacteristicField.SELECTORS.addFieldButton).click(EquipmentCharacteristicField.addFieldButton_click);
    $(EquipmentCharacteristicField.SELECTORS.cancelAddFieldButton).click(EquipmentCharacteristicField.cancelAddFieldButton_click);
    $(EquipmentCharacteristicField.SELECTORS.characteristicFieldTable).find('form').submit(EquipmentCharacteristicField.deleteForm_submit);
    $(EquipmentCharacteristicField.SELECTORS.fieldType).change(EquipmentCharacteristicField.fieldType_change);
  },

  getDropDownValuesRow: function() {
    return $('label[for="DropDownValues"]').parent().parent();
  },

  getSelectedFieldType: function() {
    return $('option:selected', EquipmentCharacteristicField.SELECTORS.fieldType).text();
  },

  validateDropDownValues: function() {
    return ('DropDown' != EquipmentCharacteristicField.getSelectedFieldType() ||
      $(EquipmentCharacteristicField.SELECTORS.dropDownValues).multiinput('getCount'));
  },

  addFieldButton_click: function() {
    $(EquipmentCharacteristicField.SELECTORS.addFieldWrap).show();
    $(EquipmentCharacteristicField.SELECTORS.addFieldButton).hide();
  },

  cancelAddFieldButton_click: function() {
    $(EquipmentCharacteristicField.SELECTORS.addFieldWrap).hide();
    $(EquipmentCharacteristicField.SELECTORS.addFieldButton).show();
  },

  deleteForm_submit: function() {
    return confirm('Are you sure you wish to delete this field?');
  },

  fieldType_change: function(e) {
    EquipmentCharacteristicField.getDropDownValuesRow()
      .toggle('DropDown' == EquipmentCharacteristicField.getSelectedFieldType());
  }
};

$(document).ready(EquipmentCharacteristicField.initialize);