var recordEmployee = 0;
var recordDepartment = 0;
$(document).ready(function () {
    $('#btnPrint').click(function () {
        $('#processing').show();
        $.ajax(
        {
            type: 'POST',
            url: '/hrm/Employee/Print',
            dataType: "json",
            contentType: 'application/json;charset=utf-8',
            success: function (response) {
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
                    InitWindowModal('/hrm/Employee/ViewReport', true, 0, 0, "Bảng danh sách nhân viên", true);
                }
            }
        });
    });
    $('#btnDeleteEmployee').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainEmployee');
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
                        url: '/hrm/Employee/Delete',
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
    $('#btnCreateEmployee').bind("click", function () {
        var id = 0;
        $("#EmployeeIdPost").val(id);
        InitWindowModal('/hrm/Employee/Employee', true, '', '', "Thêm mới nhân viên", false);
    });
    $("#grdMainEmployee[event-dbclick='1']").on("dblclick", "tr.k-state-selected", function () {
        $('#btnEditEmployee').click();
    });
    $('#btnEditEmployee').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainEmployee');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $("#EmployeeIdPost").val(id);
        InitWindowModal('/hrm/Employee/Employee?id=' + id, true, '', '', "Cập nhật nhân viên", false);
    });
    $("#grdMainEmployee").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "DanhSachNhanVien.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/Employee/Employees',
                        dataType: "json",
                        data: JSON.stringify({
                            path: $("#Path").val()
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
                    id: "EmployeeId",
                    fields: {
                        CreateDate: { type: 'date' },
                        IsActive: { type: 'boolean' },
                        DateOfBirth: { type: 'date' },
                        CreateBy: { type: 'number' },
                        IdentityCardDate: { type: 'date', nullable: true },
                        DateOfYouthUnionAdmission: { type: 'date', nullable: true },
                        DateOfPartyAdmission: { type: 'date', nullable: true }
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
                template: "#= ++recordEmployee #",
                width: 50,
                locked:true
            },
            {
                field: "EmployeeCode",
                title: "Mã nhân viên",
                width: 150,
                locked: true
            },
            {
                field: "FullName",
                title: "Tên nhân viên",
                width: 180,
                locked: true
            },
            {
                field: "DateOfBirth",
                title: "Ngày sinh",
                width: 130,
                locked: true,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "Gender",
                title: "Giới tính",
                width: 110,
                values: sexs
            },
            {
                field: "DepartmentId",
                title: "Phòng",
                width: 260,
                values: departments
            },
            {
                field: "CountryId",
                title: "Quốc tịch",
                width: 110,
                values: countries
            },
            //{
            //    field: "NationId",
            //    title: "Dân tộc",
            //    width: 100,
            //    values: nations
            //},
            //{
            //    field: "ReligionId",
            //    title: "Tôn giáo",
            //    width: 110,
            //    values: religions
            //},
            {
                field: "IdentityCardNumber",
                title: "Số CMND",
                width: 150
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
            },
            {
                field: "IsActive",
                title: "Theo dõi",
                width: 120,
                template: '<div class="text-center"><i class="#= IsActive ? "glyphicon glyphicon-ok" : "glyphicon glyphicon-ban-circle" #"></i></div>'
            }
        ],
        dataBinding: function () {
            recordEmployee = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
    $("#grdMainDepartment").kendoTreeList({
        dataSource: {
            data: departmentsObject,
            schema: {
                model: {
                    id: "DepartmentId",
                    parentId: "ParentId",
                    fields: {
                        IsActive: { field: "IsActive", type: "boolean" },
                        ParentId: { field: "ParentId", nullable: true },
                        DepartmentId: { field: "DepartmentId", nullable: false }
                    },
                    expanded: false
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: gridHeight,
        filterable: false,
        sortable: true,
        selectable: 'row',
        columns: [

            {
                field: "DepartmentName",
                title: "Phòng ban",
                width: 200
            }

        ],
        dataBinding: function () {
        }
    });
    $("#grdMainDepartment[event-dbclick='1']").on("dblclick", "tr.k-state-selected", function () {
        var treeList = $("#grdMainDepartment").data("kendoTreeList");
        var row = treeList.select();
        if (row.length > 0) {
            var data = treeList.dataItem(row);
            $("#Path").val(data.Path);
            GridCallback("#grdMainEmployee");
        }
    });
}); 