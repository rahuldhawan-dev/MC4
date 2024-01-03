
/*
 * Red Tag Permits - validation and such.
 * 
 * As of 4/29/2021, there is no built-in support for an attribute validation of making sure a view model property
 * of type DateTime is less than a hard coded value, such as 'Today', or '4/29/2021'. We can only compare one date
 * property field against another and specify operators. Thus, this JS lives. 
 * 
 * TODO: Consider supporting something like this as a validation attribute in MapCall.
 */
const RedTagPermit = (() => {
    let impairedOnTextbox = null;
    let restoredOnTextbox = null;

    const today = new Date();
    const mostRecentMidnite = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 0, 0, 0);

    /* 
     *  Let's cache our HTML elements so we do not need to refetch, as well as set
     *  today to the most recent midnite.
     */
    window.addEventListener('DOMContentLoaded', () => {
        impairedOnTextbox = document.getElementById('EquipmentImpairedOn');
        restoredOnTextbox = document.getElementById('EquipmentRestoredOn');
    });

    /*
     * Let's attempt to validate whether a given string is a date, and whether or not that 
     * string occurs in time after or equal to the most recent midnite.
     * 
     * @dateStringValue - a string that may or may not be parsable to a date
     * 
     * @returns true if the given string is a date and greater than the most recent midnite, else false
     */
    const isDateAfterOrEqualToMostRecentMidnite = (dateStringValue) => {
        if (dateStringValue === null ||
            dateStringValue === undefined || 
            dateStringValue === '') {
            return false;
        }

        const dateToTest = Date.parse(dateStringValue);

        if (isNaN(dateToTest)) {
            return false;
        }

        return new Date(dateToTest) >= mostRecentMidnite;
    }

    /*
     * Expose our public contract to the world
     */
    return {
        validatePrecautions: () => {
            return document.querySelectorAll('#red-tag-permit-precautions input[type=checkbox]:checked')
                           .length > 0;
        },
        validateEquipmentImpairedOn: () => {
            return isDateAfterOrEqualToMostRecentMidnite(impairedOnTextbox.value);
        },
        validateEquipmentRestoredOn: () => {
            return isDateAfterOrEqualToMostRecentMidnite(restoredOnTextbox.value);
        }
    }
})();