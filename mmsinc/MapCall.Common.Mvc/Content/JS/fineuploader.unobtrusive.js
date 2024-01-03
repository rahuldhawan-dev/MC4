(function ($) {

    var DATA_KEY = "unobtrusiveUploaderDataKey";

    var methods = {
        initialize: function ($el, options) {
            options = options || {};
            if (options === 'destroy') {
                // This is dumb and doesn't actually destroy anything.
                // Also it throws an exception in IE9.
                //$el.data(DATA_KEY).reset();
            }
            else if (!$el.data(DATA_KEY)) {
                var created = methods.createUploader($el, options);
                $el.data(DATA_KEY, created);
                return created;
            }
        },

        createUploader: function ($el, options) {
            var fileAttr = function (key) {
                return $el.attr('data-file-' + key);
            };

            return new qq.FineUploader({
                element: $el[0],
                request: {
                    endpoint: fileAttr('url'),
                    inputName: fileAttr('inputname')
                },
                text: {
                    uploadButton: fileAttr('buttontext') || 'Upload'
                },
                validation: {
                    allowedExtensions: methods.getAllowedExtensions(fileAttr('ext'))
                },

                callbacks: {
                    onComplete: function (fileNumber, fileName, result) {

                        // If the internal onComplete call throws an error, our handler
                        // is still called anyway. We get an empty result object,
                        // so return a success: false so callers don't have to worry about it.
                        if (!result || $.isEmptyObject(result)) {
                            result = { success: false };
                        } 

                        if (result.success) {
                            $('[name="' + fileAttr('keyelement') + '"]').val(result.key);
                        }

                        // Should this callback be inlined as an attribute or attachable
                        // after the fact by external scripts?
                        var callbackMethodName = fileAttr("oncomplete");
                        if (callbackMethodName) {
                            var callbackMethod = eval(callbackMethodName);
                            callbackMethod(result);
                        }

                        if (options.onComplete) {
                            options.onComplete(result);
                        }
                    }
                }
            });
        },

        getAllowedExtensions: function (extString) {
            if (!extString) {
                // FineUploader looks for an empty array if there are no ext restrictions.
                return [];
            }
            return extString.split(',');
        }
    };

    $.fn.unobtrusiveUploader = function (options) {
        return this.each(function () {
            return methods.initialize($(this), options);
        });
    };

    $(document).ready(function () {
        $('.file-upload').unobtrusiveUploader();
    });

})(jQuery);