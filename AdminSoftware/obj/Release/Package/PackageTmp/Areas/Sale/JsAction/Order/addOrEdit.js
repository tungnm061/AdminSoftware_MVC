$(document).ready(function () {
    $("#FinishDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });

    $("#StartDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });

    $("#FinishDate").data("kendoDatePicker").enable(true);
    $("#StartDate").data("kendoDatePicker").enable(true);


    $("#ShipMoney").kendoNumericTextBox({
        format: "{0:n2}",
        min: 0,
        step: 1
    });

    $("#RateMoney").kendoNumericTextBox({
        format: "{0:n3}",
        min: 0,
        step: 1
    });
    
    //$('#CountryId').kendoDropDownList({
    //    dataTextField: "text",
    //    dataValueField: "value",
    //    dataSource: countries,
    //    optionLabel: "Chọn quốc gia",
    //    filter: 'contains'
    //});

    $('#GmailId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: gmails,
        optionLabel: "Chọn Gmail",
        filter: 'contains'
    });

    $('#Status').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: statusOrder,
        optionLabel: "Chọn trạng thái",
        filter: 'contains'
    });

    $('#ProducerId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: producers,
        optionLabel: "Chọn nhà sản xuất",
        filter: 'contains'
    });
    
    $("#grdOrderDetail").kendoGrid({
        dataSource: {
            transport: {
                read: '/sale/Order/OrderDetails',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "OrderDetailId",
                    fields: {
                        Price: { type: 'number' },
                        Quantity: { type: 'number' }
                    }
                }
            },
            aggregate: [
                { field: "Price", aggregate: "sum" },
                { field: "Quantity", aggregate: "sum" }
            ],
            pageSize: 10000,
            serverPaging: false,
            serverFiltering: false
        },
        height: 160,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            //{
            //    title: "Loại sản phẩm",
            //    width: 140,
            //    template: '#= AssignWorkId  != null ? "Được giao" : "Đề xuất" #'
            //},
            {
                field: "ProductCode",
                title: "Mã sản phẩm",
                width: 140

            },
            {
                field: "ProductName",
                title: "Tên sản phẩm",
                width: 300
            },
            {
                field: "Quantity",
                title: "Số lượng",
                width: 160,
                format: "{0:n0}",
                footerTemplate: "#:sum ? kendo.toString(sum, \"n2\") : 0 #"
            },
            {
                field: "UnitPrince",
                title: "Đơn giá",
                format: "{0:n2}",
                width: 90
            },
            {
                field: "Price",
                title: "Thành tiền",
                format: "{0:n2}",
                width: 120,
                footerTemplate: "#:sum ? kendo.toString(sum, \"n2\") : 0 #" + " $"


            }

        ]
    });
    $('#btnAddOrderDetail').click(function () {
        InitChildWindowModal('/sale/Order/OrderDetail', 550, 250, "Thêm sản phẩm");
    });
    $("#grdOrderDetail[event-dbclick='1']").on("dblclick", "tr.k-state-selected", function () {
        $('#btnEditOrderDetail').click();
    });
    $('#btnEditOrderDetail').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdOrderDetail');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn dữ liệu trước khi cập nhật!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        InitChildWindowModal('/sale/Order/OrderDetail?id=' + id, 550, 250, "Cập nhật sản phẩm");
    });
    $('#btnRemoveOrderDetail').click(function () {
        var id = GetGridRowSelectedKeyValue("#grdOrderDetail");
        if (id != null && id != typeof undefined) {
            $('#processing').show();
            $.ajax({
                type: 'POST',
                url: '/sale/Order/DeleteDetail',
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
                                GridCallback('#grdOrderDetail');
                            }
                        });
                    }
                }
            });
        } else {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn dữ liệu!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
    });

    $('#btnSave').click(function () {
        var model = {
            OrderId: orderId,
            OrderCode: $('#OrderCode').val(),
            TrackingCode: $('#TrackingCode').val(),
            //FirstName: $('#FirstName').val(),
            //LastName: $('#LastName').val(),
            //Email: $('#Email').val(),
            //Phone: $('#Phone').val(),
            //City: $('#City').val(),
            //Address1: $('#Address1').val(),
            //Address2: $('#Address2').val(),
            //Region: $('#Region').val(),
            //PostalZipCode: $('#PostalZipCode').val(),
            GmailId: $('#GmailId').data('kendoDropDownList').value(),
            TypeMoney: 1,
            ShipMoney: $('#ShipMoney').data('kendoNumericTextBox').value(),
            ProducerId: $('#ProducerId').data('kendoDropDownList').value(),
            //CountryId: $('#CountryId').data('kendoDropDownList').value(),
            Description: $('#Description').val(),
            FinishDate: $('#FinishDate').data('kendoDatePicker').value(),
            StartDate: $('#StartDate').data('kendoDatePicker').value(),
            Status: $('#Status').data('kendoDropDownList').value(),
            IsActive: true
            //RateMoney: $('#RateMoney').data('kendoNumericTextBox').value(),
        }

        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/sale/Order/Save',
            dataType: "json",
            data: JSON.stringify({
                model: model
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
                            GridCallback('#grdMain');
                            CloseWindowModal();
                        }
                    });
                }
            }
        });
    });


});