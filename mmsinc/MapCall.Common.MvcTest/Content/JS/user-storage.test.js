/// <reference path="../../../MMSINC.Testing/Scripts/qunit.js" />
/// <reference path="../../../MMSINC.Testing/Scripts/qmock.js" />
/// <reference path="../../../MMSINC.Testing/Scripts/testHelpers.js" />
/// <reference path="../../../MapCall.Common.Mvc/Content/JS/simpleStorage.js" />
/// <reference path="../../../MapCall.Common.Mvc/Content/JS/user-storage.js" />

//
// NOTE!!!!!!!!!!!!!!!!!!!!!!!!!!
// THESE TESTS FAILS IN IE BECAUSE IE DOES NOT SUPPORT LOCALSTORAGE ON FILES RAN FROM THE LOCAL FILE SYSTEM. 
//
module('user-storage.js');

preserve([],
    function () {
        var uniqueKey;

        var initialize = function () {
            simpleStorage.flush();
            uniqueKey = 'user';
            setCookie('UserStorage=key=user&enabled=true');
        };

        var run = function(testFunc) {
            initialize();
            mock(function () {
                mock.expect('UserStorage._getUniqueKey').mock(function () {
                    return uniqueKey;
                });
                testFunc();
            });
        };

        var setCookie = function(cookie) {
            UserStorage.cookieHelper._getCookieString = function() {
                return cookie;
            }
        };

        // 
        // Tests(obviously)
        //

        test('UserStorage._getUniqueKey returns key from UserStorageKey cookie.', function () {
            // Don't use run() here, we don't want to mock _getUniqueKey.
            initialize();
            
            setCookie('UserStorage=key=someUser');
            equal(UserStorage._getUniqueKey(), 'someUser');
        });

        test('UserStorage._getUniqueKey uses unknown for user if no localStorage cookie exists.', function () {
            // Don't use run() here, we don't want to mock _getUniqueKey.
            initialize();
            setCookie('');
            equal(UserStorage._getUniqueKey(), 'unknown');
        });

        test('UserStorage.isEnabled() is set by UserStorage.enabled cookie.', function () {
            // Don't use run() here.
            initialize();
            setCookie('UserStorage=key=someUser&enabled=true');
            strictEqual(UserStorage.isEnabled(), true);
            setCookie('UserStorage=key=someUser&enabled=false');
            strictEqual(UserStorage.isEnabled(), false);
        });

        test('UserStorage.getPluginContainer returns object that store keys with a unique key as the root.', function () {
            run(function () {
                var plugin = UserStorage.getPluginContainer('plugin');
                plugin.set('property', 'value');

                equal('value', simpleStorage.get('user.plugin.property'));
            });
        });

        test('UserStorage.getPluginContainer can deal with getUniqueKey returning different values', function () {
            run(function () {
                uniqueKey = 'user one';

                var plugin = UserStorage.getPluginContainer('plugin');
                plugin.set('property', 'value');

                equal(simpleStorage.get('user one.plugin.property'), 'value');

                uniqueKey = 'user two';
                plugin.set('property', 'some other value');

                equal(simpleStorage.get('user two.plugin.property'), 'some other value');
            });
        });

        test('UserStorage.getPluginContainer.clear removes a key/value from localStorage for a user.', function () {
            run(function () {
                var plugin = UserStorage.getPluginContainer('plugin');
                plugin.set('property', 'value');
                equal(simpleStorage.get('user.plugin.property'), 'value');

                plugin.clear('property');
                strictEqual(simpleStorage.get('user.plugin.property'), undefined, 'simpleStorage will return undefined when a key is properly deleted.');
            });
        });

        test('UserStorage does not modify localStorage when isEnabled = false', function () {
            run(function () {
                mock.expect('UserStorage.isEnabled').mock(function () {
                    return false;
                });

                var plugin = UserStorage.getPluginContainer('plugin');
                plugin.set('property', 'value');

                equal(plugin.get('property'), 'value', 'A value needs to be returned for the current page if storage is disabled. This is so plugins do not break.');

                strictEqual(simpleStorage.get('user.plugin.property'), undefined, 'localStorage is not supposed to be used when UserStorage.isEnabled = false.');
            });
        });

        test('UserStorage.getPluginContainer.clearAll only deletes keys for specific plugin for the current user.', function () {
            run(function () {
                // Set for the default test user
                var plugin = UserStorage.getPluginContainer('plugin');
                plugin.set('property', 'value');

                // Then set for a different user
                uniqueKey = 'another user';
                plugin.set('property', 'another users value');

                // Ensure both users have the expected values.
                uniqueKey = 'user';
                equal(simpleStorage.get('user.plugin.property'),'value');

                uniqueKey = 'another user';
                equal(simpleStorage.get('another user.plugin.property'), 'another users value');


                // Ensure another user's keys have been deleted while default test user is still the same.
                plugin.clearAll();
                equal(simpleStorage.get('another user.plugin.property'), undefined, 'This should have been deleted.');

                uniqueKey = 'user';
                equal(simpleStorage.get('user.plugin.property'), 'value', 'This should not have been deleted.');
            });
        });

        test('UserStorage.getPluginContainer.getAll returns all of the keys + values for a specific plugin for the current user.', function () {
            run(function() {
                var plugin = UserStorage.getPluginContainer('plugin');
                plugin.set('property', 'value');
                plugin.set('other', 'other value');

                var values = plugin.getAll();
                equal(values.length, 2);
                equal(values[0].key, 'property');
                equal(values[0].value, 'value');
                equal(values[1].key, 'other');
                equal(values[1].value, 'other value');
            });
        });

        test('UserStorage.getPluginContainer.getAll returns all of the keys + values for a specific plugin for the current user when UserStorage.IsEnabled = false.', function () {
            run(function() {
                mock.expect('UserStorage.isEnabled').mock(function() {
                    return false;
                });
                var plugin = UserStorage.getPluginContainer('plugin');
                plugin.set('property', 'value');
                plugin.set('other', 'other value');

                var values = plugin.getAll();
                equal(values.length, 2);
                equal(values[0].key, 'property');
                equal(values[0].value, 'value');
                equal(values[1].key, 'other');
                equal(values[1].value, 'other value');
            });
        });
    });
// leave this at the bottom: 
// preserve()
