(($) => {
    const OptionalVisibilityItems = {
        initialize: () => {
            $('#HasModifiedDiscipline').change(OptionalVisibilityItems.onModifiedStatusChange);
            // Fire once to init the field visibility.  
            OptionalVisibilityItems.onModifiedStatusChange();
        },

        onModifiedStatusChange: () => {
            var isVisibleForModification = $('#HasModifiedDiscipline').is(':checked');
            Application.toggleField('#ModifiedAccountabilityActionTakenType', isVisibleForModification);
            Application.toggleField('#ModifiedAccountabilityActionTakenDescription', isVisibleForModification);
            Application.toggleField('#DateModified', isVisibleForModification);
            Application.toggleField('#ModifiedDisciplineAdministeredBy', isVisibleForModification);
            Application.toggleField('#ModifiedStartDate', isVisibleForModification);
            Application.toggleField('#ModifiedEndDate', isVisibleForModification);
            Application.toggleField('#ModifiedNumberOfWorkDays', isVisibleForModification);
            Application.toggleField('#BackPayRequired', isVisibleForModification);
        }
    };

    $(OptionalVisibilityItems.initialize);

})(jQuery);