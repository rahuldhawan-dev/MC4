var WorkOrderGeneralDetailView = {
    initialize: function() {
        // make tabstrip
      $('#tblInitialInformation td:even').css('font-weight', 'bold');
      getServerElementById('btnDelete').click(WorkOrderGeneralDetailView.onDeleteClick);
    },
    lbPartSearchResults_Change: function (lbPartSearchResults) {
      var ret = this.selectPartNumberByValue($(lbPartSearchResults).val());
      getServerElementById('gvMaterialsUsed_txtQuantity').focus();
      return ret;
    },
    selectPartNumberByValue: function(val) {
        var ddlPartNumber = getServerElementById('ddlPartNumber')[0];
        var opt = $('#' + ddlPartNumber.id + ' option[value=' + val + ']')[0];
        opt.selected = true;
        WorkOrderMaterialsUsedForm.ddlPartNumber_Change(ddlPartNumber);
    },
    txtPartSearch_Keyup: function(txtPartSearch) {
        var operatingCenterID = getServerElementById('hidOperatingCenterIDForMaterialLookup').val();
        this.lookupNumberOrDescription(txtPartSearch.value, operatingCenterID);
    },
    descriptionRequest: '',
    lookupNumberOrDescription: function(str, operatingCenterID) {
        if (str.length < 2) {
            return this.drawNoResults();
        }
        if (this.descriptionRequest) {
            this.descriptionRequest.abort();
            this.descriptionRequest = null;
        }
        this.drawLoadingResults();
        this.descriptionRequest = $.ajax({
            type: 'POST',
            url: '../../OperatingCenterStockedMaterials/OperatingCenterStockedMaterialServiceView.asmx/LookupMaterials',
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
        $('#lbPartSearchResults').html('<option disabled="disabled">Loading results...</option>');
    },
    loadLookupResults: function(msg) {
        this.descriptionRequest = null;
        if (!msg.d.length) {
            return this.drawNoResults();
        }

        var sb = new StringBuilder();
        sb.append('<option value="">--Select Here--</option>');
        var cur;
        for (var i = 0, len = msg.d.length; i < len; ++i) {
            cur = msg.d[i];
            sb.append('<option value="' + cur.MaterialID +
                    '">' + cur.PartNumber + ' - ' + cur.Description + ' - ' + cur.Size +
                    '</option>');
        }
        $('#lbPartSearchResults').html(sb.toString());
    },
    drawNoResults: function() {
        $('#lbPartSearchResults').html('<option disabled="disabled">No results found.</option>');
    },
    onDeleteClick: function () {
      if (getServerElementById('ddlWorkOrderCancellationReasons').val() === '') {
        alert('Please select the reason for the cancellation.');
        getServerElementById('ddlWorkOrderCancellationReasons').focus();
        return false;
      }
      return confirm('Are you sure you want to cancel the order?');
    }
};