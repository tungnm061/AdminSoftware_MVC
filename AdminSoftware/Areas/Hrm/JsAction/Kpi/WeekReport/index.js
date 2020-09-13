var record = 0;
$(document).ready(function () {
    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $('#btnPrint').click(function () {
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
        $('#processing').show();
        $.ajax(
        {
            type: 'POST',
            url: '/hrm/WeekReport/Print',
            dataType: "json",
            data: JSON.stringify({
                fromDate: fromDate,
                toDate: toDate
            }),
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
                    InitWindowModal('/hrm/WeekReport/ViewReport', true, 0, 0, "Bảng báo cáo kế hoạch tuần", true);
                }
            }
        });
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
        toolbar: ["excel"],
        excel: {
            fileName: "BaoCaoTuan.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/WeekReport/WeekReports',
                        dataType: "json",
                        data: JSON.stringify({
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
                        UsefulHours: { type: "number" },
                        WorkPoint: { type: "number" },
                        FromDate :  { type: "date" },
                        ToDate: { type: "date" }
                    }
                }
            },

            pageSize: 100,
            serverPaging: false,
            serverFiltering: false,
            group: {
                field: "Title",
                aggregates: [
                      { field: "UsefulHours", aggregate: "sum" },
                     { field: "WorkPoint", aggregate: "sum" }
                ]
            },
            aggregate: [
               { field: "UsefulHours", aggregate: "sum" },
                { field: "WorkPoint", aggregate: "sum" }
            ]
            
        },
        height: gridHeight,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
              {
                  title: "Thành tích/Công việc",
                  columns: [
                       {
                           field: "TaskCode",
                           title: "Mã công việc",
                           width: 120
                       },
                      {
                          field: "TaskName",
                          title: "Nội dung công việc",
                          width: 200,
                          groupFooterTemplate: "TỔNG CỘNG"
                      },
                       {
                          field: "Description",
                          title: "Công việc cụ thể",
                          width: 200
                      }
                  ]
              },
               {
                   field: "FromDate",
                   title: "Thời gian bắt đầu",
                   width: 180,
                   format: "{0:dd/MM/yyyy}"

               },
                {
                    field: "ToDate",
                    title: "Thời gian kết thúc",
                    width: 180,
                    format: "{0:dd/MM/yyyy}"
                },
                  {
                      field: "UsefulHours",
                      title: "Thời gian hữu ",
                      width: 100,
                      aggregates: ["sum"],
                      groupFooterTemplate: '#= sum ? sum : "0" #'
                      
                  },
                //{
                //    field: "WorkPoint",
                //    title: "Điểm công việc",
                //    width: 100,
                //    aggregates: ["sum"],
                //    groupFooterTemplate: '#= sum ? sum : "0" #'
                //},
                 {
                     field: "Title",
                     title: " ",
                     width: 100,
                     hidden: true
                 }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

});