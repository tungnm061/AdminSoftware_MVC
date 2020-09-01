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
        var grid = $("#grdMain").data("kendoGrid");
        if (action === '1') {
            $("#FromDateSearch").data("kendoDatePicker").enable(false);
            $("#ToDateSearch").data("kendoDatePicker").enable(false);
            $("#btnSearchDate").prop("disabled", true);
            grid.hideColumn("ConfirmedByName");
            grid.hideColumn("ConfirmedDate");
        }
        if (action === '3') {
            $("#FromDateSearch").data("kendoDatePicker").enable(true);
            $("#ToDateSearch").data("kendoDatePicker").enable(true);
            $("#btnSearchDate").prop("disabled", false);
            grid.showColumn("ConfirmedByName");
            grid.showColumn("ConfirmedDate");
        }
        if (action === '2') {
            $("#FromDateSearch").data("kendoDatePicker").enable(true);
            $("#ToDateSearch").data("kendoDatePicker").enable(true);
            $("#btnSearchDate").prop("disabled", false);
            grid.showColumn("ConfirmedByName");
            grid.showColumn("ConfirmedDate");
        }
        var newDataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/ApprovedComplain/Complains',
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
                    id: "ComplainId",
                    fields: {
                        CreateDate :{ type: 'date' },
                        Status: { type: 'number' },
                        ConfirmedDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
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
                        url: '/hrm/ApprovedComplain/Complains',
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
                    id: "ComplainId",
                    fields: {
                        CreateDate: { type: 'date' },
                        Status: { type: 'number' },
                        ConfirmedDate: { type: 'date' }
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
                        url: '/hrm/ApprovedComplain/Complains',
                        dataType: "json",
                        data: JSON.stringify({
                            action: 1,
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
                    id: "ComplainId",
                    fields: {
                        CreateDate: { type: 'date' },
                        Status: { type: 'number' },
                        ConfirmedDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: gridHeight,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "CreateByCode",
                title: "MNV phản ánh",
                width: 120
            },
             {
                 field: "CreateByName",
                 title: "Họ và tên",
                 width: 120
             },
             {
                 field: "AccusedByCode",
                 title: "MNV bị phản ánh",
                 width: 120
             },
             {
                 field: "AccusedByName",
                 title: "Họ và tên",
                 width: 120
             },
              {
                  field: "Description",
                  title: "Hành vi",
                  width: 450
              },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                format: "{0:dd/MM/yyyy}",
                width: 140

            },
             {
                 field: "ConfirmedByName",
                 title: "Người xác nhận",
                 width: 140,
                 hidden: true

             },
              {
                  field: "ConfirmedDate",
                  title: "Ngày xác nhận",
                  format: "{0:dd/MM/yyyy}",
                  width: 140,
                  hidden: true
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
            InitWindowModal('/hrm/ApprovedComplain/Complain?id=' + id, false, 600, 320, 'Cập nhật chi tiết phản ánh', false);  
    });
});