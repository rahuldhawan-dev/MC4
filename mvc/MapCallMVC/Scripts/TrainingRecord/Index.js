var TrainingRecordIndex = (function ($) {
  var dateToString = function(date) {
    var day = date.getDate();
    var month = date.getMonth() + 1;

    return date.getFullYear() + '-' + (month > 9 ? '' : '0') + month.toString() + '-' + (day > 9 ? '' : '0') + day.toString();
  };
  
	var tr = {
	  initialize: function() {
			$('#calendar').fullCalendar({
				header: {
					left: 'prev,next today',
					center: 'title',
					right: 'month,agendaWeek,agendaDay'
				},
				defaultDate: tr.getDefaultDate(),
				displayEventEnd: true,
				eventLimit: true, // allow "more" link when too many events
				events: { url: TrainingRecordIndex.getCalendarUrl() },
        eventRender: TrainingRecordIndex.eventRender,
				timezone: 'local'
			});
		},

    getCalendarUrl: function() {
      return window.top.location.href.replace("?", "/Index.cal?");
    },

    eventRender: function(event, element) {
      if (event.description) {
        element.attr('title', event.description);
      }
    },

    getDefaultDate: function () {
    	var today = new Date();
			var vars = [], hash;
			var q = document.URL.split('?')[1];
			if (q != undefined) {
				q = q.split('&');
				for (var i = 0; i < q.length; i++) {
					hash = q[i].split('=');
					vars.push(hash[1]);
					vars[hash[0]] = hash[1];
				}
			}

			if (vars['ScheduledDate.Start'] != '')
				return unescape(vars['ScheduledDate.Start']);
			if (vars['ScheduledDate.End'] != '')
				return unescape(vars['ScheduledDate.End']);
			return dateToString(today);
		}
	};

	$(document).ready(tr.initialize);
	return tr;
})(jQuery);