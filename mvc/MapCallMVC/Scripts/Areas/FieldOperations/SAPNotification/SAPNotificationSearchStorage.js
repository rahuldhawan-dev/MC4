// This is a one-off script that's sorta similar to what action-bar.js does to store
// and repopulate search parameters. action-bar.js only saves the most recent search
// for the entire app, so trying to get the results for a different controller/action
// is not going to work.
//
// It's worth noting this has the same shortcoming as the action-bar.js code. If the user
// has two different search result windows open, the last one they opened is going to be
// the result used to repopulate any links.
const SAPNotificationSearchStorage = (($) => {
    const searchStorage = UserStorage.getPluginContainer('SAPNotificationSearchStorage');

    const methods = {
        trySaveLastSearch: () => {
            if (Application.routeData.controller === 'SapNotification' && Application.routeData.action === 'Index') {
                searchStorage.set('lastSearch', window.location.search);
            }
        },
        getLastSearch: () => {
            return searchStorage.get('lastSearch');
        }
    };

    $(document).ready(methods.trySaveLastSearch);

    return methods;
})(jQuery);