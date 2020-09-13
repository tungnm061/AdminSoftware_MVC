var record = 0;
$(document).ready(function () {
    $('#btnDelete').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $.msgBox({
            title: "Hệ thống",
            type: "confirm",
            content: "Bạn có chắc chắn muốn xóa dữ liệu không?",
            buttons: [{ value: "Đồng ý" }, { value: "Hủy bỏ" }],
            success: function (result) {
                if (result === "Đồng ý") {
                    $('#processing').show();
                    $.ajax({
                        type: 'POST',
                        url: '/system/Post/Delete',
                        data: JSON.stringify({ id: id }),
                        contentType: 'application/json;charset=utf-8',
                        success: function (response) {
                            $('#processing').hide();
                            if (response.Status === 0) {
                                $.msgBox({
                                    title: "Hệ thống",
                                    type: "error",
                                    content: response.Message,
                                    buttons: [{ value: "Đồng ý" }],
                                    success: function () {
                                    }
                                });
                            } else {
                                $.msgBox({
                                    title: "Hệ thống",
                                    type: "info",
                                    content: response.Message,
                                    buttons: [{ value: "Đồng ý" }],
                                    success: function () {
                                        GridCallback('#grdMain');
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    });
    $('#btnCreate').bind("click", function () {
        InitWindowModal('/system/Post/Post', true, "", "", "Thêm mới bài viết", false);
    });
    $('#btnEdit').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitWindowModal('/system/Post/Post?id=' + id, true, "", "", "Cập nhật bài viết", false);
    });

    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: '/system/Post/Posts',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "PostId",
                    fields: {
                        CreateDate: { type: 'date' },
                        PublishDate :{type:'date'}
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: gridHeight,
        filterable: true,
        sortable: true,
        pageable: {
            refresh: true
        },
        selectable: 'row',
        columns: [
            {
                title: "STT",
                template: "#= ++record #",
                width: 60
            },
            {
                field: "Title",
                title: "Tiêu đề",
                width: 200
            },
            {
                field: "PostContent",
                title: "Nội dung",
                width: 400
            },
              {
                  field: "PublishDate",
                  title: "Ngày xuất bản",
                  width: 200,
                  format: "{0:dd/MM/yyyy}"
              },
            {
                field: "CreateBy",
                title: "Người tạo",
                width: 200,
                values: users
            },
             {
                 field: "CreateDate",
                 title: "Ngày tạo",
                 width: 200,
                 format:"{0:dd/MM/yyyy}"
             },
              {
                  field: "IsFeature",
                  title: "Nổi bật",
                  width: 140,
                  template: '<div class="text-center"><i class="#= IsFeature ? "glyphicon glyphicon-ok" : "glyphicon glyphicon-ban-circle" #"></i></div>'

              }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});