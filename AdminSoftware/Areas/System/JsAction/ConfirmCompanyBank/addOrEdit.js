
function fnSaveDetail(status) {
    var textStatus = "xác nhận";
    if (status === 2) {
        textStatus = "trả về";
    }

    $.msgBox({
        title: "Hệ thống",
        type: "confirm",
        content: "Bạn có chắc chắn muốn " + textStatus + " giao dịch này ?",
        buttons: [{ value: "Đồng ý" }, { value: "Hủy bỏ" }],
        success: function (result) {
            if (result === "Đồng ý") {
                $('#processing').show();
                $.ajax({
                    type: "POST",
                    url: '/system/ConfirmCompanyBank/SaveDetail',
                    data: JSON.stringify({ companyBankId: id, status: status }),
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
                                    GridCallback('#grdMain');
                                    CloseWindowModal();
                                    resetSelect();
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
            }
        }
    });
}

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


    $('#btnConfirmDetail').click(function () {
        fnSaveDetail(3);
    });

    $('#btnCancelDetail').click(function () {
        fnSaveDetail(2);
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
                            title: "Hệ thống",
                            type: "error",
                            content: response.Message,
                            buttons: [{ value: "Đồng ý" }],
                            success: function () {
                            }
                        });
                    } else {
                        $.msgBox({
                            title: "Hệ thống",
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
                title: "Hệ thống",
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