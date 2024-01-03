// CrewAssignments/Index.js

var Calendar = {
    '_availabilityPercentagesForMonth': null,
    'initialize': function() {
        Calendar._availabilityPercentagesForMonth = Calendar.getAvailabilityPercentages();
        $('#crewCalendar').datepicker({
            // null value for defaultDate just defaults it to today.
            'defaultDate': new Date($('#Date').val()),
            'onSelect': Calendar.onDatePickerSelect,
            'showOtherMonths': true,
            'selectOtherMonths': true,
            'beforeShowDay': Calendar.onDatePickerBeforeShowDay
        });
    },
   
    'getAvailabilityPercentages': function() {
        var dp = [];
        $('#crewCalendar input[type="hidden"]').each(function() {
            var input = $(this);
            var inputDate = input.data("date");
            if (inputDate) {
                var realDate = new Date(inputDate);
                dp[realDate.getDate()] = {
                    'percent': input.val(),
                    'date': realDate
                };
            }
        });
        return dp;
    },
    'onDatePickerSelect': function(selectedDate, instance) {
        $('#Date').val(selectedDate);
        Calendar.refreshCalendar();
        return true;
    },
    'onDatePickerBeforeShowDay': function(theDate) {
        var css = "";
        var dayOfMonth = theDate.getDate();
        var avail = Calendar._availabilityPercentagesForMonth[dayOfMonth];
        if (avail && avail.date.getMonth() == theDate.getMonth()) {
            var percent = parseFloat(avail.percent);
            if (percent == 0) {
                css = "day-0";
            }
            else if (0 < percent && percent < 0.5) {
                css = "day-0-50";
            }
            else if (0.5 <= percent && percent < 1.0) {
                css = "day-50-100";
            }
            else {
                css = "day-100";
            }
        }
        // Need to return this array of whatevers:
        // return [bool:Enabled, string:css, string:tooltip]
        return [true, css, ""];
    },
    'refreshCalendar': function() {
        $('#SearchForm').submit();
    }
}

$(document).ready(Calendar.initialize);