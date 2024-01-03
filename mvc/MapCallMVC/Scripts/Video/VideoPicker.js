// TODO: Move this to a common spot.
var SimpleService = function(serviceUrl, onSuccess, onError) {
  this.currentAjaxRequest = null;
  this.onSuccess = onSuccess;
  this.onError = onError;
  this.serviceUrl = serviceUrl;
  return this;
};
SimpleService.prototype = {
  beginCall: function(data) {
    var self = this;
    if (this.currentAjaxRequest) {
      this.currentAjaxRequest.abort();
    }

    this.currentAjaxRequest = $.ajax({
      url: this.serviceUrl,
      async: false,
      data: data,
      type: 'GET',
      success: function(result) {
        self.onSuccess(result);
      },
      error: function() {
        self.onError();
      }
    });
  }
};

var VideoPicker = (function($) {
  // Dialogs are created before sending out the ajax request so something
  // can be shown as happening to the user in the case of long request times.
  var currentDialog = $('<div class="video-picker">' +
                          '<div class="vp-title">' +
                            '<div><h3 class="vp-title-text">Select a Video</h3></div>' +
                            '<div><button class="vp-close">X</button></div>' +
                          '</div>' +
                          '<div><select class="vp-tags"></select></div>' +
                          '<div style="overflow:auto; height:100%;"><table class="vp-items"></table></div>' +
                        "</div>");

  currentDialog.find('.vp-close').click(function() {
    currentDialog.jqmHide();
    currentDialog.find('table').empty();
  });
  currentDialog.hide();

  var methods = {
    init: function() {
      $(document.body).append(currentDialog);
      $('#add-video-button').click(function() {
        methods.beginLoadThumbs();
      });

      var videoPickerServiceUrl = $('#video-picker-service-url').val();
      var videoTagServiceUrl = $('#video-tag-service-url').val();
      methods._videoService = new SimpleService(videoPickerServiceUrl, methods.loadThumbs, function() {
        alert('Something went wrong.');
      });

      methods._tagService = new SimpleService(videoTagServiceUrl, methods.loadTags, function() { alert("Error"); });

      $('.vp-tags').change(function() {
        var vidRows = currentDialog.find('.vp-item');
        var selected = $(this).find('option:selected').val();
        if (!selected) {
          vidRows.show();
        }
        else {
          vidRows.each(function(i, el) {
            var data = $(el).data('video-picker');
            if (data.Tags.indexOf(selected) > -1) {
              $(el).show();
            } else {
              $(el).hide();
            }
          });
        }
      });
    },

    beginLoadThumbs: function() {
      // Show dialog before starting call so it appears something is happening to the user.
      currentDialog.jqm({ modal: true }).jqmShow();

      methods._tagService.beginCall();
      methods._videoService.beginCall();
    },

    createThumb: function(video) {
      var div = $('<tr class="vp-item"><td><img /></td><td class="vp-details"><h4>' + video.Title + '</h4><div class="vp-upload-date">Uploaded: ' + video.CreatedAt + '</td></div></tr>');
      div.find('img').attr('src', video.Thumbnail);
      div.data('video-picker', video);
      div.click(function() {
        var vidField = $('#SproutVideoId');
        vidField.val(video.Id);
        vidField.parent('form').submit();
      });
      return div;
    },

    loadThumbs: function(videos) {
      var table = currentDialog.find('table');
      for (var i = 0; i < videos.length; i++) {
        table.append(methods.createThumb(videos[i]));
      }
    },

    loadTags: function(tags) {
      var select = currentDialog.find('.vp-tags');
      select.empty();
      select.append($('<option value="">Filter by Tag</option>'));

      for (var i = 0; i < tags.length; i++) {
        var option = $('<option></option>')
          .attr('value', tags[i].TagId)
          .text(tags[i].Description);
        select.append(option);
      }
    }
  };

  methods.init();
  return methods;
})(jQuery);

