var record = 0;
$(document).ready(function () {
    $('#btnPrint').click(function () {
        var dateSelect = $("#DateSelect").data("kendoDatePicker").value();
        if (dateSelect == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn tháng!",
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
            url: '/hrm/SalaryTimeSheet/Print',
            dataType: "json",
            data: JSON.stringify({
                date: dateSelect
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
                    InitWindowModal('/hrm/SalaryTimeSheet/ViewReport', true, 0, 0, "Bảng tính lương cán bộ công nhân viên", true);
                }
            }
        });
    });
    $('#DateSelect').kendoDatePicker({
        start: "year",
        depth: "year",
        format: "MM/yyyy",
        dateInput: true
    });
    $("#btnSearchDate").click(function () {
        GridCallback('#grdMain');
    });
 
    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "bangluong.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/SalaryTimeSheet/SalaryTimeSheets',
                        dataType: "json",
                        data: JSON.stringify({
                            date: $('#DateSelect').data("kendoDatePicker").value()
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
                        ProfessionalSalary: { type: 'number' },
                        ResponsibilitySalary: { type: 'number' },
                        IncurredSalary: { type: 'number' },
                        TotalSalary: { type: 'number' },
                        BaseSalary: { type: 'number' },
                        FactorPoint: { type: 'number' },
                        FactorDay: { type: 'number' },
                        SalaryHoliday: { type: 'number' },
                        SalaryKpi: { type: 'number' },
                        SalaryOt: { type: 'number' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false,
            group: { field: "DepartmentId" }
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
                width: 60,
                locked: true
            },
            {
                field: "EmployeeCode",
                title: "Mã NV",
                width: 150,
                locked: true
            },
            {
                field: "FullName",
                title: "Họ tên",
                width: 250,
                filterable: false,
                locked: true
            },
            {
                field: "DepartmentId",
                title: "Phòng",
                width: 200,
                values: departments,
                hidden: true
            },
             {
                 field: "BaseSalary",
                 title: "Lương cơ sở",
                 width: 130,
                 format: "{0:n0}"
             },
            {
                field: "BasicSalary",
                title: "Lương cơ bản",
                width: 130,
                format :"{0:n0}"
            },
             {
                 field: "ProfessionalSalary",
                 title: "Lương chuyên môn",
                 width: 170,
                 format: "{0:n0}"
             },
             {
                 field: "ResponsibilitySalary",
                 title: "Lương trách nhiệm",
                 width: 170,
                 format: "{0:n0}"
             },
             {
                 field: "FactorDay",
                 title: "Hệ số công",
                 width: 120,
                 format: "{0:n2}"
             },
               {
                   field: "TotalDayHoliday",
                   title: "Công nghỉ phép",
                   width: 160,
                   format: "{0:n2}"
               },
                 {
                     field: "DayPointOt",
                     title: "Công làm thêm",
                     width: 160,
                     format: "{0:n2}"
                 },
              {
                  field: "FactorPoint",
                  title: "Hệ số Kpi",
                  width: 120,
                  format: "{0:n2}"
              },
              {
                  field: "IncurredSalary",
                  title: "Các khoản phát sinh",
                  width: 180,
                  format: "{0:n0}"
              },
               {
                   field: "SalaryKpi",
                   title: "Lương đi làm",
                   width: 160,
                   format: "{0:n0}"
               },
                {
                    field: "SalaryHoliday",
                    title: "Lương nghỉ phép",
                    width: 160,
                    format: "{0:n0}"
                },
                  {
                      field: "SalaryOt",
                      title: "Lương làm thêm",
                      width: 160,
                      format: "{0:n0}"
                  },
               {
                   field: "TotalSalary",
                   title: "Tổng lương",
                   width: 160,
                   format: "{0:n0}"
               }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        excelExport: function (e) {
            e.workbook.fileName = "bangluong-" + $("#FromDateSearch").val()+ "-" + $("#ToDateSearch").val() + ".xlsx";
        }
    });



});