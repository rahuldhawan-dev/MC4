(function($) {
  var serviceUrl = $('#video-service-url').val();
  var playButtons = $('.play-button');
  var currentAjaxRequest = null;

  // Dialogs are created before sending out the ajax request so something
  // can be shown as happening to the user in the case of long request times.
  var currentDialog = $('<div class="video-picker">' +
                          '<div class="vp-title">' +
                            '<div><h3 class="vp-title-text"></h3></div>' +
                            '<div><button class="vp-close">X</button></div>' +
                          '</div>' +
                          '<div class="video-container"></div>' +
                        '</div>');

 
  currentDialog.hide();
  $(document.body).append(currentDialog);;

  var methods = {
    init: function() {
      currentDialog.find('.vp-close').click(function() {
        currentDialog.find('.video-container').empty();
        currentDialog.jqmHide();
      });

      // These are the width/height set by default by sproutvideo.
      var baseWidth = 630;
      var baseHeight = 354;
      methods.resizeDialog(baseWidth, baseHeight);

      playButtons.click(function() {
        var videoId = $(this).val();
        methods.beginLoadVideo(videoId);
      });
    },

    beginLoadVideo: function(id) {
      // Display the dialog *before* sending out the ajax request. This way
      // the user will see something happening even if it's just an empty window
      // opening for a second.
      currentDialog.jqm({ modal: true }).jqmShow();

      if (currentAjaxRequest) {
        currentAjaxRequest.abort();
      }

      currentAjaxRequest = $.ajax({
        url: serviceUrl,
        data: {
          id: id
        },
        async: false,
        type: 'GET',
        success: function(result) {
          methods.loadVideo(result);
        },
        error: function() {
          alert('Something went wrong.');
        }
      });
    },

    loadVideo: function(video) {
      // Will want to clear that div first.
      // Then probably make it a dialog or something
     
      // NOTE: The embed code is an iframe. This causes a javascript error in 
      //       Firefox. It's an issue with flash inside iframes and does not seem
      //       to impact any of the functionality of the video player.

      currentDialog.find('.vp-title-text').text(video.Title);
      var vidContainer = currentDialog.find('.video-container');
      vidContainer.append(video.EmbedCode);
      var neat = vidContainer.find('iframe');
      methods.resizeDialog(neat.width(), neat.height());
    },

    resizeDialog: function(width, height) {
      currentDialog.css('width', width + 12); // 12 just for spacing
      currentDialog.css('height', height + 50); // 50 just for spacing;
    }
  };

  methods.init();
})(jQuery);