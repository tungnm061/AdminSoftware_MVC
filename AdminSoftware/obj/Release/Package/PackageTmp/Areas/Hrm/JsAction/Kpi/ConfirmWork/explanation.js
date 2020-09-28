$(document).ready(function () {
  
    $("#btnSaveExplanation").click(function () {
        $("#Explanation").val($("#Text").val());
        CloseChildWindowModal();
    });

});