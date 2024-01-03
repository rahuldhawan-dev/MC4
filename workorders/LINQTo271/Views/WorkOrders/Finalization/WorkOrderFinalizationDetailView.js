var WorkOrderFinalizationDetailView = {
    RGX_MAIN_BREAK: /^WATER MAIN BREAK REP(AIR|LACE)$/,

    initialize: function () {
        $('#tblInitialInformation td:even').css('font-weight', 'bold');
        this.setSessionTimeout();
    },
    verifyMobileGis: function () {
        var isMain = getServerElementById('IsMainReplaceOrRepair').val();
        if (!isMain === 'True') {
            return true;
        }
        return confirm("Please Update Leak Repair Location on the Mobile GIS Map Now");
    },

    ///////////////////////////////////////////////////////////////////////////
    //////////////////////////////EVENT HANDLERS///////////////////////////////
    ///////////////////////////////////////////////////////////////////////////
    btnSave_Click: function () {
      // noinspection FallThroughInSwitchStatementJS
      switch (false) {
        case this.validateDateCompleted():
        case this.validateNewWorkDescription():
        case this.validateMainBreakInfo():
      	case this.validateServiceLineInfo():
        case this.validateCrewAssignments():
        case this.validateScheduleOfValues():
        case this.validateGallonsLost():
        case this.validateDistance():
        case this.validateDigitalAsBuiltInfo():
        case this.validatePitcherFilterInfo():
          return false;
        default:
          this.toggleSaveButton();
          return true;
          // return this.verifyMobileGis();
        }
    },
    toggleSaveButton: function () {
    	getServerElementById('btnSave').hide();
    	$('#btnDummySaveFinalization').show();
    },

    lbPartSearchResults_Change: function (lbPartSearchResults) {
      var ret = this.selectPartNumberByValue($(lbPartSearchResults).val());
      getServerElementById('gvMaterialsUsed_txtQuantity').focus();
      return ret;
    },

    txtPartSearch_Keyup: function (txtPartSearch) {
        var operatingCenterID = getServerElementById('hidOperatingCenterIDForMaterialLookup').val();
        this.lookupNumberOrDescription(txtPartSearch.value, operatingCenterID);
    },

    ///////////////////////////////////////////////////////////////////////////
    ////////////////////////////////UI HELPERS/////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////
    selectPartNumberByValue: function (val) {
        var ddlPartNumber = getServerElementById('ddlPartNumber')[0];
        var opt = $('#' + ddlPartNumber.id + ' option[value=' + val + ']')[0];
        opt.selected = true;
        WorkOrderMaterialsUsedForm.ddlPartNumber_Change(ddlPartNumber);
    },

    descriptionRequest: '',

    lookupNumberOrDescription: function (str, operatingCenterID) {
        if (str.length < 2) {
            this.drawNoResults();
            return;
        }
        if (this.descriptionRequest) {
            console.debug('aborting');
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
            success: (function (that) {
                return function () { that.loadLookupResults.apply(that, arguments); };
            })(this),
            error: function (req, status, errorThrown) {
                // jQuery still throws error if abort is called on the request.
                if (req.status !== 0) { alert(req.responseText); }
            }
        });
    },

    drawLoadingResults: function () {
        $('#lbPartSearchResults').html('<option disabled="disabled">Loading results...</option>');
    },

    loadLookupResults: function (msg) {
        this.descriptionRequest = null;
        if (!msg.d.length) {
            this.drawNoResults();
            return;
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

    drawNoResults: function () {
        $('#lbPartSearchResults').html('<option disabled="disabled">No results found.</option>');
    },

    ///////////////////////////////////////////////////////////////////////////
    ////////////////////////////////VALIDATION/////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////
    validateDateCompleted: function () {
      if (getServerElementById('txtDateCompleted').val().length > 0) {
        return true;
      }

      alert('Please enter a completion date.');
      return false;
    },

    validateNewWorkDescription: function () {
        var ddlFinalWorkDescription = getServerElementById('ddlFinalWorkDescription');
        var selectedWorkDescription = $(':selected', ddlFinalWorkDescription).text();
        if (/^WATER MAIN BREAK REP(AIR|LACE)$/.test(selectedWorkDescription) && $('div#mainbreak').length == 0) {
            alert('The work description indicates a main break. Main break information needs to be entered.  Please click the "Update" link under the "Additional" tab, enter main break information when the screen refreshes, and then Finalize.');
            return false;
        }
        return true;
    },

    validateServiceLineInfo: function() {
      var valid = true;
      var isServiceLineRetire = this.isServiceLineRetire();
      var isServiceLineRenewal = this.isServiceLineRenewal();
      if (!isServiceLineRenewal && !isServiceLineRetire)
        return true;

      var completed = getServerElementById('txtDateCompleted');
      if (completed === 'undefined' || completed.val() === '')
        return true;

      var previousServiceLineMaterial = getServerElementById('ddlPreviousServiceLineMaterial');
      if (previousServiceLineMaterial.val() == '' && isServiceLineRenewal) {
        valid = false;
        previousServiceLineMaterial.focus();
      }

      var previousServiceLineSize = getServerElementById('ddlPreviousServiceLineSize');
      if (previousServiceLineSize.val() == '' && isServiceLineRenewal) {
        if (valid) {
          valid = false;
        }
        previousServiceLineSize.focus();
      }

      var companyServiceLineMaterial = getServerElementById('ddlCompanyServiceLineMaterial');
      if (companyServiceLineMaterial.val() == '' && (isServiceLineRetire || isServiceLineRenewal)) {
        if (valid) {
          valid = false;
        }
        companyServiceLineMaterial.focus();
      }

      var companyServiceLineSize = getServerElementById('ddlCompanyServiceLineSize');
      if (companyServiceLineSize.val() == '' && (isServiceLineRetire || isServiceLineRenewal)) {
        if (valid) {
          valid = false;
        }
        companyServiceLineSize.focus();
      }

      var customerServiceLineMaterial = getServerElementById('ddlCustomerServiceLineMaterial');
      if (customerServiceLineMaterial.val() == '' && isServiceLineRenewal) {
        if (valid) {
          valid = false;
        }
        customerServiceLineMaterial.focus();
      }

      var customerServiceLineSize = getServerElementById('ddlCustomerServiceLineSize');
      if (customerServiceLineSize.val() == '' && isServiceLineRenewal) {
        if (valid) {
          valid = false;
        }
        customerServiceLineSize.focus();
      }

      var doorNoticeLeftDate = getServerElementById('txtDoorNoticeLeftDate');
      if (doorNoticeLeftDate.val() == '' && isServiceLineRenewal) {
        if (valid) {
          valid = false;
        }
        doorNoticeLeftDate.focus();
      }

      if (isServiceLineRenewal && valid) {
        var date = new Date(doorNoticeLeftDate.val());
        if (date instanceof Date && !isNaN(date.valueOf())) {
          valid = true;
        } else {
          valid = false;
        }
      }

      if (valid)
        return true;
      else {
        $('#additionalTab').click();
        const msg = isServiceLineRetire
          ? 'The order being entered is for a service line retire.  Please enter all the service line details.'
          : 'The order being entered is for a service line renewal.  Please enter all the service line details.';
        alert(msg);
        return false;
      }
    },

    validatePitcherFilterInfo: function() {
      if (!this.requiresPitcherFilter()) {
        return true;
      }

      const txtFlushTime = getServerElementById('txtInitialServiceLineFlushTime');
      const flushTime = txtFlushTime.val();

      if (flushTime !== '' && isNaN(flushTime)) {
        txtFlushTime.focus();
        alert('Initial service line flush time must be a number');
        return false;
      }

      if (parseInt(flushTime, 10) < 30) {
        alert('Below minimum-reflush');
      }

      if (!getServerElementById('chkHasPitcherFilterBeenProvidedToCustomer').is(':checked')) {
        return true;
      }

      const txtDatePitcherFilterDeliveredToCustomer =
        getServerElementById('txtDatePitcherFilterDeliveredToCustomer');

      if (txtDatePitcherFilterDeliveredToCustomer.val() === '') {
        txtDatePitcherFilterDeliveredToCustomer.focus();
        alert('Please enter the date when pitcher filter was provided to customer');
        return false;
      }

      const ddlPitcherFilterCustomerDeliveryMethod =
        getServerElementById('ddlPitcherFilterCustomerDeliveryMethod');

      if (ddlPitcherFilterCustomerDeliveryMethod.val() === '') {
        ddlPitcherFilterCustomerDeliveryMethod.focus();
        alert('Please select the method by which the filter was delivered to the customer');
        return false;
      }

      const txtPitcherFilterCustomerDeliveryOtherMethod =
        getServerElementById('txtPitcherFilterCustomerDeliveryOtherMethod');
      if ($('option:selected', ddlPitcherFilterCustomerDeliveryMethod).text() === 'Other' &&
        txtPitcherFilterCustomerDeliveryOtherMethod.val() === '') {
        txtPitcherFilterCustomerDeliveryOtherMethod.focus();
        alert('Please describe the method by which the filter was delivered to the customer');
        return false;
      }
    },

    hasFinalWorkDescription: function() {
      return Array.prototype.slice.call(arguments)
        .includes(getServerElementById('ddlFinalWorkDescription').val());
    },

    isServiceLineRenewal: function () {
      return this.hasFinalWorkDescription(
        '59',  // service line renewal company side
        '193', // ???
        '295'  // service line renewal company side-lead
      );
    },

    isServiceLineRetire: function () {
      return this.hasFinalWorkDescription(
        '60',  // service line retire
        '298', // service line retire-lead
        '313', // service line retire no premise
        '314'  // service line retire-lead no premise
      );
    },

    requiresPitcherFilter: function() {
      return this.hasFinalWorkDescription(
        '59',  // service line renewal company side
        '295', // service line renewal company side-lead
        '222', // service line renewal cust side
        '307'  // service line renewal cust side-lead        
      );
    },

    validateMainBreakInfo: function () {
        var lblError = getServerElementById('lblMainBreakError');
        lblError.html('');

        if (this.finalWorkDescriptionIsMainBreak()) {
            //Check if this is a Main Break replace or repair order
            var table = getServerElementById('gvMainBreak');
            //Count the rows to see if we have at least 1 associated mainbreak object
            var rows = $('tr', table);
            //1: Header 2: Spacer 3: Footer.  
            //If rows is 3, they haven't entered a main break yet.
            if (rows.length <= 3) {
                //Check the second row actually has values in it.
                var cells = $('td', rows[1]);
                if (cells.length == 1) {
                    lblError.html('You must enter the main break info before continuing.<br/>');
                    return false;
                }
            }

            if (getServerElementById('ddlFinalSignificantTrafficImpact').val() == "" ||
              getServerElementById('txtLostWater').val() == "" ||
              getServerElementById('ddlFinalRepairTimeRange').val() == "" ||
              getServerElementById('ddlFinalCustomerImpactRange').val() == ""
              ) {
                $('#additionalTab').click();
                lblError.html('You must enter the main break info under the additional tab before continuing.<br/>');
                return false;
            }
        }

        return true;
    },

    validateCrewAssignments: function () {
        if (WorkOrderCrewAssignmentForm.hasOpenAssignments()) {
            $('#crewAssignmentsTab').click();
            alert('This order has one or more Crew Assignments that are not closed.  Please ensure that all end times are recorded.');
            return false;
        } else {
            return true;
        }
    },

    validateScheduleOfValues: function () {
      if (window.WorkOrderScheduleOfValuesForm === undefined)
        return true;
      if (getServerElementById('hidOperatingCenterHasWorkOrderInvoicing') && !WorkOrderScheduleOfValuesForm.hasScheduleOfValues() && WorkOrderFinalizationDetailView.validateDateCompleted()) {
    		$('#scheduleOfValuesTab').click();
				alert('Please ensure that all schedule of values have been added.');
		    return false;
	    }
			return true;
		},

    validateGallonsLost: function() {
      var val = getServerElementById('txtLostWater').val();
      if (val === '')
        return true;
      if (!$.isNumeric(val)) {
        alert('Please enter a valid number of gallons lost');
        getServerElementById('txtLostWater').focus();
        return false;
      }
      return true;
    },

    validateDistance: function() {
      var val = getServerElementById('txtDistanceFromCrossStreet').val();
      if (val === '')
        return true;
      if (!$.isNumeric(val)) {
        alert('Please enter a valid distance');
        getServerElementById('txtDistanceFromCrossStreet').focus();
        return false;
      }
      return true;
    },

    validateDigitalAsBuiltInfo: function() {
      if (getServerElementById('hidDigitalAsBuiltRequired').val() !== 'True' ||
          getServerElementById('ddlDigitalAsBuiltCompleted').val() !== '')
      {
        return true;
      }

      alert('Please indicate whether or not a digital as-built has been completed.');
      return false;
    },

    finalWorkDescriptionIsMainBreak: function (desc) {
        return this.RGX_MAIN_BREAK.test(
      desc || $('option:selected',
                getServerElementById('ddlFinalWorkDescription')).text());
    },
    ///////////////////////////////////////////////////////////////////////////
    /////////////////////////////////TIMEOUT///////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////

    setSessionTimeout: function () {
        $(document).idleTimeout({
            inactivity: 7080000,      // 118 minutes 
            noconfirm: 60000,
            sessionAlive: false,
            alive_url: '/stillalive.aspx',
            logout_url: '/logout.aspx',
            redirect_url: '/login.aspx?returnUrl=' +
        $(location).attr('pathname') +
        $(location).attr('search')
        });
    }

};
