// If you move this script to the Bonds subfolder, it will be automatically loaded
// and have initialize called by the ClientScriptManager. Which is awesome except that
// jquery hasn't been loaded yet so it dies. TODO: Fight with this when you want to

$(document).ready(function() { Bonds.initialize(); });

var Bonds = {
  stateId: 0,
  countyId: 0,
  municipalityId: 0,
  initialize: function () {
    getServerElementById('ddlBondPurpose').change(function () {
      getServerElementById('ddlState').change();
      getServerElementById('ddlCounty').change();
      getServerElementById('ddlTown').change();
    });
  },
  validateAll: function () {
    getServerElementById('ddlState').change();
  },
  // State
  validateState: function (sender, args) {
    var name = $('#' + sender.controltovalidate).children('option').filter(':selected').text();
    if (getServerElementById('ddlBondPurpose').val() != 3)
      return;
    Bonds.setState(name);
    args.IsValid = (Bonds.stateId > 0);
  },
  setState: function (name) {
    $.ajax({
      async: false,
      type: 'POST',
      contentType: 'application/json; charset=utf-8',
      data: '{"name" : "' + name + '"}',
      url: '../../Data/Permits.asmx/GetStateId',
      dataType: 'json',
      success: function (msg) {
        Bonds.setStateId(msg.d);
      }
    });
  },
  setStateId: function (msg) {
    Bonds.stateId = (msg) ? msg.Id : 0;
  },
  // County
  validateCounty: function (sender, args) {
    if (Bonds.stateId == 0)
      Bonds.setState(getServerElementById("ddlState").children('option').filter(':selected').text());
    var name = $('#' + sender.controltovalidate).children('option').filter(':selected').text();
    Bonds.setCounty(name);
    args.IsValid = (Bonds.countyId > 0 || getServerElementById('ddlBondPurpose').val() != 3);
  },
  setCounty: function (name) {
    $.ajax({
      async: false,
      type: 'POST',
      contentType: 'application/json; charset=utf-8',
      data: '{"name" : "' + name + '", "stateId" : "' + Bonds.stateId + '" }',
      url: '../../Data/Permits.asmx/GetCountyId',
      dataType: 'json',
      success: function (msg) {
        Bonds.setCountyId(msg.d);
      }
    });
  },
  setCountyId: function (msg) {
    Bonds.countyId = (msg) ? msg.Id : 0;
  },
  //Municipality
  validateMunicipality: function (sender, args) {
    if (Bonds.stateId == 0)
      Bonds.setState(getServerElementById("ddlState").children('option').filter(':selected').text());
    if (Bonds.countyId == 0)
      Bonds.setCounty(getServerElementById("ddlCounty").children('option').filter(':selected').text());
    var name = $('#' + sender.controltovalidate).children('option').filter(':selected').text();
    Bonds.setMunicipality(name);
    args.IsValid = (Bonds.municipalityId > 0 || getServerElementById('ddlBondPurpose').val() != 3);
  },
  setMunicipality: function (name) {
    $.ajax({
      async: false,
      type: 'POST',
      contentType: 'application/json; charset=utf-8',
      data: '{"name" : "' + name + '", "countyId" : "' + Bonds.countyId + '" }',
      url: '../../Data/Permits.asmx/GetMunicipalityId',
      dataType: 'json',
      success: function (msg) {
        Bonds.setMunicipalityId(msg.d);
      }
    });
  },
  setMunicipalityId: function (msg) {
    Bonds.municipalityId = (msg) ? msg.Id : 0;
  }
};

