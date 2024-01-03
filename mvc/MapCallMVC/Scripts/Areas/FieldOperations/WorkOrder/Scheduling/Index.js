var WorkOrders = (function () {
    var workOrderUrl;
    var assignedFor;

    var sm = {
        init: function () {
            workOrderUrl = $('#WorkOrderUrl').val();
            ELEMENTS = {
                assignedFor: $('#AssignFor'),
                btnAssign: $('#Assign')              
            };
            ELEMENTS.btnAssign.on('click', sm.onBtnAssignClick);
            sm.onBtnAssignClick();
        },

        onBtnAssignClick: function () {
            $("#workOrdersTable input[type=checkbox]:checked").each(function () {
                var id = $(this).closest("tr").find("td:first").find("input:first").attr("value");
                $.ajax({
                    url: workOrderUrl,
                    type: 'GET',
                    data: {
                        id: id,
                        assignedFor: ELEMENTS.assignedFor.val()
                    },
                    async: false,
                    success: function (result) {
                        if (result.Notification) {
                            alert(result.Notification);
                            event.preventDefault();
                            return;
                        }
                    }
                });                                
            });
        }
    }

    $(document).ready(sm.init);

    return sm;
})();
