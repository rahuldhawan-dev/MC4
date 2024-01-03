var MapAction = (function () {

    function initializeClickHandler(modelCount, maxResults) {
        if (modelCount > maxResults) {
            $('.ab-map').click(() => {
                alert(`Only the first ${maxResults} records will be shown on the map.`);
            });
        }
    }
    
    return {
       initializeClickHandler
    };
})();
