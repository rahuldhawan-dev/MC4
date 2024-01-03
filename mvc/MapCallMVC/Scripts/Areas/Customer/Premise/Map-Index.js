// NOTE: Try not to reference the esri plugin calls here. This script is already an abstraction layer
//       away from it. Add any calls to Map/Index.js instead.

var Premises = (function ($, Maps) {
    var pre = {
        init: function () {
           Maps.onInit = function () {
            var attachLegendItemOnly = function (Id, iconFileName, labelText, group) {
                    Maps.initializeLegend();
                    var icon = Maps.getIconByFileName(iconFileName);
                    var legendRow = $('<tr>' +
                     '<td><img /></td>' +
                        '<td class="label-cell"><label></label></td>' +
                        '</tr>');
                    var id = 'chk' + Id;
                    legendRow.find('img').attr({ src: icon.url })
                        .css({ width: icon.width, height: icon.height });
                    legendRow.find('label').text(labelText).attr({ 'for': id });
                    if (group) {
                        Maps.getLegendGroup(group).append(legendRow);
                    }
                    else {
                        Maps._legend.append(legendRow);
                    }
                };
                var createPremisesLegendGroup = function (groupName, prefix) {
                    var filePrefix = prefix.toLowerCase();
                    attachLegendItemOnly(prefix + 'Premise', filePrefix + '-blue', 'Premise', groupName);
                    attachLegendItemOnly(prefix + 'CriticalCare', filePrefix + '-orange', 'Critical Care', groupName);
                    attachLegendItemOnly(prefix + 'premise', filePrefix + '-mablue', 'Major Account', groupName);
                    attachLegendItemOnly(prefix + 'premise', filePrefix + '-blueorange', 'Critical Care with Major Account', groupName);
                };
                createPremisesLegendGroup('Premises', 'Premise');
            };
        }
    };
    pre.init();
    return pre;
})(jQuery, Maps);