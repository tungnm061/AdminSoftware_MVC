var recordSalary = 0;
$(document).ready(function () {
    $('#btnDelete').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdSalaries');
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
                        url: '/hrm/Salary/Delete',
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
                                        GridCallback('#grdSalaries');
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
        InitChildWindowModal('/hrm/Salary/Salary', 600, 280, "Thêm mới quá trình tăng giảm lương");
    });
    $('#btnEdit').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdSalaries');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Salary/Salary?id=' + id, 600, 280, "Cập nhật quá trình tăng giảm lương");
    });
    
    $("#grdSalaries").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/Salary/Salaries',
                        dataType: "json",
                        data: JSON.stringify({
                            employeeId: employeeId
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
                    id: "SalaryId",
                    fields: {
                        CreateDate: { type: 'date' },
                        ApplyDate: { type: 'date' },
                        BasicSalary:{ type: 'number'}
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 500,
        filterable: true,
        sortable: true,
        pageable: {
            refresh: true
        },
        selectable: 'row',
        columns: [
            {
                title: "STT",
                template: "#= ++recordSalary #",
                width: 60
            },
            {
                field: "ApplyDate",
                title: "Áp dụng từ",
                width: 170,
                format:'{0:MM/yyyy}'
            },
            {
                field: "BasicSalary",
                title: "Mức lương cơ sở",
                width: 200,
                format:'{0:###,###}'
            },
            {
                field: "BasicCoefficient",
                title: "Hệ số lương CB",
                width: 150
            },
            {
                field: "ProfessionalCoefficient",
                title: "Hệ số chuyện môn",
                width: 150
            },
            {
                field: "ResponsibilityCoefficient",
                title: "Hệ số lương TN",
                width: 150
            },
            {
                field: "CreateBy",
                title: "Người tạo",
                width: 130,
                values: users
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                format: "{0:dd/MM/yyyy}",
                width: 150
            }
        ],
        dataBinding: function () {
            recordSalary = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});