// So these are nice and all but there are a few caveats with using them. 
// They set values on the parent form so those ids must match what this script is using.
// There's a trick where you can comma separate jquery ids in the selectors and it'll grab 
// whichever one it finds. Careful though if you have both, it'll set both.
// If it's not working in your form, make sure you're calling AjaxTable.initialize('#wbsElementTable');
// in your file.js initialize method.
var WBSElementFind = (function ($) {
  var ELEMENTS = {};
  var URLS = {};
  var WBSElementFindFormId = '#WBSElementForm';

  var wbsef = {
    initialize: function() {
      URLS = { wbsElementSearch: '../../SAP/WBSElement/Index.json' };
      ELEMENTS = {
        assetTypeSearch: $('#AssetType'),
        doSAPWBSElementSearch: $(WBSElementFindFormId + ' button[id=DoWBSElementSearch]'),
        isOpenSearch: $(WBSElementFindFormId + ' select[id=IsOpen]'),
        operatingCenterSearch: $('#OperatingCenter'),
        planningPlantSearch: $('#PlanningPlant'),
        projectTypeSearch: $(WBSElementFindFormId + ' select[id=SAPProjectType]'),
        projectDefinitionSearch: $(WBSElementFindFormId + ' input[id=ProjectDefinition]'),
        serviceCategorySearch: $('#ServiceCategory option:selected'),
        selectedWBSNumber: $(WBSElementFindFormId + ' '),
        selectWBSElement: $(WBSElementFindFormId + ' button[id=selectWBSElement]'),
        wbsElementResults: $(WBSElementFindFormId + ' table[id=wbsElementResults]'),
        wbsDescriptionSearch: $(WBSElementFindFormId + ' input[id=WBSDescription]'),
        wbsNumber: $('#AccountCharged, #WBSElement, #WBSNumber, #TaskNumber1'), // jquery kinda behave like coalesce, as long as they don't have both field names
        wbsNumberSearch: $(WBSElementFindFormId + ' input[id=WBSNumber]'),
        yearSearch: $(WBSElementFindFormId + ' input[id=Year]')
      };

      ELEMENTS.doSAPWBSElementSearch.on('click', wbsef.doSearch_click);
      ELEMENTS.selectWBSElement.on('click', wbsef.selectWBSElement_click);
      wbsef.setInitialValues();
    },

    doSearch_click: function () {
      if (ELEMENTS.operatingCenterSearch.val() === '') {
        alert('Please close the dialog and select an operating center first.');
        // we need to exit at this point, and we need to return false from this function in order to
        // prevent the browser's default behavior.  thus we have two `return false` statements in this
        // function.  not all code smells are actual issues.
        return false;
      }
      var assetTypeId = ELEMENTS.assetTypeSearch.val();
      if (assetTypeId === undefined && ELEMENTS.serviceCategorySearch.text().toUpperCase().indexOf('SEWER') > -1)
        assetTypeId = 6;
      ELEMENTS.wbsElementResults.find('tr:gt(0)').remove();
      $.getJSON(
        (window.location.pathname.toUpperCase().match(/\/NEW\/|\/EDIT\/|\/NEWFROMWORKORDER\//)) ? '../' + URLS.wbsElementSearch : URLS.wbsElementSearch,
        $.param({
          'WBSNumber': ELEMENTS.wbsNumberSearch.val(),
          'WBSDescription': ELEMENTS.wbsDescriptionSearch.val(),
          'Year': ELEMENTS.yearSearch.val(),
          'OperatingCenter': ELEMENTS.operatingCenterSearch.val(),
          'AssetType': assetTypeId,
          'SAPProjectType': ELEMENTS.projectTypeSearch.val(),
          'ProjectDefinition': ELEMENTS.projectDefinitionSearch.val(),
          'IsOpen': ELEMENTS.isOpenSearch.val(),
          'PlanningPlant': ELEMENTS.planningPlantSearch.val()
        }),
        WBSElementFind.search_callBack);
      return false;
    },

    resultsRow_click: function (e) {
      $(e.target.parentElement.parentElement).find('td').css('background-color', '');
      $(e.target.parentElement).find('td').css('background-color', 'orange');
      ELEMENTS.selectedWBSNumber.val(e.target.parentElement.cells[0].innerHTML);
    },

    resultsRow_doubleClick: function (e) {
      wbsef.resultsRow_click(e);
      wbsef.selectWBSElement_click();
    },

    search_callBack: function (d) {
      if (d.Data.length === 1 && d.Data[0].SAPErrorCode !== '' && d.Data[0].SAPErrorCode.indexOf('Successful') === -1) {
        ELEMENTS.wbsElementResults.find('tbody').append(
          '<tr>' +
          '<td colspan=4>' + d.Data[0].SAPErrorCode + '</td>' +
          '</tr>');
        return false;
      }
      for (var x = 0; x < d.Data.length; x++) {
          ELEMENTS.wbsElementResults.find('tbody').append(
          '<tr>' +
          '<td>' + d.Data[x].WBSNumber+ '</td>' +
          '<td>' + d.Data[x].WBSDescription+ '</td>' +
          '<td>' + d.Data[x].Status + '</td>' +
          '<td>' + d.Data[x].StartDate + '</td>' +
          '<td>' + d.Data[x].EndDate + '</td>' +
          '</tr>');
      }
      const rows = ELEMENTS.wbsElementResults.find('> tbody > tr');
      rows.on('click', wbsef.resultsRow_click);
      rows.on('dblclick', wbsef.resultsRow_doubleClick);
    },

    selectWBSElement_click: function() {
      if (ELEMENTS.selectedWBSNumber.val() !== '' && ELEMENTS.selectedWBSNumber.val() !== 'null') {
        wbsef.setParentValue(ELEMENTS.wbsNumber, ELEMENTS.selectedWBSNumber.val());
      };
      $(WBSElementFindFormId + ' button[class=cancel]').click();
    },

    setInitialValues:function() {
      ELEMENTS.wbsNumberSearch.val(ELEMENTS.wbsNumber.val());
      if (ELEMENTS.yearSearch.val() === '') ELEMENTS.yearSearch.val(new Date().getFullYear());
    },

    setParentValue: function (elem, val) {
      elem.val(val);
      elem.blur();
    }
  };
  $(document).ready(wbsef.initialize);
  return wbsef;
})(jQuery);