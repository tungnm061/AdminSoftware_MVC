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
                        url: '/hrm/Contract/Delete',
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
        InitWindowModal('/hrm/Contract/Contract', false, 550, 430, "Thêm mới hợp đồng", false);
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
        InitWindowModal('/hrm/Contract/Contract?id=' + id, false, 550, 430, "Cập nhật hợp đồng", false);
    });
    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/Contract/Contracts',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "ContractId",
                    fields: {
                        CreateDate: { type: 'date' },
                        StartDate: { type: 'date' },
                        EndDate:{type:'date'}
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
                field: "ContractCode",
                title: "Mã hợp đồng",
                width: 150
            },
            {
                field: "ContractTypeId",
                title: "Loại hợp đồng",
                values: contractTypes,
                width: 170
            },
            {
                field: "EmployeeId",
                title: "Nhân viên",
                width: 200,
                values: employees
            },
            {
                field: "StartDate",
                title: "Từ ngày",
                format: "{0:dd/MM/yyyy}",
                width: 150
            },
            {
                field: "EndDate",
                title: "Từ ngày",
                format: "{0:dd/MM/yyyy}",
                width: 150
            },
            {
                field: "ContractFile",
                title: "Hợp đồng",
                width: 120,
                template: "#if(ContractFile != null){#" + "<a href='#=ContractFile#' style='text-align:center'><i class='glyphicon glyphicon-download'></i></a>" + "#}#",
                filterable: false,
                sortable:false
            },
            {
                field: "ContractOthorFile",
                title: "Phụ lục",
                width: 120,
                template: "#if(ContractOthorFile != null){#" + "<a href='#=ContractOthorFile#' style='text-align:center'><i class='glyphicon glyphicon-download'></i></a>" + "#}#",
                filterable: false,
                sortable: false
            },
            {
                field: "CreateBy",
                title: "Người tạo",
                width: 150,
                values: users
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                format: "{0:dd/MM/yyyy hh:mm tt}",
                width: 180
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});