var MaterialPicker = {
  txtSearch_Keyup: function(txtSearch) {
    var operatingCenterID = $('#hidOperatingCenterID').val();
    this.lookupNumberOrDescription(txtSearch.value, operatingCenterID);
  }, 

  lbMaterials_Change: function(lbMaterials) {
    $('#hidMaterialID').val($('#' + lbMaterials.id).val());
  },
  descriptionRequest: '',

  lookupNumberOrDescription: function(str, operatingCenterID) {
    if (str == undefined || str.length < 2) {
      return this.drawNoResults();
    }
    if (this.descriptionRequest) {
      this.descriptionRequest.abort();
      this.descriptionRequest = null;
    }
    this.drawLoadingResults();
    this.descriptionRequest = $.ajax({
      type: 'POST',
      url: '../Data/DropDowns.asmx/LookupMaterials',
      data: '{search:\'' + str + '\',operatingCenterID:\'' + operatingCenterID + '\'}',
      contentType: 'application/json; charset=utf-8',
      dataType: 'json',
      // closure magic:  this needs to happen to control the ref to 'this' (functionreturnfunction)
      success: (function(that) {
        return function() { that.loadLookupResults.apply(that, arguments); };
      })(this),
      error: function(req, status, errorThrown) {
        if (req.status !== 0) { alert(req.responseText); }
      }
    });
  },

  drawLoadingResults: function() {
    $('#lbMaterials').html('<option disabled="disabled">Loading results...</option>');
  },

  drawNoResults: function() {
    $('#lbMaterials').html('<option disabled="disabled">No results found.</option>');
  },

  loadLookupResults: function(msg) {
    this.descriptionRequest = null;
    if (!msg.d.length) {
      return this.drawNoResults();
    }

    var sb = new StringBuilder();
    var cur;
    for (var i = 0, len = msg.d.length; i < len; ++i) {
      cur = msg.d[i];
      sb.append('<option value="' + cur.MaterialID +
                '">' + cur.PartNumber + ' - ' + cur.Description +
                '</option>');
    }
    $('#lbMaterials').html(sb.toString());
  }
};

