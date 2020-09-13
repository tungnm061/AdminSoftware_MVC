var record = 0;
$(document).ready(function () {
    $('#RecruitPlanIdSelect').kendoDropDownList({
        dataTextField: 'text',
        dataValueField: 'value',
        dataSource: recruitPlans,
        optionLabel: 'Chọn kế hoạch',
        filter: 'startswith'
    });
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
                        url: '/hrm/RecruitResult/Delete',
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
        InitWindowModal('/hrm/RecruitResult/RecruitResult', true, 600, 400, "Thêm mới kết quả tuyển dụng", false);
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
        InitWindowModal('/hrm/RecruitResult/RecruitResult?id=' + id, true, 600, 400, "Cập nhật kết quả tuyển dụng", false);
    });
    $('#btnSearch').click(function () {
        var newDataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/RecruitResult/RecruitResults',
                        dataType: "json",
                        data: JSON.stringify({
                            recruitPlanId: $('#RecruitPlanIdSelect').data('kendoDropDownList').value()
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
                    id: "RecruitResultId",
                    fields: {
                        CreateDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        });
        var grid = $("#grdMain").data("kendoGrid");
        grid.setDataSource(newDataSource);
    });
    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "ketquatuyendung.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/RecruitResult/RecruitResults',
                        dataType: "json",
                        data: JSON.stringify({
                            recruitPlanId: $('#RecruitPlanIdSelect').data('kendoDropDownList').value()
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
                    id: "RecruitResultId",
                    fields: {
                        CreateDate: { type: 'date' },
                        EmployeeId:{nullable:true}
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
                field: "DepartmentId",
                title: "Phòng",
                width: 200,
                values: departments
            },
            {
                field: "PositionId",
                title: "Vị trí",
                width: 200,
                values: positions
            },
            {
                field: "FullName",
                title: "Ứng viên",
                width: 170
            },
            {
                field: "Sex",
                title: "Giới tính",
                width: 130,
                values:sexs
            },
            {
                field: "EmployeeId",
                title: "Người đánh giá",
                width: 200,
                values: employees
            },
            {
                field: "Result",
                title: "Kết quả",
                width: 130,
                values: recruitResults
            },
            {
                field: "Description",
                title: "Ghi chú",
                width: 250
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
                format: "{0:dd/MM/yyyy hh:mm tt}",
                width: 180
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});