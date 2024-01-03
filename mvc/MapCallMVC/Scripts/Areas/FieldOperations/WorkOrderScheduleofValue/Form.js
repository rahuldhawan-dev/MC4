var ScheduleOfValueForm = (function ($) {
    const LABOR_SCHEDULE_OF_VALUE_CATEGORY = 21;
    const OTHER_SCHEDULE_OF_VALUE_CATEGORIES = [ 27, 28 ];
    var m = {
        initialize: function () {
            ELEMENTS = {
                scheduleOfValueCategory: $('#ScheduleOfValueCategory'),
                otherDescription: $('#OtherDescription'),
                isOvertime: $('#IsOvertime')
            },
            m.initScheduleOfValueCategory();
        },
        initScheduleOfValueCategory: function () {
            ELEMENTS.scheduleOfValueCategory.on('change', m.onScheduleOfValueCategoryChanged);
            m.onScheduleOfValueCategoryChanged();
        },
        onScheduleOfValueCategoryChanged: function () {
            const selectedScheduleOfValueCategoryId = parseInt(ELEMENTS.scheduleOfValueCategory.val(), 10);
            if (selectedScheduleOfValueCategoryId == LABOR_SCHEDULE_OF_VALUE_CATEGORY) {
                ELEMENTS.isOvertime.prop('disabled', false);
            } else {
                ELEMENTS.isOvertime.prop('disabled', true);
            }

            if (OTHER_SCHEDULE_OF_VALUE_CATEGORIES.includes(selectedScheduleOfValueCategoryId)) {
                ELEMENTS.otherDescription.prop('disabled', false);
            } else {
                ELEMENTS.otherDescription.prop('disabled', true);
            }
        }
    };
    $(document).ready(m.initialize);
    return m;
})(jQuery);