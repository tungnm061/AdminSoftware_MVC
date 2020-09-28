$(document).ready(function () {
    $("#ModalEmployee").click(function () {
        InitChildWindowModal('/hrm/EmployeeHoliday/Employees', 720, 445, "Tìm kiếm nhân viên");
    });
    //$("#Holidays").kendoNumericTextBox({
    //    format: "{0:###,###.##}",
    //    min : 0
    //});
    $("#BtnSearchHolidayReason").click(function () {
        InitChildWindowModal('/hrm/EmployeeHoliday/HolidayReasonSearch', 1000, 550, "Tìm kiếm dữ liệu");
    });
    //$("#HolidayReasonId").kendoDropDownList({
    //    dataTextField: "text",
    //    dataValueField: "value",
    //    dataSource: window.holidayReason,
    //    optionLabel: "Chọn lý do",
    //    change: function () {
    //        $.ajax({
    //            type: "POST",
    //            url: '/hrm/EmployeeHoliday/CreateDetail',
    //            data: JSON.stringify({
    //                fromDate: $('#FromDate').data('kendoDatePicker').value(),
    //                toDate: $('#ToDate').data('kendoDatePicker').value(),
    //                holidayReasonId: $('#HolidayReasonId').data('kendoDropDownList').value()
    //            }),
    //            contentType: 'application/json; charset=utf-8',
    //            success: function (response) {
    //                $('#processing').hide();
    //                GridCallback("#grdMainHolidayDetail");
    //            }

    //        });
    //    }
    //});
    $("#FromDate,#ToDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy",
        change: function (e) {
            $.ajax({
                type: "POST",
                url: '/hrm/EmployeeHoliday/CreateDetail',
                data: JSON.stringify({
                    fromDate: $('#FromDate').data('kendoDatePicker').value(),
                    toDate: $('#ToDate').data('kendoDatePicker').value(),
                    holidayReasonId: $('#HolidayReasonId').val()
                }),
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    $('#processing').hide();
                    GridCallback("#grdMainHolidayDetail");
                }

            });
        }
    });
    $("#grdMainHolidayDetail").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/EmployeeHoliday/HolidayDetails',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "HolidayDetailId",
                    fields: {
                        DateDay: { editable: false, type: 'date' },
                        NumberDays: { editable: true, type: 'number' },
                        PercentSalary: { editable: true, type: 'number' },
                        Permission: { editable: true, type: 'number' }
                    }
                }
            },
            pageSize: 10000,
            serverPaging: false,
            serverFiltering: false
        },
        height: 150,
        filterable: false,
        editable: true,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
             {
                 field: "DateDay",
                 title: "Ngày",
                 width: 200,
                 format : "{0:dd/MM/yyyy}"
             },
              {
                  field: "NumberDays",
                  title: "Số công nghỉ",
                  width: 140,
                  format: "{0:n2}"
              },
            {
                field: "Permission",
                title: "Trừ phép",
                width: 140,
                format: "{0:n2}"

            },
            {
                field: "PercentSalary",
                title: "% Lương",
                width: 220,
                format: "{0:p0}"
            },
            {
                field: "ToTalDays",
                title: "Công nghỉ",
                width: 120,
                hidden: true

            }
        ],
        edit: function(e) {
            var input = e.container.find(".k-input");
            input.change(function() {
                var rowSelected = GetGridRowSelected('#grdMainHolidayDetail');
                if (rowSelected != null && rowSelected != typeof undefined) {
                    if (e.container.find("input[name='NumberDays']").length > 0) {

                        rowSelected.NumberDays = e.container.find("input[name='NumberDays']").val();
                        if (rowSelected.NumberDays > 1) {
                            $.msgBox({
                                title: "Hệ thống",
                                type: "error",
                                content: "Ngày nghỉ không được lớn hơn 1!",
                                buttons: [{ value: "Đồng ý" }],
                                success: function () {
                                }
                            });
                            GridCallback('#grdMainHolidayDetail');
                            return;
                        }
                    }
                    if (e.container.find("input[name='Permission']").length > 0) {
                        rowSelected.Permission = e.container.find("input[name='Permission']").val();
                        if (rowSelected.NumberDays > 1) {
                            $.msgBox({
                                title: "Hệ thống",
                                type: "error",
                                content: "Ngày trừ phép không được lớn hơn 1!",
                                buttons: [{ value: "Đồng ý" }],
                                success: function () {
                                }
                            });
                            GridCallback('#grdMainHolidayDetail');
                            return;
                        }

                    }
                    if (e.container.find("input[name='PercentSalary']").length > 0) {
                        rowSelected.PercentSalary = e.container.find("input[name='PercentSalary']").val();
                    }
                    $('#processing').show();
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/EmployeeHoliday/UpdateDetail',
                        data: JSON.stringify({
                            model: rowSelected
                        }),
                        contentType: 'application/json;charset=utf-8',
                        success: function (response) {
                            GridCallback('#grdMainHolidayDetail');
                            $('#processing').hide();
                        },
                        error: function (response) {
                            $('#processing').hide();
                            $.msgBox({
                                title: "Hệ thống",
                                type: "error",
                                content: response.Msg,
                                buttons: [{ value: "Đồng ý" }]
                            });
                        }
                    });
                }

            });

        }
    });
    $('#btnSave').click(function () {
        var model = {
            EmployeeHolidayId: employeeHolidayId,
            HolidayReasonId: $('#HolidayReasonId').val(),
            FromDate: $('#FromDate').data("kendoDatePicker").value(),
            ToDate: $('#ToDate').data("kendoDatePicker").value(),
            EmployeeId: $('#EmployeeId').val(),
            CreateDate : createDate,
            Description: $('#Description').val()
        };
       
        if (model.EmployeeId == null || model.EmployeeId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn nhân viên!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.HolidayReasonId == null || model.HolidayReasonId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn lí do!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.FromDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn ngày bắt đầu!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.ToDate == null ) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn ngày kết thúc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/EmployeeHoliday/Save',
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