var recordHoliday = 0;
$(document).ready(function () {
    $('#btnAddHolidayReason').click(function () {
        var entityGrid = $("#grdMainHolidayReason").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        $.ajax({
            type: "POST",
            url: '/hrm/EmployeeHoliday/CreateDetail',
            data: JSON.stringify({
                fromDate: $('#FromDate').data('kendoDatePicker').value(),
                toDate: $('#ToDate').data('kendoDatePicker').value(),
                holidayReasonId: selectedItem.HolidayReasonId
            }),
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                $('#processing').hide();
                GridCallback("#grdMainHolidayDetail");
            }

        });
        
        if (selectedItem != null && selectedItem != typeof undefined) {
            $('#HolidayReasonId').val(selectedItem.HolidayReasonId);
            $('#ReasonName').val(selectedItem.ReasonName);          
            CloseChildWindowModal();
           
        } else {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn lý do!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
    });
    $('#btnDeleteHolidayReason').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainHolidayReason');
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
                        url: '/hrm/EmployeeHoliday/DeleteHolidayReason',
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
                                        GridCallback('#grdMainHolidayReason');
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    });
    $('#btnCreateHolidayReason').bind("click", function () {
        InitChildOfChildWindowModal('/hrm/EmployeeHoliday/HolidayReason', 600, 360, "Thêm mới lý do nghỉ");
    });
    $('#btnEditHolidayReason').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainHolidayReason');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildOfChildWindowModal('/hrm/EmployeeHoliday/HolidayReason?id=' + id,  600, 360, "Cập nhật lý do nghỉ");
    });
    $("#grdMainHolidayReason").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/EmployeeHoliday/HolidayReasons',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "HolidayReasonId",
                    fields: {
                        CreateDate: { type: 'date' },
                        IsActive: { type: 'boolean' }
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
                template: "#= ++recordHoliday #",
                width: 60
            },
            {
                field: "ReasonCode",
                title: "Mã lý do",
                width: 150
            },
            {
                field: "ReasonName",
                title: "Lý do",
                width: 200
            },
              {
                  field: "PercentSalary",
                  title: "% Nhận lương",
                  width: 200,
                  format: "{0:p0}"
              },
            {
                field: "Description",
                title: "Ghi chú",
                minwidth:250
            },
            {
                field: "IsActive",
                title: "Theo dõi",
                width: 150,
                template: '<div class="text-center"><i class="#= IsActive ? "glyphicon glyphicon-ok" : "glyphicon glyphicon-ban-circle" #"></i></div>'
            }
        ],
        dataBinding: function () {
            recordHoliday = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});