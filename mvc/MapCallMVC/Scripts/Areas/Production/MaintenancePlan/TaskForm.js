(function ($) {
    var EMPTY_PLACEHOLDER = '(empty)';
    var NONE_SELECTED_TASK_DETAILS_PLACEHOLDER = 'Please select a task group name above';
    var NONE_SELECTED_PLAN_TYPE_PLACEHOLDER = 'Please select a task group name below';
    var ADDITIONAL_TASK_DETAILS_PLACEHOLDER = '(enter additional task information)';

    var taskGroupDataEndpointUrl = $('#TaskGroupDataEndpointUrl').val();
    var elements = {
        taskGroupDropdown: $('#TaskGroup'),
        taskDetails: $('#TaskDetails'),
        taskDetailsSummary: $('#TaskDetailsSummary'),
        additionalTaskDetails: $('#AdditionalTaskDetails'),
        planType: $('#PlanType')
    };

    function fetchTaskGroupDetails() {
        var result = {};
        $.ajax({
            type: 'GET',
            async: false,
            url: taskGroupDataEndpointUrl,
            data: {
                taskGroupId: elements.taskGroupDropdown.val()
            },
            success: function (d) {
                result = d.Data;
            }
        });
        return result;
    }

    function disableTaskGroupDataTextBoxesAndShowDefaultPlaceholder() {
        elements.taskDetails.prop('disabled', true);
        elements.taskDetailsSummary.prop('disabled', true);
        elements.planType.prop('disabled', true);
        elements.taskDetails.val(NONE_SELECTED_TASK_DETAILS_PLACEHOLDER);
        elements.taskDetailsSummary.val(NONE_SELECTED_TASK_DETAILS_PLACEHOLDER);
        elements.planType.val(NONE_SELECTED_PLAN_TYPE_PLACEHOLDER);
    }

    function enableTaskDataTextBoxes() {
        elements.taskDetails.prop('disabled', false);
        elements.taskDetailsSummary.prop('disabled', false);
        elements.planType.prop('disabled', false);
    }

    function checkSelectedTaskGroupAndUpdateTaskDataTextBoxesAccordingly() {
        if (!elements.taskGroupDropdown.val()) {
            disableTaskGroupDataTextBoxesAndShowDefaultPlaceholder();
            return;
        }
        var result = fetchTaskGroupDetails();

        enableTaskDataTextBoxes();
        elements.taskDetails.val(result.TaskDetails || EMPTY_PLACEHOLDER);
        elements.taskDetailsSummary.val(result.TaskDetailsSummary || EMPTY_PLACEHOLDER);
        elements.planType.val(result.PlanType || EMPTY_PLACEHOLDER);
    }
    
    $(document).ready(function () {
        elements.additionalTaskDetails.prop('placeholder', ADDITIONAL_TASK_DETAILS_PLACEHOLDER);
        checkSelectedTaskGroupAndUpdateTaskDataTextBoxesAccordingly();

        elements.taskGroupDropdown.change(checkSelectedTaskGroupAndUpdateTaskDataTextBoxesAccordingly);
    });
})(jQuery);