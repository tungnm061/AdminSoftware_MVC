var videoVietnamese;
$(document).ready(function () {
    ClassicEditor.create(document.querySelector('#PostContent'), {
        toolbar: ['heading', '|', 'ckfinder', '|', 'imageUpload', '|', 'bold', 'italic', 'link', 'bulletedList', 'numberedList', 'blockQuote', 'insertTable', 'mediaEmbed', 'undo', 'redo'],
        ckfinder: {
            openerMethod: 'popup',
            uploadUrl: '/   /connector?command=FileUpload&lang=vi&type=Images&responseType=json'
        }
    }).then(e =>
    {
        videoVietnamese = e;
        e.ui.view.editable.editableElement.style.height = '380px';
    });
    $("#PublishDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $('#btnSave').click(function () {
        var model = {
            PostId: postId,
            Title: $('#Title').val(),
            PostContent: videoVietnamese.getData(),
            PublishDate: $('#PublishDate').data('kendoDatePicker').value(),
            IsFeature: $('#IsFeature').is(":checked")
        }
        if (model.PublishDate == null ) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn ngày xuất bản!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.Title == null || model.Title.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa nhập tiêu đề!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.PostContent == null || model.PostContent.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa nhập nội dung bài viết!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/system/Post/Save',
            data: JSON.stringify({ model: model }),
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                $('#processing').hide();
                var type;
                if (response.Status === 1)
                    type = 'info';
                else
                    type = 'error';
                $.msgBox({
                    title: "Hệ thống ERP",
                    type: type,
                    content: response.Message,
                    buttons: [{ value: "Đồng ý" }],
                    success: function (result) {
                        if (result === 'Đồng ý' && response.Status === 1) {
                            CloseWindowModal();
                            GridCallback('#grdMain');
                        }
                    } //
                });
            },
            error: function (response) {
                $('#processing').hide();
                $.msgBox({
                    title: "Hệ thống ERP",
                    type: "error",
                    content: response.Msg,
                    buttons: [{ value: "Đồng ý" }]
                });
            }
        });
    });
});