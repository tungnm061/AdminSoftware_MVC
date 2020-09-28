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
                        url: '/hrm/InsuranceMedical/Delete',
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
        InitWindowModal('/hrm/InsuranceMedical/InsuranceMedical', false, 600, 430, "Thêm mới thông tin bảo hiểm y tế", false);
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
        InitWindowModal('/hrm/InsuranceMedical/InsuranceMedical?id=' + id, false, 600, 430, "Cập nhật thông tin bảo hiểm", false);
    });
    $('#Year').kendoDropDownList(
    {
        dataTextField: "text",
        dataValueField: "value",
        dataSource: years,
        change:function() {
            window.location.href = '/hrm/InsuranceMedical/Index?year=' + $('#Year').data('kendoDropDownList').value();
        }
    });
    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "thongtinbaohiemyte.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: '/hrm/InsuranceMedical/InsuranceMedicals?year=' + $('#Year').data('kendoDropDownList').value(),
                dataType: "json"
            },
            schema: {
                model: {
                    id: "InsuranceMedicalId",
                    fields: {
                        StartDate: { type: 'date' },
                        ExpiredDate: { type: 'date' },
                        Amount:{type:'number'}
                    }
                }
            },
            group: { field: 'DepartmentId' },
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
                field: "InsuranceMedicalNumber",
                title: "Mã BHYT",
                width: 150
            },
            {
                field: "StartDate",
                title: "Ngày bắt đầu",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "ExpiredDate",
                title: "Ngày hết hạn",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "Amount",
                title: "Số tiền",
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
                field: "MedicalId",
                title: "Nơi đăng ký KCB",
                width: 300,
                values: medicals
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
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        excelExport: function (e) {
            e.workbook.fileName = "thongtinbaohiemyte-" + $('#Year').data('kendoDropDownList').value() + ".xlsx";
        }
    });
}); 