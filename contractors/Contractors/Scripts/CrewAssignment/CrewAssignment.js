var CrewAssignment = {
    'initialize': function() {
        CrewAssignment.initializeAssignments();
    },
    'initializeAssignments': function() {
        CrewAssignment.initializeStartableAssignments();
        CrewAssignment.initializeEndableAssignments();
    },
    'initializeStartableAssignments': function() {
        // Tells the form that it needs to http-post the url in the link.
        $(document).on('click', '#assignmentsTable a.start', function() {
            var href = $(this).attr('href');
            var assForm = $('#startAssignmentForm');
            assForm.attr('action', href);
            assForm.submit();
            return false;
        });
    },
    'initializeEndableAssignments': function() {
        $(document).on('click', '#assignmentsTable a.end', function() {
            var frm = CrewAssignment.getEndAssignmentForm($(this));
            frm.trigger({ 'type': 'submit', 'currentTarget': frm[0] });
            return false;
        });
    },
    'getEndAssignmentForm': function($endLink) {
        var row = $endLink.closest('tr');
        var frm = row.find('form');
        if (frm.length == 0) {
            frm = CrewAssignment.createFormFromAjaxLink($endLink);
            row.find('.formGoesHere').wrap(frm);
            // We have to re-query for the form so we return
            // a form object that is aware of its wrap children. 
            frm = $('#' + frm.attr('id'));
            $.validator.unobtrusive.parseDynamicContent('#' + frm.attr('id'));
        }
        return frm;
    },
    'createFormFromAjaxLink': function($link) {
        var frm = $('<form method="post"></form>');
        frm.attr('action', $link.attr('href'));
        frm.attr('id', 'form-' + Math.floor(Math.random() * 10000));
        frm.submit(function(e) {
            // This prevents the unobtrusive ajax library from
            // bubbling a child form submit up to the parent form.
            e.stopPropagation();
            return true;
        });
        return frm;
    }
};

$(document).ready(CrewAssignment.initialize);