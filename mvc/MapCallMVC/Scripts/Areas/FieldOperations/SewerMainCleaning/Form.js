(($) => {
    const selectors = {
        defectGradeGuide: '.defect-grade-guide',
        inspectionGrade: '#InspectionGrade',
        inspectionType: '#InspectionType'
    };
    
    const toggleDropDownBasedOnThing = (selectorToToggle, shouldToggleP) => {
        if (shouldToggleP()) {
            Application.toggleField(selectorToToggle, true);
        } else {
            Application.toggleField(selectorToToggle, false);
            $(selectorToToggle).val('');
        }
    };
    
    const inspectionTypeChange = () => {
        const inspectionType = $(selectors.inspectionType).val() || 5;
        const show = inspectionType < 3;
        toggleDropDownBasedOnThing(
            selectors.inspectionGrade,
            () => show);
        $(selectors.defectGradeGuide).toggle(show);
    };
    
    const initChangeHandler = (selector, handler) => {
        $(selector).change(handler);
        handler();
    };

    $(() => {
        initChangeHandler(selectors.inspectionType, inspectionTypeChange);
    });
})(jQuery);