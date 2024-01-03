(function () {
    $("#PlannedCompletionDate").datepicker({
        onSelect: function (date) {
            $("#updateWorkOrderPlanningSubmitButton").removeAttr('disabled');
        }
    });

    var isRoutineOrHighPriorityWorkOrderChecked = function () {
        return $('input:checked[name="WorkOrderIds"]').is(function (i, e) {
            var recordPriority = $(this).attr("priority");
            return recordPriority === 'Routine' || recordPriority === 'High Priority';
        });
    }

    var isEmergencyChecked = function () {
        return $('input:checked[name="WorkOrderIds"]').is(function (i, e) {
            var recordPriority = $(this).attr("priority");
            return recordPriority === 'Emergency';
        });
    }

    var validateDateIsTwoDaysInTheFuture = function () { 
        var date = $("#PlannedCompletionDate").datepicker('getDate');
        var dateInFuture = new Date();
        dateInFuture.setDate(dateInFuture.getDate() + 2);
        var dateSelect = new Date(date);
        dateInFuture.setHours(0, 0, 0, 0);

        return dateInFuture <= dateSelect;
    }

    var validateDateIsNotInPast = function () {
        var date = $("#PlannedCompletionDate").datepicker('getDate');
        var dateNow = new Date();
        var dateSelect = new Date(date);
        dateNow.setHours(0, 0, 0, 0);

        return dateNow <= dateSelect;
    }

    $('#updateWorkOrderPlanningSubmitButton').on('click', function (evt) {
        if (isRoutineOrHighPriorityWorkOrderChecked() && !validateDateIsTwoDaysInTheFuture()) {
            //executes if there is a checked work order with routine/high priority and the date selected isn't 
            //between 2 days from now and 12 / 31 / 9999 11: 59: 59 PM.
            $("#dateIsNotAtLeastTwoDaysFromNowError").attr('class', 'field-validation-error');
            $("#dateIsInPastError").attr('class', 'field-validation-valid');
        }
        else if (!validateDateIsNotInPast()) {
            //executes if the date is in the past
            $("#dateIsInPastError").attr('class', 'field-validation-error');
            $("#dateIsNotAtLeastTwoDaysFromNowError").attr('class', 'field-validation-valid');        }
        else {
            //Hide errors
            $("#dateInPastError").attr('class', 'field-validation-valid');
            $("#dateIsNotAtLeastTwoDaysFromNowError").attr('class', 'field-validation-valid');

            // get all the checked checkboxes, add them to the form
            $('input:checked[name="WorkOrderIds"]').each(function () {
                $(this).attr('form', 'UpdateWorkOrderPlanning');
            });

            //put up confirm message if an emergency work order is checked.
            if (isEmergencyChecked()) {
                if (!confirm("You have included an emergency order in your selection. Select OK to continue with the Planned Completion Date update or uncheck the emergency order(s) and pre-plan accordingly.")) {
                    //If user hits cancel, don't continue.
                    return;
                }
            }

            //submit form
            $('form[id="UpdateWorkOrderPlanning"]').submit()
        }
    });
})();
