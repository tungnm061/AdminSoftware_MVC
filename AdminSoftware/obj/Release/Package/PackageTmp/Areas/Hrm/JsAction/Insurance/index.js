var recordInsurance = 0;
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
                        url: '/hrm/Employee/DeleteInsurance',
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
        InitChildOfChildWindowModal('/hrm/Employee/Insurance', 600, 400, "Thêm mới thông tin bảo hiểm");
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
        InitChildOfChildWindowModal('/hrm/Employee/Insurance?id=' + id,  600, 400, "Cập nhật thông tin bảo hiểm");
    });
    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "thongtinbaohiemxahoi.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: '/hrm/Employee/Insurances',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "InsuranceId",
                    fields: {
                        CreateDate: { type: 'date' },
                        IsActive: { type: 'boolean' },
                        SubscriptionDate: { type: 'date' }
                    }
                }
            },
            group: { field: 'DepartmentId' },
            pageSize: 100000,
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
                template: "#= ++recordInsurance #",
                width: 60
            },
            {
                field: "EmployeeCode",
                title: "Mã nhân viên",
                width: 150
            },
            {
                field: "FullName",
                title: "Tên nhân viên",
                width: 200
            },
            {
                field: "DepartmentId",
                title: "Phòng",
                width: 200,
                values: departments,
                hidden:true
            },
            {
                field: "InsuranceNumber",
                title: "Mã số BH",
                width: 150
            },
            {
                field: "SubscriptionDate",
                title: "Ngày tham gia",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "MonthBefore",
                title: "Tháng trước đó",
                width: 150,
                format: "{0:###,###}"
            },
            {
                field: "CityId",
                title: "Tỉnh",
                width: 150,
                values: cities
            },
             {
                 field: "Description",
                 title: "Ghi chú",
                 width: 300
             },
            {
                field: "IsActive",
                title: "Theo dõi",
                width: 120,
                template: '<div class="text-center"><i class="#= IsActive ? "glyphicon glyphicon-ok" : "glyphicon glyphicon-ban-circle" #"></i></div>'
            },
            {
                field: "CreateBy",
                title: "Người tạo",
                width: 150,
                values: users
            }
        ],
        dataBinding: function () {
            recordInsurance = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
}); 