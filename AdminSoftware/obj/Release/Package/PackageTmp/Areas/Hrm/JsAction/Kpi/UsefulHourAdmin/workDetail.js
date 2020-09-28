var record = 0;
$(document).ready(function () {
    console.log($("#UserId").val());
    $("#grdWorkDetails").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "CongViec.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/UsefulHourAdmin/Employees',
                        dataType: "json",
                        data: JSON.stringify({
                            fromDate: $("#FromDateSearch").data("kendoDatePicker").value(),
                            toDate: $("#ToDateSearch").data("kendoDatePicker").value(),
                            id : $("#UserId").val()
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
                        WorkType: { type: 'number' }
                    }
                }
            },
            pageSize: 1000,
            serverPaging: false,
            serverFiltering: false
        },
        height: gridHeight,
        filterable: true,
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
                  field: "WorkType",
                  title: "Loại Công Việc",
                  width: 150,
                  values: statusWorkDetail
              },
               {
                   field: "TaskCode",
                   title: "Mã Công Việc",
                   width: 150
               },
            {
                field: "TaskName",
                title: "Tên công việc",
                width: 250
            },
               {
                   field: "Status",
                   title: "Trạng thái",
                   width: 160,
                   values: statusWorkPlanDetail
               },
            {
                field: "UsefulHours",
                title: "Giờ hữu ích thực tế",
                width: 160
            },
                {
                    field: "UsefulHoursTask",
                    title: "Giờ hữu ích quy định",
                    width: 180

                }


        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

});