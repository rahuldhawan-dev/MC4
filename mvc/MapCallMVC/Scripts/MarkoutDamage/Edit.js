var MarkoutDamage = (function($) {

    var md = {
        getAddress: function () {
        	var state = $('#State option:selected').text();
	        if (state == "Select an operating center above" || state == "-- Select --")
		        state = "";
	        var town = $('#Town option:selected').text();
	        if (town == "Select a county above" || town == "-- Select --")
	        	town = "";
	        var street = $('#Street').val();
	        var ret = street + ', ' + town + ', ' + state;

	        return (ret != ", , ") ? ret : "";
        },
        setTicketMessage: function(msg) {
            $('#ticket-message').html(msg);
        },
        initTicketFinder: function() {
            var url = $('#by-request-number-url').val();

            $('#populate-ticket-button').click(function () {
                if (!$('#RequestNumber').valid()) {
                    return;
                }

                var reqNum = $('#RequestNumber').val();
                var opCenter = $('#OperatingCenter').val();
                if (!reqNum) {
                    md.setTicketMessage('You must enter a request number first.');
                    return;
                }
                else if (!opCenter) {
                    md.setTicketMessage('You must select an operating center first.');
                    return;
                } else {
                    md.setTicketMessage('Loading...');
                }

                $.ajax({
                    url: url,
                    data: {
                        requestNumber: reqNum
                    },
                    async: false,
                    type: 'POST',
                    success: function (ticket) {
                        md.setTicketMessage('');
                        $('#Excavator').val(ticket.excavator);
                        $('#ExcavatorAddress').val(ticket.excavatorAddress);
                        $('#ExcavatorPhone').val(ticket.excavatorPhone);
                        $('#Street').val(ticket.street);
                        $('#NearestCrossStreet').val(ticket.nearestCrossStreet);

                        // NOTE: "one" isn't a typo, it's jQuery's "fire this event handler one time only" thing.
                        $('#County').one('refresh', function () {
                            $('#Town').one('refresh', function () {
                                $('#Town').val(ticket.townId).change();
                            });

                            $('#County').val(ticket.countyId).change();
                        });

                        $('#State').val(ticket.stateId).change();
                    },
                    error: function () {
                        md.setTicketMessage("Unable to prepopulate data from the given request number.");
                    }
                });
            });
        }
    };

    $(document).ready(md.initTicketFinder);

    return md;
})(jQuery);