$(document).ready(function () {
    $('#Search').click(function () {
        var keyword = $('#Keyword').val();
        if (keyword == null || keyword.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải nhập từ khóa tìm kiếm!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                    $('#Keyword').focus();
                }
            });
            return;
        }
        var newDataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/Praise/Employees',
                        dataType: "json",
                        data: JSON.stringify({
                            keyword: keyword
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
                    id: "EmployeeId"
                }
            }
        });
        var grid = $("#grdEmployees").data("kendoGrid");
        grid.setDataSource(newDataSource);
    });
    $("#grdEmployees").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/Praise/Employees',
                        dataType: "json",
                        data: JSON.stringify({
                            keyword: ''
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
                    id: "CustomerId"
                }
            }
        },
        height: 250,
        filterable: false,
        sortable: false,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "EmployeeCode",
                title: "Mã nhân viên"
            },
            {
                field: "FullName",
                title: "Tên nhân viên"
            },
            {
                field: "DepartmentId",
                title: "Phòng",
                values: departments
            }
        ]
    });
    $('#btnSaveAdd').click(function () {
        var employeeId = GetGridRowSelectedKeyValue("#grdEmployees");
        if (employeeId != null && employeeId != typeof undefined) {
            $('#processing').show();
            $.ajax({
                type: 'POST',
                url: '/hrm/Praise/SaveEmployee',
                dataType: "json",
                data: JSON.stringify({
                    employeeId: employeeId
                }),
                contentType: 'application/json;charset=utf-8',
                success: function (response) {
                    $('#processing').hide();
                    if (response.Status === 0) {
                        $.msgBox({
                            title: "Hệ thống ERP",
                            type: "error",
                            content: response.Message,
                            buttons: [{ value: "Đồng ý" }],
                            success: function() {
                            }
                        });
                    } else {
                        $.msgBox({
                            title: "Hệ thống ERP",
                            type: "info",
                            content: response.Message,
                            buttons: [{ value: "Đồng ý" }],
                            success: function () {
                                $('#Search').click();
                                GridCallback('#grdPraiseDisciplineDetail');
                            }
                        });
                    }
                }
            });
        } else {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn nhân viên!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                    $('#Keyword').focus();
                }
            });
            return;
        }
    });
});