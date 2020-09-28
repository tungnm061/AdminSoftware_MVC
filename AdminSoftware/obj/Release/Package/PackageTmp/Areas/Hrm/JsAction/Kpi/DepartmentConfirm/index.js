var record = 0;
$(document).ready(function () {
    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $(document).on("change", "input[type='radio'][name=status]", function () {
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();

        if (fromDate == null) {
            $.msgBox({
                title: "Hệ thống",
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
                title: "Hệ thống",
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
                title: "Hệ thống",
                type: "error",
                content: "Ngày bắt đầu không được lớn hơn ngày kết thúc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $("#Action").val(this.value);
        GridCallback("#grdMain");
    });
    $("#btnSearchDate").click(function () {
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();

        if (fromDate == null) {
            $.msgBox({
                title: "Hệ thống",
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
                title: "Hệ thống",
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
                title: "Hệ thống",
                type: "error",
                content: "Ngày bắt đầu không được lớn hơn ngày kết thúc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        GridCallback("#grdMain");
    });
    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/DepartmentConfirm/WorkDetails',
                        dataType: "json",
                        data: JSON.stringify({
                            action: $("#Action").val(),
                            fromDate: $("#FromDateSearch").data("kendoDatePicker").value(),
                            toDate: $("#ToDateSearch").data("kendoDatePicker").value()
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
                    id: "WorkDetailId",
                    fields: {
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' },
                        Status: { type: 'number' },
                        WorkType: { type: 'number' }
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
                 field: "Status",
                 title: "Trạng Thái",
                 width: 150,
                 values: window.statusWorkPlanDetail
             },
            {
                field: "CreateByName",
                title: "Người thực hiện",
                width: 150
            },
           
             {
                 field: "WorkType",
                 title: "Loại Công Việc",
                 width: 140,
                 values: window.statusWorkDetail
             },
            {
                field: "TaskCode",
                title: "Mã Công việc",
                width: 120
            },
           {
               field: "TaskName",
               title: "Tên Công việc",
               width: 260
           },
            {
                field: "DepartmentId",
                title: "Phòng chức năng",
                width: 200,
                values: departments,
                hidden : true
            },
             {
                 field: "UsefulHours",
                 title: "Giờ hữu ích",
                 width: 120
             },
              {
                  field: "Quantity",
                  title: "Số lượng",
                  width: 100
              },
            {
                field: "FromDate",
                title: "Ngày bắt đầu",
                format: "{0:dd/MM/yyyy}",
                width: 120

            },
            {
                field: "FisnishDateForMat",
                title: "Ngày hoàn thành thực tế",
                width: 200
             },
            {
                field: "ToDate",
                title: "Ngày hoàn thành",
                format: "{0:dd/MM/yyyy}",
                width: 140
            },
              {
                  field: "FileConfirm",
                  title: "File xác nhận",
                  width: 160,
                  template: "#if(FileConfirm != null){#" + "<a href='#=FileConfirm#' style='text-align:center'><i class='glyphicon glyphicon-download'></i></a>" + "#}#",
                  filterable: false,
                  sortable: false
              },
            {
                field: "Description",
                title: "Mô tả công việc",
                width: 300
            }

            
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
    $('#btnEdit').click(function () {
        var grid = $("#grdMain").data("kendoGrid");
        var selectedItem = grid.dataItem(grid.select());
        var id = selectedItem.WorkDetailId;
        var workType = selectedItem.WorkType;

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
        console.log($("#Action").val());
        if ($("#Action").val() === '3') {
            $("#btnSaveCancel").prop("disabled", true);
            $("#btnSaveAdd").prop("disabled", false);
            InitWindowModal('/hrm/DepartmentConfirm/ConfirmFinish?id=' + id + '&workType=' + workType, false, 800, 545, 'Xác nhận công việc đã hoàn thành', false);
        }
        if ($("#Action").val() === '4') {
            $("#btnSaveCancel").prop("disabled", false);
            $("#btnSaveAdd").prop("disabled", true);
            InitWindowModal('/hrm/DepartmentConfirm/ConfirmFinish?id=' + id + '&workType=' + workType, false, 800, 545, 'Cập nhật công việc đã hoàn thành', false);
        }
        if ($("#Action").val() === '5') {
            InitWindowModal('/hrm/DepartmentConfirm/CancelFinish?id=' + id + '&workType=' + workType, false, 800, 545, 'Chi tiết công việc chưa hoàn thành', false);
        }
    });
});