// NOTE: NONE OF THIS WORKS WITH IE7 BECAUSE SIMPLESTORAGE DOES NOT WORK WITH IE7

// This is not meant to be global storage for every use on a machine. Make a
// GlobalStorage thing or something if that's needed.
window.UserStorage = (function (storage) {

    // If there's any major changes to how keys are stored, we'll want to add
    // a schema property that can be checked for. If the schema's changed,
    // we can then delete all the localStorage keys and start again.

    var _isIE7 = (navigator.userAgent.indexOf('MSIE 7.0') > -1);
   
    var methods = {

        // TODO: This property needs to be disabled for IE7 and while doing regression tests.
        //       The plugin container should "work" but it just won't store anything when the page changes.
        isEnabled: function() {
            if (_isIE7) {
                return false;
            }

            var cookie = cookieHelper.get().UserStorage;

            if (!cookie) {
                return false;
            }
            return cookie.enabled;
        },

        // This exists so that plugins using UserStorage do not need
        // to worry about things breaking due to storage being disabled.
        // Only difference is that things stored will not persist across
        // page loads.
        _disabledStorage: (function () {
            var obj = {};

            return {
                get: function (key) {
                    return obj[key];
                },
                set: function (key, value) {
                    obj[key] = value;
                    return true; // Mimic the simpleStorage implementation.
                },
                index: function () {
                    var keys = [];
                    for (var k in obj) {
                        keys.push(k);
                    }
                    return keys;
                },
                deleteKey: function (key) {
                    delete obj[key];
                }
            };
        })(),

        // This method should not be replaced, it's only replacable for unit testing.
        _getUniqueKey: function () {
            var cookie = cookieHelper.get().UserStorage;
            if (cookie && cookie.key) {
                return cookie.key;
            }
            return 'unknown';
        },

        // Returns a wrapper object that sandboxes a plugin's storage to
        // its own keys. 
        getPluginContainer: function (pluginName) {
            return methods._createPluginContainer(pluginName);
        },

        _createPluginContainer: function (pluginName) {
            var getRootKey = function() {
                 return methods._getUniqueKey() + '.' + pluginName + '.';
            };

            var getKeyForSection = function (section) {
                if (!section) {
                    throw 'A UserStorage key may not be null or empty.';
                }
                return getRootKey() + section;
            };

            var container = {
                // Gets the stored value for a plugin property.
                get: function (section) {
                    // NOTE: This will return different instances of the object
                    //       each time it is saved, so it's not recommend to keep
                    //       the object cached anywhere. Also because the localStorage
                    //       can be updated from multiple tabs/windows and
                    //       the other windows should be able to get updated info.
                    //       Also there's events and stuff for the other windows!
                    var key = getKeyForSection(section);
                    return methods._getStorageInstance().get(key);
                },

                // Sets the stored value for a plugin property.
                set: function (section, value) {
                    // storage.get will not return the same js object that is passed
                    // to storage.set. HOWEVERS! storage.get will always return the same
                    // object.
                    var key = getKeyForSection(section);
                    var storageInstance = methods._getStorageInstance();
                    storageInstance.set(key, value);
                    return storageInstance.get(key, value);
                },

                // Removes a key entirely from storage.
                clear: function(key) {
                    var storageInstance = methods._getStorageInstance();
                    storageInstance.deleteKey(getRootKey() + key);
                },

                // Deletes all storage for this plugin.
                clearAll: function() {
                    var all = container.getAll();
                    for (var i = 0; i < all.length; i++) {
                        container.clear(all[i].key);
                    }
                },

                // Returns all the stored values for a plugin.
                getAll: function() {
                    var pluginValues = [];
                    var storageInstance = methods._getStorageInstance();
                    var index = storageInstance.index();
                    var rootKey = getRootKey();
                    for (var i = 0; i < index.length; i++) {
                        var key = index[i];
                        if (key.indexOf(rootKey) === 0) {
                            key = key.split(rootKey)[1];
                            pluginValues.push({
                                key: key,
                                value: container.get(key)
                            });
                        }
                    }
                    return pluginValues;
                }
            };

            return container;
        },

        // Returns the storage implementation required for the page.
        // The instance returned should not be cached! Only cache
        // on a per-function basis.
        _getStorageInstance: function () {
            if (methods.isEnabled()) {
                return storage;
            } else {
                return methods._disabledStorage;
            }
        }
    };

    var cookieHelper = {

        // This is needed for unit testing. Chrome
        // does not allow file:/// accessed files to
        // store anything in document.cookie for some reason.
        _getCookieString: function () {
            return document.cookie;
        },

        // Returns the current document.cookie as a json object.
        get: function () {
            var cookie = {};
            var docCookie = cookieHelper._getCookieString();
            if (docCookie) {
                var cookies = docCookie.split('; ');
                for (var i = 0; i < cookies.length; i++) {
                    var curString = cookies[i];
                    var firstEquals = curString.indexOf('=');
                    var name = curString.substring(0, firstEquals);
                    var value = curString.substring(firstEquals + 1, (curString.length));

                    if (value.indexOf('=') === -1) {
                        cookie[name] = value;
                    }
                    else {
                        var subKeyValues = value.split('&');
                        var subCookie = {};

                        for (var sub = 0; sub < subKeyValues.length; sub++) {
                            var subSplit = subKeyValues[sub].split('=');
                            var subName = subSplit[0];
                            var subValue = cookieHelper._getSafeValue(subSplit[1]);
                            subCookie[subName] = subValue;
                        }

                        cookie[name] = subCookie;
                    }
                }
            }
            return cookie;
        },
        _getSafeValue: function (val) {
            if (val === 'true') {
                return true;
            }
            else if (val === 'false') {
                return false;
            }
            return val;
        }
    };

    methods.cookieHelper = cookieHelper;

    return methods;
})(simpleStorage);