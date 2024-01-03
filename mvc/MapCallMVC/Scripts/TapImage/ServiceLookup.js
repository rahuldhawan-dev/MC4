var ServiceLookup = (function ($) {

    const setMessage = (message) => {
        $('#look-up-error').text(message);
    };

    const setData = (data) => {
        // Set the easy stuff first:
        $('#TownSection').val(data.townSection);
        $('#StreetNumber').val(data.streetNumber);
        $('#CrossStreet').val(data.crossStreet);
        $('#ServiceNumber').val(data.serviceNumber);
        $('#PremiseNumber').val(data.premiseNumber);
        $('#Lot').val(data.lot);
        $('#ApartmentNumber').val(data.apartmentNumber);
        $('#Block').val(data.block);
        $('#LengthOfService').val(data.lengthOfService);
        $('#ServiceType').val(data.serviceType);
        $('#ServiceMaterial').val(data.serviceMaterial);
        $('#ServiceSize').val(data.serviceSize);
        $('#PreviousServiceMaterial').val(data.previousServiceMaterial);
        $('#PreviousServiceSize').val(data.previousServiceSize);
        $('#DateCompleted').val(data.dateInstalled);
        $('#IsDefaultImageForService').val(data.isDefaultImageForService);
        $('#CustomerSideMaterial').val(data.customerSideMaterial);
        $('#CustomerSideSize').val(data.customerSideSize);
        $('#OperatingCenter').val(data.operatingCenterId);

        // NOTE: Since this is rarely used, 'refresh' is the event
        //       fired by cascading drop downs when the items refresh.
        $('#Town').one('refresh', function () {
            // streetId can be 0 sadly.
            if (data.streetId) {
                $('#StreetIdentifyingInteger').one('refresh', function () {

                    $('#Service').one('refresh', function () {
                        $('#Service').val(data.serviceId);
                    });

                    $('#StreetIdentifyingInteger').val(data.streetId);
                    $('#StreetIdentifyingInteger').change();
                });
            }

            if (data.crossStreetId) {
                $('#CrossStreetIdentifyingInteger').one('refresh', function () {
                    $('#CrossStreetIdentifyingInteger').val(data.crossStreetId);
                    $('#CrossStreetIdentifyingInteger').change();
                });
            }

            $('#Town').val(data.townId);
            $('#Town').change();
        });

        // This starts the entire cascading event chain.
        $('#OperatingCenter').change();
    };

    return {
        onBegin: () => {
            // Clear out the message.
            setMessage('Searching...');
        },
        onSuccess: (data) => {
            if (!data.success) {
                setMessage(data.message);
            } else {
                setMessage('');
                setData(data);
            }
        },
        onError: () => {
            setMessage("An unexpected error has occurred. Please try again.");
        }
    };

})(jQuery);