var record = 0;
var columnDynamic = [];
columnDynamic.push('Họ tên');
columnDynamic.push('Điện thoại');
$(document).ready(function () {
    $('#FilterFrom,#FilterTo').kendoDatePicker({
        format: '{0:dd/MM/yyyy}',
        max:new Date()
    });
    $('#btnDelete').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
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
                        url: '/hrm/RecruitPlan/Delete',
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
        InitWindowModal('/hrm/RecruitPlan/RecruitPlan', true, 600, 400, "Thêm mới kế hoạch tuyển dụng", false);
    });
    $('#btnEdit').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitWindowModal('/hrm/RecruitPlan/RecruitPlan?id=' + id, true, 600, 400, "Cập nhật kế hoạch tuyển dụng", false);
    });
    $('#btnSearch').click(function () {
        var newDataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/RecruitPlan/RecruitPlans',
                        dataType: "json",
                        data: JSON.stringify({
                            fromDate: $('#FilterFrom').data('kendoDatePicker').value(),
                            toDate: $('#FilterTo').data('kendoDatePicker').value()
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
                    id: "RecruitPlanId",
                    fields: {
                        CreateDate: { type: 'date' },
                        IsActive: { type: 'boolean' },
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' }
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
    var columns = [
        {
            title: "STT",
            template: "#= ++record #",
            width: 60
        },
        {
            field: "RecruitPlanCode",
            title: "Mã kế hoạch",
            width: 150
        },
        {
            field: "Title",
            title: "Kế hoạch",
            width: 200
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
            field: "FromDate",
            title: "Ngày bắt đầu",
            width: 150,
            format: '{0:dd/MM/yyyy}'
        },
        {
            field: "ToDate",
            title: "Ngày kết thúc",
            width: 150,
            format: '{0:dd/MM/yyyy}'
        },
        {
            field: "Quantity",
            title: "Số lượng",
            width: 130,
            format: '{0:###,###}'
        },
        {
            field: "ApplicantNumber",
            title: "SL UV",
            width: 100,
            format: '{0:###,###}',
            filterable: false
        },
        {
            field: "ApplicantPass",
            title: "UV đạt",
            width: 100,
            format: '{0:###,###}',
            filterable: false
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
            title: "Kích hoạt",
            width: 130,
            template: '<div class="text-center"><i class="#= IsActive ? "glyphicon glyphicon-ok" : "glyphicon glyphicon-ban-circle" #"></i></div>'
        }
    ];
    for (var i = 0; i < columnDynamic.length; i++) {
        columns.push({
            field: "DynamicObject["+i+"].Value",
            title: columnDynamic[i],
            width: 130
        });
    }

    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/RecruitPlan/RecruitPlans',
                        dataType: "json",
                        data: JSON.stringify({
                            fromDate: $('#FilterFrom').data('kendoDatePicker').value(),
                            toDate: $('#FilterTo').data('kendoDatePicker').value()
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
                    id: "RecruitPlanId",
                    fields: {
                        CreateDate: { type: 'date' },
                        IsActive: { type: 'boolean' },
                        FromDate: { type: 'date' },
                        ToDate:{type:'date'}
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
        //columns: [
        //    {
        //        title: "STT",
        //        template: "#= ++record #",
        //        width: 60
        //    },
        //    {
        //        field: "RecruitPlanCode",
        //        title: "Mã kế hoạch",
        //        width: 150
        //    },
        //    {
        //        field: "Title",
        //        title: "Kế hoạch",
        //        width:200
        //    },
        //    {
        //        field: "DepartmentId",
        //        title: "Phòng",
        //        width: 200,
        //        values: departments
        //    },
        //    {
        //        field: "PositionId",
        //        title: "Vị trí",
        //        width: 200,
        //        values: positions
        //    },
        //    {
        //        field: "FromDate",
        //        title: "Ngày bắt đầu",
        //        width: 150,
        //        format:'{0:dd/MM/yyyy}'
        //    },
        //    {
        //        field: "ToDate",
        //        title: "Ngày kết thúc",
        //        width: 150,
        //        format: '{0:dd/MM/yyyy}'
        //    },
        //    {
        //        field: "Quantity",
        //        title: "Số lượng",
        //        width: 130,
        //        format: '{0:###,###}'
        //    },
        //    {
        //        field: "ApplicantNumber",
        //        title: "SL UV",
        //        width: 100,
        //        format: '{0:###,###}',
        //        filterable:false
        //    },
        //    {
        //        field: "ApplicantPass",
        //        title: "UV đạt",
        //        width: 100,
        //        format: '{0:###,###}',
        //        filterable: false
        //    },
        //    {
        //        field: "CreateBy",
        //        title: "Người tạo",
        //        width: 150,
        //        values: users
        //    },
        //    {
        //        field: "CreateDate",
        //        title: "Ngày tạo",
        //        format: "{0:dd/MM/yyyy hh:mm tt}",
        //        width: 180
        //    },
        //    {
        //        field: "IsActive",
        //        title: "Kích hoạt",
        //        width: 130,
        //        template: '<div class="text-center"><i class="#= IsActive ? "glyphicon glyphicon-ok" : "glyphicon glyphicon-ban-circle" #"></i></div>'
        //    },
        //    {
        //        field: "DynamicObject[0].Value",
        //        title:"",
        //        width: 130
        //    }
        //],
        columns:columns,
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});