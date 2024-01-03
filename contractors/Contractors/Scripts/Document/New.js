var DocumentNew = {
    initialize: function () {
        // Need to manually init the file uploader since it's coming from a partial.
        $('.file-upload').unobtrusiveUploader();
        $('#Save').attr('disabled', 'disabled');
    },
    onComplete: function(result) {
        if (result.success) {
            $('#FileName').val(result.fileName);
            $('#Save').removeAttr('disabled');
        } else {
            alert("An unknown error occurred while uploading. Please try again.");
        }
        
    }
};

$(document).ready(DocumentNew.initialize);
