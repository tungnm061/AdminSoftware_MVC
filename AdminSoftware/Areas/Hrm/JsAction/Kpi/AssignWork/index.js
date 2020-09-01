var record = 0;

$(document).ready(function () {
    $('#FromDate,#ToDate').kendoDatePicker({
        dateInput: true,
        format: '{0:dd/MM/yyyy}'
    });
    $('#StatusSelect').kendoDropDownList({
        dataTextField: 'text',
        dataValueField: 'value',
        dataSource: statusAssignWorks
    });
    $('#btnSearch').click(function() {
        if ($('#FromDate').data('kendoDatePicker').value() == null || $('#ToDate').data('kendoDatePicker').value() == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn khoảng ngày!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if ($('#StatusSelect').data('kendoDropDownList').value() == null || $('#StatusSelect').data('kendoDropDownList').value().trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn trạng thái công việc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        GridCallback('#grdMain');
    });
    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/AssignWork/AssignWorks',
                        dataType: "json",
                        data: JSON.stringify({
                            fromDate: $('#FromDate').data('kendoDatePicker').value(),
                            toDate: $('#ToDate').data('kendoDatePicker').value(),
                            status: $('#StatusSelect').data('kendoDropDownList').value()
                        }),
                        contentType: 'application/json;charset=utf-8',
                        success: function (response) {
                            options.success(response);
                        }
                    });
                }
            },
            schema: {
                model: {
                    id: "AssignWorkId",
                    fields: {
                        CreateDate: { type: 'date' },
                        Status: { type: 'number' },
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' }
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
                 field: "Status",
                 title: "Trạng Thái",
                 width: 120,
                 values: statusAssignWorks
             },
             {
                 field: "AssignCode",
                 title: "MNV được giao ",
                 width: 180
             },
              {
                  field: "AssignName",
                  title: "Họ và tên ",
                  width: 180
              },
              {
                  field: "TaskCode",
                  title: "Mã công việc",
                  width: 140
              },
              {
                  field: "TaskName",
                  title: "Tên công việc",
                  width: 250
              },
              //    {
              //        field: "DirectorFollowBy",
              //        title: "GĐ theo dõi",
              //        width: 150
              //    },
              //{
              //    field: "DepartmentFollow",
              //    title: "TP theo dõi",
              //    width: 150
              //},
            {
                field: "FromDate",
                title: "Thời gian bắt đầu",
                width: 180,
                format: "{0:dd/MM/yyyy}"

            },
            {
                field: "ToDate",
                title: "Thời gian hoàn thành",
                width: 180,
                format: "{0:dd/MM/yyyy}"

            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                width: 120,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "Description",
                title: "Ghi Chú",
                width: 350
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

    $('#btnCreate').click(function () {
        InitWindowModal('/hrm/AssignWork/AssignWork', false, 680, 370, 'Thêm mới công việc được giao');
    });

    $('#btnEdit').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn dữ liệu trước khi cập nhật!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        InitWindowModal('/hrm/AssignWork/AssignWork?id=' + id, false, 680, 370, 'Cập nhật công việc được giao');
    });

    $('#btnDelete').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn dữ liệu cần xóa!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
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
                        url: '/hrm/AssignWork/Delete',
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
});