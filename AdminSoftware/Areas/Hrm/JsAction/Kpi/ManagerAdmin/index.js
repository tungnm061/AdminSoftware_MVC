
var record = 0;
$(document).ready(function () {
    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy",
        max: new Date()
    });
    $("#FromDateSearch").data("kendoDatePicker").enable(false);
    $("#ToDateSearch").data("kendoDatePicker").enable(false);
    $("#btnSearchDate").prop("disabled", true);
    $(document).on("change", "input[type='radio'][name=status]", function () {
        $("#Action").val(this.value);
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();
        var action = this.value;
        if (action === '3') {
            $("#FromDateSearch").data("kendoDatePicker").enable(false);
            $("#ToDateSearch").data("kendoDatePicker").enable(false);
            $("#btnSearchDate").prop("disabled", true);
        }
        if (action === '4') {
            $("#FromDateSearch").data("kendoDatePicker").enable(true);
            $("#ToDateSearch").data("kendoDatePicker").enable(true);
            $("#btnSearchDate").prop("disabled", false);
        }
        var newDataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/ManagerAdmin/WorkPlans',
                        dataType: "json",
                        data: JSON.stringify({
                            action: action,
                            fromDate: fromDate,
                            toDate: toDate
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
                    id: "WorkPlanId",
                    fields: {
                        CreateDate: { type: 'date' },
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' },
                        ConfirmedDate: { type: 'date' },
                        ApprovedDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false,
            group: { field: 'DepartmentId' }
        });
        grid = $("#grdMain").data("kendoGrid");
        grid.setDataSource(newDataSource);
    });
    $("#btnSearchDate").click(function () {
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();
        var action = $("#Action").val();
        if (fromDate == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải chọn ngày bắt đầu tìm kiếm!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (toDate == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải chọn ngày kết thúc tìm kiếm!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (toDate < fromDate) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Ngày bắt đầu không được lớn hơn ngày kết thúc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
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
                        url: '/hrm/ManagerAdmin/WorkPlans',
                        dataType: "json",
                        data: JSON.stringify({
                            action: action,
                            fromDate: fromDate,
                            toDate: toDate
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
                    id: "WorkPlanId",
                    fields: {
                        CreateDate: { type: 'date' },
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' },
                        ConfirmedDate: { type: 'date' },
                        ApprovedDate: { type: 'date' }
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
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/ManagerAdmin/WorkPlans',
                        dataType: "json",
                        data: JSON.stringify({
                            action: 3,
                            fromDate: null,
                            toDate: null
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
                    id: "WorkPlanId",
                    fields: {
                        CreateDate: { type: 'date' },
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' },
                        ConfirmedDate: { type: 'date' },
                        ApprovedDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false,
            group: { field: 'DepartmentId' }
        },
        height: gridHeight,
        filterable: false,
        sortable: true,
        pageable: false,
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
                  width: 130
              },
               {
                   field: "FullName",
                   title: "Tên nhân viên",
                   width: 130
               },
              {
                  field: "WorkPlanCode",
                  title: "Mã kế hoạch",
                  width: 140
              },
            {
                field: "FromDate",
                title: "Ngày bắt đầu",
                width: 140,
                format: "{0:dd/MM/yyyy}"

            },
            {
                field: "ToDate",
                title: "Ngày hoàn thành",
                width: 150,
                format: "{0:dd/MM/yyyy}"

            },
             {
                 field: "DepartmentId",
                 title: "Phòng",
                 width: 200,
                 hidden: true,
                 values: departments

             },
           {
               field: "CreateDateFormat",
               title: "Ngày tạo",
               width: 220
           },
             {
                 field: "Description",
                 title: "Mô tả",
                 width: 300
             },

            {
                field: "ConfirmedByName",
                title: "Trưởng BP duyệt",
                width: 150
            },
            {
                field: "ConfirmedDate",
                title: "Ngày duyệt",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "ApprovedByName",
                title: "TP HCNS duyệt ",
                width: 150
            },
            {
                field: "ApprovedDate",
                title: "Ngày duyệt",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            }


        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

    $('#btnEdit').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn dữ liệu trước khi cập nhật!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        InitWindowModal('/hrm/ManagerAdmin/WorkPlan?id=' + id, true, "", "", 'Cập nhật kế hoạch', false);
    });
});