$(document).ready(function () {
    $('#ContractTypeId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: contractTypes,
        filter: 'startswith'
    });
    $('#StartDate,#EndDate').kendoDatePicker({
        format: "dd/MM/yyyy",
        dateInput: true
    });
    $('#btnUploadContractFile').click(function () {
        $('#UploadContractFile').click();
    });
    $('#UploadContractFile').change(function () {
        var data = new FormData();
        var files = $("#UploadContractFile").get(0).files;
        if (files.length > 0) {
            data.append("File", files[0]);
        }
        $('#processing').show();
        $.ajax({
            url: '/hrm/Employee/UploadFileContract',
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                $('#processing').hide();
                if (response.Status === 1) {
                    $.msgBox({
                        title: "Hệ thống ERP",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                            $('#ContractFile').val(response.Url);
                        }
                    });
                } else {
                    $.msgBox({
                        title: "Hệ thống ERP",
                        type: "error",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                        }
                    });
                }
            },
            error: function (er) {
                $('#processing').hide();
                $.msgBox({
                    title: "Hệ thống ERP",
                    type: "error",
                    content: er,
                    buttons: [{ value: "Đồng ý" }]
                });
            }
        });
    });
    $('#btnContractOthorFile').click(function () {
        $('#UploadContractOthorFile').click();
    });
    $('#UploadContractOthorFile').change(function () {
        var data = new FormData();
        var files = $("#UploadContractOthorFile").get(0).files;
        if (files.length > 0) {
            data.append("File", files[0]);
        }
        $('#processing').show();
        $.ajax({
            url: '/hrm/Employee/UploadFileContract',
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                $('#processing').hide();
                if (response.Status === 1) {
                    $.msgBox({
                        title: "Hệ thống ERP",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                            $('#ContractOthorFile').val(response.Url);
                        }
                    });
                } else {
                    $.msgBox({
                        title: "Hệ thống ERP",
                        type: "error",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                        }
                    });
                }
            },
            error: function (er) {
                $('#processing').hide();
                $.msgBox({
                    title: "Hệ thống ERP",
                    type: "error",
                    content: er,
                    buttons: [{ value: "Đồng ý" }]
                });
            }
        });
    });

    $('#btnRemoveContractOthorFile').click(function () {
        var filePath = $('#ContractOthorFile').val();
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/Employee/DeleteFileContract',
            data: JSON.stringify({ filePath: filePath }),
            contentType: 'application/json;charset=utf-8',
            success: function (response) {
                $('#processing').hide();
                if (response.Status === 1) {
                    $.msgBox({
                        title: "Hệ thống ERP",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                            $('#ContractOthorFile').val('');
                        }
                    });
                } else {
                    $.msgBox({
                        title: "Hệ thống ERP",
                        type: "error",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                        }
                    });
                }
            }
        });
    });
    $('#btnRemoveContractFile').click(function () {
        var filePath = $('#ContractFile').val();
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/Employee/DeleteFileContract',
            data: JSON.stringify({ filePath: filePath }),
            contentType: 'application/json;charset=utf-8',
            success: function (response) {
                $('#processing').hide();
                if (response.Status === 1) {
                    $.msgBox({
                        title: "Hệ thống ERP",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                            $('#ContractFile').val('');
                        }
                    });
                } else {
                    $.msgBox({
                        title: "Hệ thống ERP",
                        type: "error",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                        }
                    });
                }
            }
        });
    });
    $('#btnSave').click(function () {
        var model = {
            ContractId: contractId,
            ContractCode: $('#ContractCode').val(),
            EmployeeId: $('#EmployeeIdPost').val(),
            StartDate: $('#StartDate').data('kendoDatePicker').value(),
            EndDate: $('#EndDate').data('kendoDatePicker').value(),
            ContractTypeId: $('#ContractTypeId').data('kendoDropDownList').value(),
            ContractFile: $('#ContractFile').val(),
            ContractOthorFile: $('#ContractOthorFile').val(),
            Description: $('#Description').val()
        };
        if (model.ContractCode == null || model.ContractCode.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải nhập số hợp đồng!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.ContractTypeId == null || model.ContractTypeId.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải chọn loại hợp đồng!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.StartDate == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải chọn ngày bắt đầu!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.EndDate == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải chọn ngày kết thúc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.StartDate  > model.EndDate) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Ngày bắt đầu không được lớn hơn ngày kết thúc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/Employee/SaveContract',
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
                    title: "Hệ thống ERP",
                    type: type,
                    content: response.Message,
                    buttons: [{ value: "Đồng ý" }],
                    success: function (result) {
                        if (result === 'Đồng ý' && response.Status === 1) {
                            CloseChildWindowModal();
                            GridCallback('#grdMainContract');
                        }
                    }
                });
            },
            error: function (response) {
                $('#processing').hide();
                $.msgBox({
                    title: "Hệ thống ERP",
                    type: "error",
                    content: response.Msg,
                    buttons: [{ value: "Đồng ý" }]
                });
            }
        });
    });
});