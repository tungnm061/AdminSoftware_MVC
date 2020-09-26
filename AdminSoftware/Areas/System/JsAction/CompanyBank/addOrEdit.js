$(document).ready(function () {
    $('#ExpenseId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: expenseTypes,
        optionLabel: "Chọn loại giao dịch",
        filter: 'contains'
    });

    $('#TypeMonney').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: typeMoneys,
        filter: 'contains'
    });

    $('#TradingBy').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: users,
        filter: 'contains'
    });

    $("#TradingDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });

    $("#MoneyNumber").kendoNumericTextBox({
        format: "{0:n2}",
        step: 1
    });


    $('#btnSave').click(function () {
        var status = $('input[name=optradio]:checked').val();
        var model = {
            CompanyBankId: id,
            ExpenseId: $('#ExpenseId').data("kendoDropDownList").value(),
            //Description: $('#Description').val(),
            TypeMonney: $('#TypeMonney').data("kendoDropDownList").value(),
            MoneyNumber: $('#MoneyNumber').data("kendoNumericTextBox").value(),
            TradingDate: $('#TradingDate').data("kendoDatePicker").value(),
            TradingBy: $('#TradingBy').data("kendoDropDownList").value(),
            ExpenseText: $('#ExpenseText').val(),
            Status: status
        }

        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/system/CompanyBank/Save',
            data: JSON.stringify({ model: model }),
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                $('#processing').hide();
                var type;
                if (response.Status === 1)
                    type = 'info';
                else
                    type = 'error';
                $.msgBox({
                    title: "Hệ thống",
                    type: type,
                    content: response.Message,
                    buttons: [{ value: "Đồng ý" }],
                    success: function (result) {
                        if (result === 'Đồng ý' && response.Status === 1) {
                            CloseWindowModal();
                            GridCallback('#grdMain');
                        }
                    } //
                });
            },
            error: function (response) {
                $('#processing').hide();
                $.msgBox({
                    title: "Hệ thống",
                    type: "error",
                    content: response.Msg,
                    buttons: [{ value: "Đồng ý" }]
                });
            }
        });
    });


    $("#grdMainTextNote").kendoGrid({
        dataSource: {
            transport: {
                read: '/system/CompanyBank/TextNotes',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "TextNoteId",
                    fields: {
                        CreateDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 180,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "CreateDate",
                title: "Thời gian",
                format: "{0:dd/MM/yyyy hh:mm:ss tt}",
                width: 160
            },
            {
                field: "CreateBy",
                title: "Người tạo",
                width: 120,
                values: users
            },
            {
                field: "Text",
                title: "Nội dung",
                width: 400
            }

        ]
    });
    $('#btnCreateTextNote').click(function () {
        InitChildWindowModal('/system/CompanyBank/TextNote', 550, 330, "Thêm ghi chú");
    });
    //$("#grdMainTextNote[event-dbclick='1']").on("dblclick", "tr.k-state-selected", function () {
    //    $('#btnEditTextNote').click();
    //});

    //$('#btnEditTextNote').click(function () {
    //    var id = GetGridRowSelectedKeyValue('#grdMainTextNote');
    //    if (id == null) {
    //        $.msgBox({
    //            title: "Hệ thống",
    //            type: "error",
    //            content: "Bạn phải chọn dữ liệu trước khi cập nhật!",
    //            buttons: [{ value: "Đồng ý" }],
    //            success: function () {
    //            }
    //        });
    //        return;
    //    }
    //    InitChildWindowModal('/system/CompanyBank/TextNote?id=' + id, 550, 330, "Cập nhật ghi chú");
    //});

    $('#btnDeleteTextNote').click(function () {
        var id = GetGridRowSelectedKeyValue("#grdMainTextNote");
        if (id != null && id != typeof undefined) {
            $('#processing').show();
            $.ajax({
                type: 'POST',
                url: '/system/CompanyBank/DeleteTextNote',
                dataType: "json",
                data: JSON.stringify({
                    id: id
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
                                GridCallback('#grdMainTextNote');
                            }
                        });
                    }
                }
            });
        } else {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn nhân viên!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
    });
});