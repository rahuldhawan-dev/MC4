var RestorationDetailView = {
    RESTORATION_TYPES: {
        CURB_RESTORATION: 4,
        GUTTER_RESTORATION: 5
    },
    RGX_DECIMAL_VALUE: /^(\.\d+|\d+(\.\d+)?)$/,

    initialize: function () {
        //Show or hide the appropriate measurment on load.
        var label = $('.lblRestorationType');
        if (label.text())
            this.onRestorationTypeChanged(label.text());

        var edit = $('.ddlRestorationType');
        if ($(':selected', edit).text())
            this.onRestorationTypeChanged($(':selected', edit).text());
    },

    ddlRestorationType_Change: function (elem) {
        this.onRestorationTypeChanged($(':selected', elem).text());
    },

    btnSave_Click: function () {
        var ddlRestorationType = getServerElementById('ddlRestorationType');
        var type = ddlRestorationType.val();
        if (type == '') {
            alert('Please choose a restoration type.');
            ddlRestorationType.focus();
            return false;
        } else if (!this.validatePavingArea(type) || !this.validateForStabBase()) {
            return false;
        }
    },

    btnDone_Click: function () {
        window.opener.getServerElementById('btnRefresh').click();
        window.close();
    },

    onRestorationTypeChanged: function (txt) {
        var toShow, toHide;
        if (txt.indexOf('CURB') > -1) {
            toShow = $('#trLinearFtOfCurb');
            toHide = $('#trPavingSquareFootage');
        } else {
            toShow = $('#trPavingSquareFootage');
            toHide = $('#trLinearFtOfCurb');
        }
        toShow.show();
        toHide.hide();
    },

    validatePavingArea: function (type) {
        var elem = (type == this.RESTORATION_TYPES.CURB_RESTORATION ||
                type == this.RESTORATION_TYPES.GUTTER_RESTORATION) ?
      'txtLinearFeetOfCurb' : 'txtPavingSquareFootage';
        elem = getServerElementById(elem);
        var value = elem.val();
        if (value == '') {
            alert('Please specify the estimated restoration footage.');
            elem.focus();
            return false;
        } else if (!this.RGX_DECIMAL_VALUE.test(value)) {
            alert('Estimated restoration footage must be a numerical value.');
            elem.focus();
            return false;
        }
        return true;
    },

    validateForStabBase: function () {
        var chk = getServerElementById('chkEightInchStabilizeBaseByCompanyForces');

        if (!chk.attr('checked')) {
            return true;
        }

        var elem = getServerElementById('txtPartialPavingSquareFootage');
        var value = elem.val();
        if (value == '') {
            alert('Please specify the estimated base restoration completed.')
            elem.focus();
            return false;
        } else if (!this.RGX_DECIMAL_VALUE.test(value)) {
            alert('Estimated base restoration completed must be a numerical value.');
            elem.focus();
            return false;
        }

        elem = getServerElementById('ccPartialRestorationDate');
        if (elem.val() == '') {
            alert('Please specify the base restoration date.');
            elem.focus();
            return false;
        }

        elem = getServerElementById('txtPartialRestorationCompletedBy');
        if (elem.val() == '') {
            alert('Please specify who completed the base restoration.');
            elem.focus();
            return false;
        }

        return true;
    }
};