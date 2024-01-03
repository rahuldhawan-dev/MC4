var WorkOrderMapForm = {
  initialize: function() {
    $('#mapTab').click(this.loadMap);
    $('a.tab').click(this.unloadMap);
  },
  loadMap: function() {
    var assetTypeID = getServerElement('hidMapAssetTypeID').val();
    var assetID = getServerElement('hidMapAssetID').val();

    var woLatitude = getServerElement('hidWorkOrderLatitude').val();
    var woLongitude = getServerElement('hidWorkOrderLongitude').val();

    $('#mapWrapper').html('<iframe src="../../Assets/AssetLatLonReadOnlyView.aspx?assetTypeID=' +
    assetTypeID + '&assetID=' + assetID + '&latitude=' + woLatitude + '&longitude=' + 
    woLongitude + '&operatingCenter=' + getServerElementById('hidOperatingCenter').val() + '" class="map"></iframe>');
  },
  unloadMap: function() {
    $('#mapWrapper').html('');
  }
};
