$(document).ready(function () {

    $('#btnSave').click(function () {
        var formData = new FormData();
        var file = document.getElementById("attachment").files[0];
        formData.append("files", file);
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/sale/Listting/ImportExcel',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                document.getElementById("attachment").value = "";
                document.getElementById('btnAttachment').value = "File";
                $('#processing').hide();
                var type;
                if (response.Status === 1)
                    type = 'info';
                else
                    type = 'error';
                $.msgBox({
                    title: "Hệ thống",
                    type: type,
                    content: response.Message,
                    buttons: [{ value: "Đồng ý" }],
                    success: function (result) {
                        if (result == 'Đồng ý' && response.Status === 1) {
                            CloseWindowModal();
                            GridCallback('#grdMain');
                        }
                    } //
                });
            },
            error: function (response) {
                document.getElementById("attachment").value = "";
                document.getElementById('btnAttachment').value = "File";
                $('#processing').hide();
                $.msgBox({
                    title: "Hệ thống",
                    type: "error",
                    content: response.Msg,
                    buttons: [{ value: "Đồng ý" }]
                });
            }
        });
    });
});