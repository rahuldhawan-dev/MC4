var DocumentNew = {
    initialize: function () {
        $('#Save').attr('disabled', 'disabled');
    },
    onComplete: function (result) {
        if (result.success) {
            $('#FileName').val(result.fileName);
            $('#FileName').valid(); 
            $('#Save').removeAttr('disabled');
        } else {
            alert("An unknown error occurred while uploading. Please try again.");
        }
    }
};

$(document).ready(DocumentNew.initialize);