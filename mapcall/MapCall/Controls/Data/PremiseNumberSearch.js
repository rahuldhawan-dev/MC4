// PremiseNumberSearch control script

var PremiseNumberSearch = function(args) {

    // Just a util function to tack the # on to the id for jQuery.
    var getById = function(id) {
        return $('#' + id);
    }

    this.hfPremiseID = getById(args.hidPremiseID);
    this.hfPremiseNumber = getById(args.hidPremiseNumber);
    this.label = getById(args.lblPremiseNumber);
    this.listBox = getById(args.listPremiseNumber);
    this.searchTextBox = getById(args.txtPremiseNumber);
    this.removeSelectedLink = getById(args.lnkRemoveSelected);

    this.isRequired = args.isRequiredField;
    if (this.isRequired) {
        // The control doesn't render if it's not required, so no reason
        // to look for it.
        this.requiredFieldValidator = getById(args.rfvPremiseID);
    }
    // I feel this is hacky. -Ross
    var that = this;

    this.searchTextBox.keyup(function() {
        that.lookupPremiseNumber();
    });

    this.listBox.change(function() {
        that.listBoxChanged();
    });

    this.removeSelectedLink.click(function() {
        that.clearSelected();
        return false;
    });
};
PremiseNumberSearch.servicePath = null;
PremiseNumberSearch.prototype = {
    // Properties
    descriptionRequest: null,
    hfPremiseID: null,
    hfPremiseNumber: null,
    isRequired: false,
    label: null,
    listBox: null,
    removeSelectedLink: null,
    requiredFieldValidator: null,
    searchTextBox: null,


    // Methods
    clearSelected: function () {
        this.setSelected(null, null);
    },
    drawNoResults: function () {
        this.listBox.html('<option disabled="disabled">No results found.</option>');
    },
    drawLoadingResults: function () {
        this.listBox.html('<option disabled="disabled">Loading results...</option>');
    },

    // Methods

    setSelected: function (premiseId, premiseNumber) {
        this.hfPremiseID.val(premiseId);
        this.hfPremiseNumber.val(premiseNumber);
        this.label.html(premiseNumber);

        if (this.isRequired) {
            this.validatePremiseID();
        };
    },

    listBoxChanged: function () {
        var premiseId = this.listBox.val();
        var premiseNumber = "";

        var lBox = this.listBox[0];

        if (lBox.options.length > 0) {
            premiseNumber = lBox.options[lBox.selectedIndex].text;
        }

        this.setSelected(premiseId, premiseNumber);
    },

    loadLookupResults: function (msg) {
        this.descriptionRequest = null;

        if (!msg.d.length) {
            return this.drawNoResults();
        }
        else {
            var sb = new StringBuilder();
            var cur;
            for (var i = 0, len = msg.d.length; i < len; ++i) {
                cur = msg.d[i];
                sb.append('<option value="' + cur.PremiseID + '">' + cur.PremiseNumber + '</option>');
            }
            this.listBox.html(sb.toString());
        }
    },

    lookupPremiseNumber: function () {
        var str = this.searchTextBox.val();

        if (str.length < 3) {
            return this.drawNoResults();
        }
        else {
            if (this.descriptionRequest) {
                this.descriptionRequest.abort();
            }

            this.drawLoadingResults();
            this.descriptionRequest = $.ajax({
                type: 'POST',
                url: (PremiseNumberSearch.servicePath + '/GetPremiseNumbersWithID'),
                data: '{q:\'' + str + '\',limit:\'10\'}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: (function (that) {
                    return function () { that.loadLookupResults.apply(that, arguments); };
                })(this),
                error: function (req, status, errorThrown) {
                    // Empty text means the request got aborted, which we don't care about errors for.
                    if (req.responseText != '') {
                        alert("Error: " + req.responseText);
                    }
                }
            });
        }
    },

    validatePremiseID: function () {
        var rfv = this.requiredFieldValidator[0];
        if (this.hfPremiseID.val().length == 0) {
            rfv.innerHTML = 'Required';
            return false;
        }
        else {
            rfv.innerHTML = '';
            return true;
        }
    }
};


