var MeterTest = {
  descriptionRequest: '',
  lbPremiseNumber_onChange: function() {
    var lbPremiseNumber = getServerElement('lbPremiseNumber');
    var premiseID = lbPremiseNumber.val();
    getServerElement('hidPremiseID').val(premiseID);
    if (lbPremiseNumber[0].options.length > 0) {
      getServerElement('lblPremiseNumber')[0].innerHTML =
        lbPremiseNumber[0].options[lbPremiseNumber[0].selectedIndex].text;
    }
    this.validatePremiseID();
  },
  txtPremiseNumber_Keyup: function(txtPremiseNumber) {
    this.lookupPremiseNumber(txtPremiseNumber.value);
  },
  lookupPremiseNumber: function(str) {
    if (str.length < 3) return this.drawNoResults();
    if (this.descriptionRequest) {
      this.descriptionRequest.abort();
      this.descriptionRequest = null;
    }
    this.drawLoadingResults();
    this.descriptionRequest = $.ajax({
      type: 'POST',
      url: '../Data/Premises/Premises.asmx/GetPremiseNumbersWithID',
      data: '{q:\'' + str + '\',limit:\'10\'}',
      contentType: 'application/json; charset=utf-8',
      dataType: 'json',
      success: (function(that) {
        return function() { that.loadLookupResults.apply(that, arguments); };
      })(this),
      error: function(req, status, errorThrown) {
        alert(req.responseText);
      }
    });
  },
  drawNoResults: function() {
    getServerElement('lbPremiseNumber').html('<option disabled="disabled">No results found.</option>');
  },
  drawLoadingResults: function() {
    getServerElement('lbPremiseNumber').html('<option disabled="disabled">Loading results...</option>');
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
      sb.append('<option value="' + cur.PremiseID + '">'
        + cur.PremiseNumber + '</option>');
    }
    getServerElement('lbPremiseNumber').html(sb.toString());
  },
  validatePremiseID: function() {
    if (getServerElement('hidPremiseID').val().length == 0) {
      getServerElement('rfvPremiseID')[0].innerHTML = 'Required';
      return false;
    }
    getServerElement('rfvPremiseID')[0].innerHTML = '';
    return true;
  }
};
