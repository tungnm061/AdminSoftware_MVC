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
                        url: '/hrm/StatisticalFactorDepartment/StatisticalFactorWorks',
                        dataType: "json",
                        data: JSON.stringify({
                            monthDate: $("#MonthDate").data("kendoDropDownList").value(),
                            yearDate: $("#YearDate").data("kendoDropDownList").value()
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
                    id: "UserId",
                    fields: {
                        TotalQuantity: { type: "number" },
                        TotalFinish: { type: "number" },
                        TotalUsefulHours: { type: "number" },
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
                  title: "Thời gian làm việc hữu ích",
                  columns: [
                       {
                           field : "UsefullHourMin",
                           title: "Quy định",
                           width: 90

                       },
                      {
                      field: "TotalUsefulHours",
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

               }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});