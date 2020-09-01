
$(document).ready(function () {

    $("#select").kendoMultiSelect({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: users,
        value: performers,
        autoClose: false
    });
    $("#select").data("kendoMultiSelect").enable(false);
    $("#grdWorkStreamDetail").kendoGrid({
        dataSource: {
            data: workStreamDetails,
            schema: {
                model: {
                    id: "WorkStreamDetailId",
                    fields: {
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' }
                    }
                }
            },
            pageSize: 10000,
            serverPaging: false,
            serverFiltering: false
        },
        height: 230,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [

              {
                  field: "CreateByName",
                  title: "Người thực hiện",
                  width: 140
              },
            {
                field: "TaskCode",
                title: "Mã công việc",
                width: 140

            },
            {
                field: "TaskName",
                title: "Tên công việc",
                width: 220
            },
            {
                field: "FromDate",
                title: "Ngày bắt đầu",
                format: "{0:dd/MM/yyyy}"

            },
            {
                field: "ToDate",
                title: "Ngày hoàn thành",
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "Description",
                title: "Mô tả công việc",
                width: 320
            }

        ]
    });
    $('#btnSave').click(function () {
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/system/ApprovedWorkStream/Save',
            dataType: "json",
            data: JSON.stringify({
                id: workStreamId,
                action: 1
            }),
            contentType: 'application/json;charset=utf-8',
            success: function (response) {
                $('#processing').hide();
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
                    $.msgBox({
                        title: "Hệ thống ERP",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                            GridCallback('#grdMain');
                            CloseWindowModal();
                        }
                    });
                }
            }
        });
    });
    $('#btnCancel').click(function () {
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/system/ApprovedWorkStream/Save',
            dataType: "json",
            data: JSON.stringify({
                id: workStreamId,
                action: 2
            }),
            contentType: 'application/json;charset=utf-8',
            success: function (response) {
                $('#processing').hide();
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
                    $.msgBox({
                        title: "Hệ thống ERP",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                            GridCallback('#grdMain');
                            CloseWindowModal();
                        }
                    });
                }
            }
        });
    });
});