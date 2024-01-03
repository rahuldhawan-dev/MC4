(function($) {
  var EsriPicker = {
    DEFAULTS: {
      ZOOM: 16
    },
    MAP_ID: '6f796eb2f2284f89be86048b80f5f6cf',

    loadMap: function() {
      var thisMap = $(this);

      var lat = thisMap.data('latitude');
      var lng = thisMap.data('longitude');

      thisMap.esriMap({
        mapId: EsriPicker.MAP_ID,
        initialZoom: EsriPicker.DEFAULTS.ZOOM,
        center: { lat: lat, lng: lng },
        callback: function(map) {
          thisMap.esriMap('addSimpleMarker', lng, lat);
        }
      });
    }
  };

  $.fn.esriPicker = function() {
    return this.each(EsriPicker.loadMap);
  };

})(jQuery);