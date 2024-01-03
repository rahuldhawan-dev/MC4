var FormFields = {
  SELECTORS: {
    oneCallMarkoutResponseStatus: '#OneCallMarkoutResponseStatus',
    toggleFields: '#OneCallMarkoutResponseTechnique, #Paint, #Flag, #Stake'
  },
  STATUSES: [
    'No Facilities', 'Cancelled', 'Co. Not Required', 'Unable to Locate'
  ],

  initialize: function() {
    $(FormFields.SELECTORS.oneCallMarkoutResponseStatus).change(FormFields.oneCallMarkoutResponseStatus_change);
  },

  oneCallMarkoutResponseStatus_change: function (e) {
    var status = $('option:selected', e.target).text();
    var hide = FormFields.STATUSES.indexOf(status);
    var fn = hide > -1 ? 'hide' : 'show';

    $(FormFields.SELECTORS.toggleFields).each(function (i) {
      $(this).parent().parent().parent()[fn]();
    });
  }
};

$(document).ready(FormFields.initialize)