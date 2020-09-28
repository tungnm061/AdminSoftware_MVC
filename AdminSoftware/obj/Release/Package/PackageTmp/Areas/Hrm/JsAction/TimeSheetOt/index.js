var record = 0;
$(document).ready(function () {
    $("#btnSearchDate").click(function() {    
        GridCallback("#grdMain");
    });
    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/TimeSheetOt/TimeSheetOts',
                        dataType: "json",
                        data: JSON.stringify({
                             fromDate :  $("#FromDateSearch").data("kendoDatePicker").value(),
                             toDate :    $("#ToDateSearch").data("kendoDatePicker").value()
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
                    id: "TimeSheetOtId",
                    fields: {
                        CreateDate: { type: 'date' },
                        DayDate: { type: 'date' },
                        CoefficientPoint: { type: 'number' },
                        Hours: { type: 'number' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false,
            group: { field: 'DepartmentName' }
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
                width: 50
            },
              {
                  field: "EmployeeCode",
                  title: "Mã nhân viên",
                  width: 140
              },
            {
                field: "FullName",
                title: "Tên nhân viên",
                width: 250
            },
            {
                field: "DepartmentName",
                title: "Phòng chức năng",
                width: 100,
                hidden :true
            },
            {
                field: "DayDate",
                title: "Ngày",
                width: 120,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "Hours",
                title: "Số giờ",
                width: 120
            },
            {
                field: "CoefficientPoint",
                title: "Hệ số",
                width: 100
            },
             {
                 field: "DayPoints",
                 title: "Số công",
                 width: 100
             },
            {
                field: "Description",
                title: "Mô tả",
                width: 200
            }
            

        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

    $('#btnCreate').click(function () {
        InitWindowModal('/hrm/TimeSheetOt/TimeSheetOt', false, 600, 365, 'Thêm mới làm thêm giờ ', false);
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
        InitWindowModal('/hrm/TimeSheetOt/TimeSheetOt?id=' + id, false, 600, 365, 'Cập nhật làm thêm giờ', false);
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
                        url: '/hrm/TimeSheetOt/Delete',
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