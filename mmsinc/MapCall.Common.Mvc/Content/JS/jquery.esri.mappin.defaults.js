(function ($) {
  var onLayerCheckboxCreated = function(esriMap, layer, chk) {
    var name = layer.title.toLowerCase();
    if (name.indexOf('water') == 0) {
      chk.addClass('water');
    } else if (name.indexOf('retired') == 0) {
      chk.addClass('retired');
    } else if (name.indexOf('sewer') == 0) {
      chk.addClass('sewer');
    } else if (name.indexOf('raw') >= 0) {
      chk.addClass('raw');
    }
  };

  var createLegendButton = function (esriMap, legend, label, cls) {
    cls = cls || label.toLowerCase();
    var btn = $('<button type="button">' + label + '</button>');
    btn.click(function() {
      var turnOff = $('.' + cls + ':checkbox:checked').length != 0;
      $('.' + cls + ':checkbox').each(function (i, chk) {
        chk = $(chk);
        if (chk.prop('checked') == turnOff) {
          chk.prop('checked', !turnOff);
          chk.change();
        }
      });
    });
    legend.append(btn);
  };

  var onLayerLegendCreated = function(esriMap, legend) {
    createLegendButton(esriMap, legend, 'Sewer');
    createLegendButton(esriMap, legend, 'Water');
    createLegendButton(esriMap, legend, 'Retired');
    createLegendButton(esriMap, legend, 'Raw GPS', 'raw');
  };

  $.esriMapSetup({
    renderLayerCheckboxesReversed: true,
    renderAllLayersButtons: false,
    onLayerCheckboxCreated: onLayerCheckboxCreated,
    onLayerLegendCreated: onLayerLegendCreated
  });
})(jQuery);
