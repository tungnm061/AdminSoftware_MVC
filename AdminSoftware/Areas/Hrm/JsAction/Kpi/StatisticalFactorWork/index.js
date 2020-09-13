var record = 0;
$(document).ready(function () {
    $("#MonthDate").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        optionLabel: "Chọn tháng",
        dataSource: months
    });
    $("#YearDate").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        optionLabel: "Chọn năm",
        dataSource: years
    });
    $("#btnSearchDate").click(function () {
       
        GridCallback('#grdMain');
    });
    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "NangSuatLaoDong.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/StatisticalFactorWork/StatisticalFactorWorks',
                        dataType: "json",
                        data: JSON.stringify({
                        monthDate : $("#MonthDate").data("kendoDropDownList").value(),
                        yearDate :  $("#YearDate").data("kendoDropDownList").value()
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
                        ToltalUsefulHourReal: { type: "number" },
                        UsefullHourMin :{type:"number"}
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
                 title: "Mã Nhân viên",
                 width: 180,
                 locked: true
                 
             },
             {
                 field: "FullName",
                 title: "Họ và tên",
                 width: 150,
                 locked: true
             },
              { 
                  field: "DepartmentId",
                  title: "Phòng ban",
                  width: 200,
                  values: department,
                  locked: true
              },
              {
                  title: "Thời gian làm việc hữu ích",
                  columns: [
                       {
                           field : "UsefullHourMin",
                           title: "Quy định",
                           width: 90
                       },
                      {
                      field: "ToltalUsefulHourReal",
                      title: "Thực tế",
                      width: 90
                      },
                          {
                              field: "NumberEmployee",
                              title: "SL NV",
                              width: 90
                          }
                  ]
              },
              //{
              //    title: "Thành tích hoàn thành đề xuất CV",
              //    columns: [
              //        {
              //        title: "A(2Đ)",
              //        width: 60,
              //        template: '<div class="text-center"><i class="#:SuggesPointA =="x" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
              //        }, {
              //        title: "B(1.5Đ)",
              //        width: 60,
              //        template: '<div class="text-center"><i class="#:SuggesPointB =="x" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
              //        },
              //     {
              //         title: "C(1Đ)",
              //         width: 60,
              //         template: '<div class="text-center"><i class="#:SuggesPointC =="x" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
              //     }, {
              //         title: "D(0.5Đ)",
              //         width: 60,
              //         template: '<div class="text-center"><i class="#:SuggesPointD =="x" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
              //     },
              //     {
              //         title: "E(0Đ)",
              //         width: 60,
              //         template: '<div class="text-center"><i class="#:SuggesPointE =="x" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
              //     }
              //    ]
              //},
              //{
              //    title: "Thành tích hoàn thành CV",
              //    columns: [
              //        {
              //            title: "A(2Đ)",
              //            width: 60,
              //            template: '<div class="text-center"><i class="#:ApprovedPointA =="x" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
              //        }, {
              //            title: "B(1Đ)",
              //            width: 60,
              //            template: '<div class="text-center"><i class="#:ApprovedPointB =="x" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
              //        },
              //     {
              //         title: "C(0Đ)",
              //         width: 60,
              //         template: '<div class="text-center"><i class="#:ApprovedPointC =="x"? "glyphicon glyphicon-ok" : "" #"></i></div>'
              //     }
              //    ]
              //},
              //{
              //    title: "Thành tích mức độ hài lòng ",
              //    columns: [
              //        {
              //            title: "A(1Đ)",
              //            width: 60,
              //            template: '<div class="text-center"><i class="#:ComplainPointA == "x" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
              //        }, {
              //            title: "B(0.5Đ)",
              //            width: 60,
              //            template: '<div class="text-center"><i class="#:ComplainPointB == "x" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
              //        },
              //     {
              //         title: "C(-0.5Đ)",
              //         width: 70,
              //         template: '<div class="text-center"><i class="#:ComplainPointC == "x" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
              //     }, {
              //         title: "D(-1Đ)",
              //         width: 60,
              //         template: '<div class="text-center"><i class="#:ComplainPointD == "x" ? "glyphicon glyphicon-ok" : "" #"></i></div>'
              //     }
              //    ]
              //},
             {
                 field: "AvgPoint",
                 title: "Điểm bình quân",
                 width: 140,
                 format: "{0:n2}"

             },
               {
                   field: "FactorPoint",
                   title: "Hệ số NSLD",
                   width: 140,
                   format: "{0:n2}"

               },             
               {
                   field: "FactorTypeReal",
                   title: "Xếp loại",
                   width: 140

               }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
    $('#btnPrint').click(function () {
        $('#processing').show();
        $.ajax(
        {
            type: 'POST',
            url: '/hrm/StatisticalFactorWork/Print',
            dataType: "json",
            data: JSON.stringify({
                monthDate: $("#MonthDate").data("kendoDropDownList").value(),
                yearDate: $("#YearDate").data("kendoDropDownList").value()
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
                    InitWindowModal('/hrm/StatisticalFactorWork/ViewReport', true, 0, 0, "Bảng đánh giá thành tích lao động", true);
                }
            }
        });
    });
});