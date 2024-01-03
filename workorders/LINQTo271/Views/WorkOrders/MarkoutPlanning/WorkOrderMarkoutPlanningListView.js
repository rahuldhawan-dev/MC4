var WorkOrderMarkoutPlanningListView = {
  initialize: function () {
    this.initializeEventHandlers();
  },

  initializeEventHandlers: function () {
    $(document).on('change', '.date_needed', this.dateNeededChanged);
    $(document).on('change', '.markout_type', this.markoutTypeChanged);
    $(document).on('click', '.save_link', this.saveLinkClicked);
  },

  dateNeededChanged: function (e) {
    var target = $(e.target);
    var dateToCallLabel = $('span.date_to_be_called', target.parent().parent());
    $.ajax({
      type: 'POST',
      url: '../../Markouts/MarkoutsServiceView.asmx/GetCallDateForDateNeeded',
      data: '{dateNeeded:\'' + target.val() + '\'}',
      contentType: 'application/json; charset=utf-8',
      dataType: 'json',
      success: WorkOrderMarkoutPlanningListView.getGetCallDateCallback(dateToCallLabel)
    });
  },

  saveLinkClicked: function (e) {
    var row = $(e.target).parent().parent();
    var txtDateNeeded = $('.date_needed', row);
    var ddlMarkoutType = $('.markout_type', row);

    if (isNaN(Date.parse(txtDateNeeded.val()))) {
      alert('Cannot update a record without entering the date needed.');
      txtDateNeeded.focus();
      return false;
    } else if (ddlMarkoutType.val() == '') {
      alert('Cannot update a record without choosing the markout type.');
      ddlMarkoutType.focus();
      return false;
    }

    return true;
  },

  getGetCallDateCallback: function (label) {
    return function (e) {
      label.text(e.d);
    };
  },

  markoutTypeChanged: function (e) {
    var target = $(e.target);
    var parent = target.parent();
    var txtNotes = $('input', parent);
    txtNotes.toggle($(':selected', target).text() == 'NOT LISTED');
  }
};
