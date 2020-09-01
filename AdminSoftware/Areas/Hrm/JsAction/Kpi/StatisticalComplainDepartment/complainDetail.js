var record = 0;
$(document).ready(function () {
    $("#grdComplainDetails").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "DanhSachPhanAnh.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/StatisticalComplainDepartment/ComplainDetails',
                        dataType: "json",
                        data: JSON.stringify({
                            fromDate: $("#FromDateSearch").data("kendoDatePicker").value(),
                            toDate: $("#ToDateSearch").data("kendoDatePicker").value(),
                            id: $("#UserId").val()
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
                 field: "AccusedByCode",
                 title: "MNV bị phản ánh",
                 width: 200
             },
             {
                 field: "AccusedByName",
                 title: "Họ và tên",
                 width: 160
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
                 width: 140

             },
              {
                  field: "ConfirmedDate",
                  title: "Ngày xác nhận",
                  format: "{0:dd/MM/yyyy}",
                  width: 140
              },
               {
                   field: "Status",
                   title: "Trạng thái",
                   width: 140,
                   values: statusComplain
               }


        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

});