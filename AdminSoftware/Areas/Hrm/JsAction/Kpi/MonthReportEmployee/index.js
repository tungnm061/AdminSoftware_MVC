var record = 0;
$(document).ready(function () {
    $("#MounthSearch").kendoDatePicker({
        start: "year",
        depth: "year",
        format: "MM yyyy",
        dateInput: true

    });
    $('#btnPrint').click(function () {
        var monthlyDate = $("#MounthSearch").data("kendoDatePicker").value();
        if (monthlyDate == null) {
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
        $('#processing').show();
        $.ajax(
        {
            type: 'POST',
            url: '/hrm/MonthReportEmployee/Print',
            dataType: "json",
            data: JSON.stringify({
                monthlyDate: monthlyDate
            }),
            contentType: 'application/json;charset=utf-8',
            success: function (response) {
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
                    InitWindowModal('/hrm/MonthReportEmployee/ViewReport', true, 0, 0, "Bảng báo cáo kế hoạch tháng", true);
                }
            }
        });
    });
    $("#btnSearchDate").click(function () {
        var monthlyDate = $("#MounthSearch").data("kendoDatePicker").value();
        if (monthlyDate == null) {
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
        GridCallback("#grdMain");
    });

    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "BaoCaoThang.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/MonthReportEmployee/WeekReports',
                        dataType: "json",
                        data: JSON.stringify({
                            monthlyDate: $("#MounthSearch").data("kendoDatePicker").value()
                        }),
                        contentType: 'application/json;charset=utf-8',
                        success: function (response) {
                            options.success(response);
                        }
                    });
                }

            },
            group: {
                field: "Title",
                aggregates: [
                      { field: "TimeTaskWeek1", aggregate: "sum" },
                    { field: "TimeTaskWeek2", aggregate: "sum" },
                    { field: "TimeTaskWeek3", aggregate: "sum" },
                    { field: "TimeTaskWeek4", aggregate: "sum" },
                    { field: "PointTaskWeek1", aggregate: "sum" },
                    { field: "PointTaskWeek2", aggregate: "sum" },
                    { field: "PointTaskWeek3", aggregate: "sum" },
                    { field: "PointTaskWeek4", aggregate: "sum" }
                ]
            },
            aggregate: [
                { field: "TimeTaskWeek1", aggregate: "sum" },
                { field: "TimeTaskWeek2", aggregate: "sum" },
                { field: "TimeTaskWeek3", aggregate: "sum" },
                { field: "TimeTaskWeek4", aggregate: "sum" },
                { field: "PointTaskWeek1", aggregate: "sum" },
                { field: "PointTaskWeek2", aggregate: "sum" },
                { field: "PointTaskWeek3", aggregate: "sum" },
                { field: "PointTaskWeek4", aggregate: "sum" }
            ],
            schema: {
                model: {
                    fields: {
                        TimeTaskWeek1: { type: "number" },
                        TimeTaskWeek2: { type: "number" },
                        TimeTaskWeek3: { type: "number" },
                        TimeTaskWeek4: { type: "number" },
                        PointTaskWeek1: { type: "number" },
                        PointTaskWeek2: { type: "number" },
                        PointTaskWeek3: { type: "number" },
                        PointTaskWeek4: { type: "number" }
                    }
                }
            },
            pageSize: 10000,
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
                 field: "Title",
                 title: " ",
                 width: 100,
                 hidden: true
             },
             {
                 title: "Thành tích công việc",
                 columns: [
                     {
                         field: "TaskCode",
                         title: "Mã CV",
                         width: 120
                     },
                     {
                         field: "TaskName",
                         title: "Nội dung công việc",
                         width: 250,
                         groupFooterTemplate: "TỔNG CỘNG"
                     }
                 ]
             },
             {
                 title: "Tuần 1",
                 columns: [
                     {
                         field: "StartTaskWeek1",
                         title: "Bắt đầu",
                         width: 120
                     },
                     {
                         field: "EndTaskWeek1",
                         title: "Kết thúc",
                         width: 120
                     },
                     {
                         field: "TimeTaskWeek1",
                         title: "Giờ hữu ích",
                         width: 120,
                         format: '{0:###,###.##}',
                         aggregates: ["sum"],
                         groupFooterTemplate: '#= sum ? sum : "" #',
                         template: '#= TimeTaskWeek1 == 0 ? "" : TimeTaskWeek1 #'
                     },
                     {
                         field: "PointTaskWeek1",
                         title: "Điểm CV",
                         width: 120,
                         format: '{0:###,###.##}',
                         aggregates: ["sum"],
                         groupFooterTemplate: '#= sum ? sum : "" #',
                         template: '#= PointTaskWeek1 == 0 ? "" : PointTaskWeek1 #'
                     }
                 ]
             },
             {
                 title: "Tuần 2",
                 columns: [
                     {
                         field: "StartTaskWeek2",
                         title: "Bắt đầu",
                         width: 120
                     },
                     {
                         field: "EndTaskWeek2",
                         title: "Kết thúc",
                         width: 120
                     },
                     {
                         field: "TimeTaskWeek2",
                         title: "Giờ hữu ích",
                         width: 120,
                         format: '{0:###,###.##}',
                         aggregates: ["sum"],
                         groupFooterTemplate: '#= sum ? sum : "" #',
                         template: '#= TimeTaskWeek2 == 0 ? "" : TimeTaskWeek2 #'
                     },
                     {
                         field: "PointTaskWeek2",
                         title: "Điểm CV",
                         width: 120,
                         format: '{0:###,###.##}',
                         aggregates: ["sum"],
                         groupFooterTemplate: '#= sum ? sum : "" #',
                         template: '#= PointTaskWeek2 == 0 ? "" : PointTaskWeek2 #'
                     }
                 ]
             },
             {
                 title: "Tuần 3",
                 columns: [
                     {
                         field: "StartTaskWeek3",
                         title: "Bắt đầu",
                         width: 120
                     },
                     {
                         field: "EndTaskWeek3",
                         title: "Kết thúc",
                         width: 120
                     },
                     {
                         field: "TimeTaskWeek3",
                         title: "Giờ hữu ích",
                         width: 120,
                         format: '{0:###,###.##}',
                         aggregates: ["sum"],
                         groupFooterTemplate: '#= sum ? sum : "" #',
                         template: '#= TimeTaskWeek3 == 0 ? "" : TimeTaskWeek3 #'
                     },
                     {
                         field: "PointTaskWeek3",
                         title: "Điểm CV",
                         width: 120,
                         format: '{0:###,###.##}',
                         aggregates: ["sum"],
                         groupFooterTemplate: '#= sum ? sum : "" #',
                         template: '#= PointTaskWeek3 == 0 ? "" : PointTaskWeek3 #'
                     }
                 ]
             },
             {
                 title: "Tuần 4",
                 columns: [
                     {
                         field: "StartTaskWeek4",
                         title: "Bắt đầu",
                         width: 120
                     },
                     {
                         field: "EndTaskWeek4",
                         title: "Kết thúc",
                         width: 120
                     },
                     {
                         field: "TimeTaskWeek4",
                         title: "Giờ hữu ích",
                         width: 120,
                         format: '{0:###,###.##}',
                         aggregates: ["sum"],
                         groupFooterTemplate: '#= sum ? sum : "" #',
                         template: '#= TimeTaskWeek4 == 0 ? "" : TimeTaskWeek4 #'
                     },
                     {
                         field: "PointTaskWeek4",
                         title: "Điểm CV",
                         width: 120,
                         format:'{0:###,###.##}',
                         aggregates: ["sum"],
                         groupFooterTemplate: '#= sum ? sum : "" #',
                         template: '#= PointTaskWeek4 == 0 ? "" : PointTaskWeek4 #'
                     }
                 ]
             },
              {
                  field: "Total",
                  title: "Tổng cộng",
                  width: 120
              }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

});