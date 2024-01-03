// So these are nice and all but there are a few caveats with using them. 
// They set values on the parent form so those ids must match what this script is using.
// There's a trick where you can comma separate jquery ids in the selectors and it'll grab 
// whichever one it finds. Careful though if you have both, it'll set both.
// If it's not working in your form, make sure you're calling AjaxTable.initialize('#sapFunctionalLocationTable');
// in your file.js initialize method.
var SAPFunctionalLocationFind = (function ($) {
    var ELEMENTS = {};
    var URLS = {};
    var SAPFunctionalLocationFindFormId = '#SAPFunctionalLocationForm';

    var sfl = {
        initialize: function () {
            URLS = { sapFunctionalLocationSearch: $('#SearchUrl').val() };
            ELEMENTS = {
                //controls
                doSAPSAPFunctionalLocationSearch: $(SAPFunctionalLocationFindFormId + ' button[id=DoSAPFunctionalLocationSearch]'),
                selectSAPFunctionalLocation: $(SAPFunctionalLocationFindFormId + ' button[id=selectSAPFunctionalLocation]'),
                //parent form search fields
                operatingCenterSearch: $('#OperatingCenter'),
                functionalLocation: $('#FunctionalLocation'),
                //search fields
                functionalLocationSearch: $(SAPFunctionalLocationFindFormId + ' input[id=FunctionalLocation]'),
                descriptionSearch: $(SAPFunctionalLocationFindFormId + ' input[id=Description]'),
                functionalLocationCategorySearch: $(SAPFunctionalLocationFindFormId + ' select[id=FunctionalLocationCategory]'),
                sortFieldSearch: $(SAPFunctionalLocationFindFormId + ' input[id=SortField]'),
                technicalObjectTypeSearch: $(SAPFunctionalLocationFindFormId + ' select[id=TechnicalObjectType]'),
                planningPlantSearch: $(SAPFunctionalLocationFindFormId + ' select[id=PlanningPlant]'),
                //selected values
                selectedFunctionalLocation: $(SAPFunctionalLocationFindFormId + ' '),
                //other
                sapFunctionalLocationResults: $(SAPFunctionalLocationFindFormId + ' table[id=sapFunctionalLocationResults]')
            };

            ELEMENTS.doSAPSAPFunctionalLocationSearch.on('click', sfl.doSearch_click);
            ELEMENTS.selectSAPFunctionalLocation.on('click', sfl.selectSAPFunctionalLocation_click);
            sfl.setInitialValues();
            window.setTimeout(function () { ELEMENTS.functionalLocationSearch.focus(); }, 200);
        },

        doSearch_click: function () {
            ELEMENTS.doSAPSAPFunctionalLocationSearch.attr('disabled', 'true');
            ELEMENTS.doSAPSAPFunctionalLocationSearch.text('Searching...');
            if (ELEMENTS.operatingCenterSearch.val() === '') {
                alert('Please close the dialog and select an operating center first.');
                // we need to exit at this point, and we need to return false from this function in order to
                // prevent the browser's default behavior.  thus we have two `return false` statements in this
                // function.  not all code smells are actual issues.
                return false;
            }
            ELEMENTS.sapFunctionalLocationResults.find('tr:gt(0)').remove();
            $.getJSON(
                URLS.sapFunctionalLocationSearch,
                $.param({
                    'FunctionalLocation': ELEMENTS.functionalLocationSearch.val(),
                    'Description': ELEMENTS.descriptionSearch.val(),
                    'FunctionalLocationCategory': ELEMENTS.functionalLocationCategorySearch.val(),
                    'SortField': ELEMENTS.sortFieldSearch.val(),
                    'TechnicalObjectType': ELEMENTS.technicalObjectTypeSearch.val(),
                    'OperatingCenter': ELEMENTS.operatingCenterSearch.val(),
                    'PlanningPlant': ELEMENTS.planningPlantSearch.val()
                }),
                SAPFunctionalLocationFind.search_callBack);
            return false;
        },

        resultsRow_click: function (e) {
            $(e.target.parentElement.parentElement).find('td').css('background-color', '');
            $(e.target.parentElement).find('td').css('background-color', 'orange');
            ELEMENTS.selectedFunctionalLocation.val(e.target.parentElement.cells[0].innerHTML);
        },

        resultsRow_doubleClick: function (e) {
            sfl.resultsRow_click(e);
            sfl.selectSAPFunctionalLocation_click();
        },

        search_callBack: function (d) {
            ELEMENTS.doSAPSAPFunctionalLocationSearch.removeAttr('disabled');
            ELEMENTS.doSAPSAPFunctionalLocationSearch.text('Search');

            if (d.Data.length === 1 && d.Data[0].SAPErrorCode !== '' && d.Data[0].SAPErrorCode.indexOf('Successful') === -1) {
                ELEMENTS.sapFunctionalLocationResults.find('tbody').append(
                    '<tr>' +
                    '<td colspan=4>' + d.Data[0].SAPErrorCode + '</td>' +
                    '</tr>');
                return false;
            }
            for (var x = 0; x < d.Data.length; x++) {
                ELEMENTS.sapFunctionalLocationResults.find('tbody').append(
                    '<tr>' +
                    '<td>' + d.Data[x].FunctionalLocation + '</td>' +
                    '<td>' + d.Data[x].FunctionalLocationDescription + '</td>' +
                    //'<td>' + d.Data[x].FunctionalLocationCategory + '</td>' +
                    //'<td>' + d.Data[x].SortField + '</td>' +
                    //'<td>' + d.Data[x].TechnicalObjectType + '</td>' +
                    '</tr>');
            }
            const rows = ELEMENTS.sapFunctionalLocationResults.find('> tbody > tr');
            rows.on('click', sfl.resultsRow_click);
            rows.on('dblclick', sfl.resultsRow_doubleClick);
        },

        selectSAPFunctionalLocation_click: function () {
            if (ELEMENTS.selectedFunctionalLocation.val() !== '' && ELEMENTS.selectedFunctionalLocation.val() !== 'null') {
                sfl.setParentValue(ELEMENTS.functionalLocation, ELEMENTS.selectedFunctionalLocation.val());
            };
            ELEMENTS.functionalLocation.change();
            $(SAPFunctionalLocationFindFormId + ' button[class=cancel]').click();
        },

        setInitialValues: function () {
            if (ELEMENTS.functionalLocationSearch.val() === '')
                ELEMENTS.functionalLocationSearch.val(ELEMENTS.functionalLocation.val());
        },

        setParentValue: function (elem, val) {
            elem.val(val);
            elem.blur();
        }
    };
    $(document).ready(sfl.initialize);
    return sfl;
})(jQuery);