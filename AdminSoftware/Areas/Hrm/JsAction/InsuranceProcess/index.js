var recordMedia = 0;
$(document).ready(function () {
    $('#btnDeleteProcess').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainProcess');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $.msgBox({
            title: "Hệ thống ERP",
            type: "confirm",
            content: "Bạn có chắc chắn muốn xóa dữ liệu không?",
            buttons: [{ value: "Đồng ý" }, { value: "Hủy bỏ" }],
            success: function (result) {
                if (result === "Đồng ý") {
                    $('#processing').show();
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/Employee/DeleteProcess',
                        data: JSON.stringify({ id: id }),
                        contentType: 'application/json;charset=utf-8',
                        success: function (response) {
                            $('#processing').hide();
                            if (response.Status === 0) {
                                $.msgBox({
                                    title: "Hệ thống ERP",
                                    type: "error",
                                    content: response.Message,
                                    buttons: [{ value: "Đồng ý" }],
                                    success: function () {
                                    }
                                });
                            } else {
                                $.msgBox({
                                    title: "Hệ thống ERP",
                                    type: "info",
                                    content: response.Message,
                                    buttons: [{ value: "Đồng ý" }],
                                    success: function () {
                                        GridCallback('#grdMainProcess');
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    });
    $('#btnCreateProcess').bind("click", function () {
        var employeeId = $("#EmployeeMediaId").val();

        InitChildOfChildWindowModal('/hrm/Employee/InsuranceProcess?employeeId=' + employeeId, 600, 350, "Thêm mới quá trình tham gia BHXH");
    });
    $('#btnEditProcess').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainProcess');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildOfChildWindowModal('/hrm/Employee/InsuranceProcess?id=' + id, 600, 350, "Cập nhật thông tin bảo hiểm");
    });
    $("#grdMainProcess").kendoGrid({
        dataSource: {
            transport: {     
                read: function (options) {
                    $.ajax(
                            {
                                type: 'POST',
                                url: '/hrm/Employee/InsuranceProcesses',
                                dataType: "json",
                                data: JSON.stringify({
                                    employeeId: $("#EmployeeMediaId").val()
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
                    id: "InsuranceProcessId",
                    fields: {
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date',nullable:true},
                        Amount:{type:'number'}
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
        pageable: false,
        selectable: 'row',
        columns: [
            {
                title: "STT",
                template: "#= ++recordMedia #",
                width: 60
            },
            {
                field: "InsuranceNumber",
                title: "Mã BHXH",
                width: 150
            },
            {
                field: "FromDate",
                title: "Ngày bắt đầu",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "ToDate",
                title: "Ngày kết thúc",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "Amount",
                title: "Mức đóng",
                width: 150,
                format: "{0:###,###}"
            },
             {
                 field: "Description",
                 title: "Ghi chú",
                 width: 300
             },
            {
                field: "CreateBy",
                title: "Người tạo",
                width: 150,
                values: users
            }
        ],
        dataBinding: function () {
            recordMedia = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
}); 