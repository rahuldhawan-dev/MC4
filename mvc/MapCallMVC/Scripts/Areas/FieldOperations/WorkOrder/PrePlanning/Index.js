var WorkOrderPrePlanning = (function ($) {
    var m = {
        initialize: function () {
            m.initAssignTo();
            m.initFormSubmit();
        },

        initAssignTo: function () {
            $('[name="AssignTo"]').on('click', m.onAssignToClick);
        },
        onAssignToClick: function () {
            var name = $('[name="AssignTo"]:checked').val();
            if (name === "user") {
                $("#ContractorAssignedTo").hide();
                $("#AssignedTo").show();
            } else {
                $("#ContractorAssignedTo").show();
                $("#AssignedTo").hide();
            }
            
            $('input[name="WorkOrderIds"]').each(function () {
                if (name === 'contractor' && $(this).attr("IsEnabled") === 'False') {
                    $(this).attr('disabled', 'disabled');
                    $(this).prop("checked", false);
                } else {
                    $(this).removeAttr('disabled');
                }
            });
        },

        initFormSubmit: function () {
            $('form[id="UpdateWorkOrderPrePlanning"]').on('click', m.onFormSubmit);
            m.onFormSubmit();
        },
        onFormSubmit: function () {
            $('input:checked[name="WorkOrderIds"]').each(function () {
                $(this).attr('form', 'UpdateWorkOrderPrePlanning');
            });
            var plannedCompletionDate = new Date($('#PlannedCompletionDate').val()),
                now = new Date();
            plannedCompletionDate.setHours(0, 0, 0, 0);
            now.setHours(0, 0, 0, 0);
            now.setDate(now.getDate() + 2);
            $('#workOrdersTable input[type=checkbox]:checked').each(function () {
                var text = $(this).closest("tr").find(".priorityClass").text();
                if (plannedCompletionDate >= now && text.toLowerCase() == "emergency") {
                    if (!confirm("You have included an emergency order in your selection. Select OK to continue with the Planned Completion Date update or uncheck the emergency order(s) and pre-plan accordingly.")) {
                        event.preventDefault();
                        return false;
                    }                    
                }
            });
        },

        validatePlannedCompletionDate: function (val, element) {
            var result = true;
            if (!val) return false;
            var plannedCompletionDate = new Date(val),
                now = new Date();
            plannedCompletionDate.setHours(0, 0, 0, 0);
            now.setHours(0, 0, 0, 0);
            if (plannedCompletionDate < now) {
                return false;
            }
            now.setDate(now.getDate() + 2);

            $('#workOrdersTable input[type=checkbox]:checked').each(function () {
                var text = $(this).closest("tr").find(".priorityClass").text();
                if (now > plannedCompletionDate && text.toLowerCase() != "emergency") {
                    result = false;
                }
            });

            return result;
        }
    };
    $(document).ready(m.initialize);
    return m;
})(jQuery);
