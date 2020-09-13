var record = 0;
$(document).ready(function () {
    $("#MounthSearch").kendoDatePicker({
        start: "year",
        depth: "year",
        format: "MM yyyy",
        dateInput: true

    });

    $("#btnSearchDate").click(function () {
        var monthlyDate = $("#MounthSearch").data("kendoDatePicker").value();
        if (monthlyDate == null) {
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
                        url: '/hrm/MonthReportDepartment/WeekReports',
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
            aggregate: [
                { field: "UsefulHoursTask", aggregate: "sum" },
                { field: "UsefulHourTask1", aggregate: "sum" },
                { field: "UsefulHour1", aggregate: "sum" },
                { field: "UsefulHourTask2", aggregate: "sum" },
                { field: "UsefulHour2", aggregate: "sum" },
                { field: "UsefulHourTask3", aggregate: "sum" },
                { field: "UsefulHour3", aggregate: "sum" },
                { field: "UsefulHourTask4", aggregate: "sum" },
                { field: "UsefulHour4", aggregate: "sum" }
            ]
            ,
            schema: {
                model: {
                    id: "WorkDetailId",
                    fields: {
                        UsefulHourTask1: { type: "number" },
                        UsefulHour1: { type: "number" },
                        UsefulHourTask2: { type: "number" },
                        UsefulHour2: { type: "number" },
                        UsefulHourTask3: { type: "number" },
                        UsefulHour3: { type: "number" },
                        UsefulHourTask4: { type: "number" },
                        UsefulHour4: { type: "number" },
                        UsefulHoursTask: { type: "number" }
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
                 title: "STT",
                 width: 50,
                 template: "#= ++record#",
                 locked: true
             },
              {
                  field: "EmployeeCode",
                  title: "Mã nhân viên",
                  width: 140,
                  locked: true,
                  footerTemplate: "TỔNG"
              },
             {
                 field: "CreateByName",
                 title: "Họ và tên",
                 width: 140,
                 locked: true,
                 footerTemplate: "THỜI GIAN"
             },
              {
                  field: "TaskCode",
                  title: "Mã công việc",
                  width: 100,
                  locked: true,
                  footerTemplate: "HỮU ÍCH"
              },
              {
                  title: "Tên công việc",
                  columns: [
                       {
                           field: "TaskName",
                           title: "Tên CV",
                           width: 200,
                           footerTemplate: "THỰC HIỆN TRONG THÁNG"
                       },
                      {
                          field: "DescriptionTask",
                          title: "Yêu cầu",
                          width: 200
                      }
                  ]
              },
               {
                   title: "Thời gian hữu ích",
                   columns: [
                        {
                            field: "UsefulHoursTask",
                            title: "Giờ",
                            width: 80,
                            footerTemplate: "#: sum # "
                        },
                       {
                           field: "CalcType",
                           title: "Quy Cách",
                           width: 100,
                           values: calcType
                       }
                   ]

               },
              {
                  title: "Tuần 1 - " + dateMonth,
                  columns: [
                      {
                          title: "Kế hoạch",
                          columns: [
                        {
                            field: "FromDate1",
                            title: "Ngày bắt đầu",
                            width: 120
                        },
                       {
                           field: "ToDate1",
                           title: "Ngày kết thúc",
                           width: 120
                       },
                       {
                           field: "UsefulHourTask1",
                           title: "Giờ hữu ích",
                           width: 100,
                           footerTemplate: "#: sum # "
                       }
                          ]
                      },
                      {
                          title: "Thực tế",
                          columns: [
                        {
                            field: "FromDate1",
                            title: "Ngày bắt đầu",
                            width: 120

                        },
                       {
                           field: "ToDateReal1",
                           title: "Ngày kết thúc",
                           width: 120
                       },
                       {
                           field: "UsefulHour1",
                           title: "Giờ hữu ích",
                           width: 100,
                           footerTemplate: "#: sum # "

                       }
                          ]
                      },
                   {
                       title: "Chưa hoàn thành",
                       width: 140,
                       template: '<div class="text-center"><i class="#:CancelFinish1 =="V" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
                   },
                   {
                       title: "Hoàn thành",
                       width: 100,
                       template: '<div class="text-center"><i class="#:Finish1 =="V" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
                   }

                  ]
              },
                {
                    title: "Tuần 2 - " + dateMonth,
                    columns: [
                        {
                            title: "Kế hoạch",
                            columns: [
                          {
                              field: "FromDate2",
                              title: "Ngày bắt đầu",
                              width: 120
                          },
                         {
                             field: "ToDate2",
                             title: "Ngày kết thúc",
                             width: 120
                         },
                         {
                             field: "UsefulHourTask2",
                             title: "Giờ hữu ích",
                             width: 100,
                             footerTemplate: "#: sum # "
                         }
                            ]
                        },
                        {
                            title: "Thực tế",
                            columns: [
                          {
                              field: "FromDate2",
                              title: "Ngày bắt đầu",
                              width: 120

                          },
                         {
                             field: "ToDateReal2",
                             title: "Ngày kết thúc",
                             width: 120
                         },
                         {
                             field: "UsefulHour2",
                             title: "Giờ hữu ích",
                             width: 100,
                             footerTemplate: "#: sum # "

                         }
                            ]
                        },
                     {
                         title: "Chưa hoàn thành",
                         width: 140,
                         template: '<div class="text-center"><i class="#:CancelFinish2 =="V" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
                     },
                     {
                         title: "Hoàn thành",
                         width: 100,
                         template: '<div class="text-center"><i class="#:Finish2 =="V" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
                     }

                    ]
                },
               {
                   title: "Tuần 3 - " + dateMonth,
                   columns: [
                       {
                           title: "Kế hoạch",
                           columns: [
                         {
                             field: "FromDate3",
                             title: "Ngày bắt đầu",
                             width: 120
                         },
                        {
                            field: "ToDate3",
                            title: "Ngày kết thúc",
                            width: 120
                        },
                        {
                            field: "UsefulHourTask3",
                            title: "Giờ hữu ích",
                            width: 100,
                            footerTemplate: "#: sum # "
                        }
                           ]
                       },
                       {
                           title: "Thực tế",
                           columns: [
                         {
                             field: "FromDate3",
                             title: "Ngày bắt đầu",
                             width: 120

                         },
                        {
                            field: "ToDateReal3",
                            title: "Ngày kết thúc",
                            width: 120
                        },
                        {
                            field: "UsefulHour3",
                            title: "Giờ hữu ích",
                            width: 100,
                            footerTemplate: "#: sum # "

                        }
                           ]
                       },
                    {
                        title: "Chưa hoàn thành",
                        width: 140,
                        template: '<div class="text-center"><i class="#:CancelFinish3 =="V" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
                    },
                    {
                        title: "Hoàn thành",
                        width: 100,
                        template: '<div class="text-center"><i class="#:Finish3 =="V" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
                    }

                   ]
               },
              {
                  title: "Tuần 4 - " + dateMonth,
                  columns: [
                      {
                          title: "Kế hoạch",
                          columns: [
                        {
                            field: "FromDate4",
                            title: "Ngày bắt đầu",
                            width: 120
                        },
                       {
                           field: "ToDate4",
                           title: "Ngày kết thúc",
                           width: 120
                       },
                       {
                           field: "UsefulHourTask4",
                           title: "Giờ hữu ích",
                           width: 100,
                           footerTemplate: "#: sum # "
                       }
                          ]
                      },
                      {
                          title: "Thực tế",
                          columns: [
                        {
                            field: "FromDate4",
                            title: "Ngày bắt đầu",
                            width: 120

                        },
                       {
                           field: "ToDateReal4",
                           title: "Ngày kết thúc",
                           width: 120
                       },
                       {
                           field: "UsefulHour4",
                           title: "Giờ hữu ích",
                           width: 100,
                           footerTemplate: "#: sum # "

                       }
                          ]
                      },
                   {
                       title: "Chưa hoàn thành",
                       width: 140,
                       template: '<div class="text-center"><i class="#:CancelFinish4 =="V" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
                   },
                   {
                       title: "Hoàn thành",
                       width: 100,
                       template: '<div class="text-center"><i class="#:Finish4 =="V" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
                   }

                  ]
              }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

});