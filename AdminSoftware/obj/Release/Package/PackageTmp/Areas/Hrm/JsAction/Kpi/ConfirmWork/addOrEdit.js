
$(document).ready(function () {
    $("#UsefulHours,#UsefulHoursTask").kendoNumericTextBox({
        format: "{0:###,###.##}"
    });
    $("#UsefulHours").data("kendoNumericTextBox").enable(false);
    $(document).on("change", "input[type='radio'][name=optradio]", function () {
        if (this.value === '2' || this.value === '5') {
            $("#UsefulHours").data("kendoNumericTextBox").enable(false);
        }
        if (this.value === '3') {
            $("#UsefulHours").data("kendoNumericTextBox").enable(true);
        }
    });
    $("#grdMainWorkingNote").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/ConfirmWork/WorkingNotes',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "WorkingNoteId",
                    fields: {
                        CreateDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 200,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "CreateDate",
                title: "Thời gian",
                format: "{0:dd/MM/yyyy hh:mm:ss tt}",
                width: 140
            },
            {
                field: "TextNote",
                title: "Công việc thực hiện",
                width: 500
            }

        ]
    });
    $('#btnCreateWorkingNote').click(function () {
        InitChildWindowModal('/hrm/ConfirmWork/WorkingNote', 550, 330, "Thêm tiến độ thực hiện công việc");
    });
    $("#grdMainWorkingNote[event-dbclick='1']").on("dblclick", "tr.k-state-selected", function () {
        $('#btnEditWorkingNote').click();
    });
    $('#btnEditWorkingNote').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdMainWorkingNote');
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
        InitChildWindowModal('/hrm/ConfirmWork/WorkingNote?id=' + id, 550, 330, "Cập nhật tiến độ thực hiện công việc");
    });
    $('#btnDeleteWorkingNote').click(function () {
        var id = GetGridRowSelectedKeyValue("#grdMainWorkingNote");
        if (id != null && id != typeof undefined) {
            $('#processing').show();
            $.ajax({
                type: 'POST',
                url: '/hrm/ConfirmWork/DeleteWorkingNote',
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
                                GridCallback('#grdMainWorkingNote');
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
    $("#btnSaveAdd").click(function () {
        var status = $('input[name=optradio]:checked').val();
        if (status === '5' && ($("#Explanation").val() == null || $("#Explanation").val().trim() === "")) {
            InitChildWindowModal('/hrm/ConfirmWork/Explanation', 600, 300, 'Giải trình công việc');
            return;
        }
        if (status === '3' && $("#UsefulHours").data("kendoNumericTextBox").value() === null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn cần nhập số giờ làm việc thực tế khi xác nhận hoàn thành công việc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        //if (status === '3' && ($("#FileConfirm").val() === null || $("#FileConfirm").val() === "")) {
        //    $.msgBox({
        //        title: "Hệ thống",
        //        type: "error",
        //        content: "Bạn phải tải File xác minh hoàn thành công việc!",
        //        buttons: [{ value: "Đồng ý" }],
        //        success: function () {
        //        }
        //    });
        //    return;
        //}
        if (status === '3' && $("#UsefulHours").data("kendoNumericTextBox").value() === 0) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Số giờ làm việc thực tế phải lớn hơn 0!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
            var model = {
                WorkDetailId: $("#WorkDetailId").val(),
                WorkType: $("#WorkType").val(),
                Status: status,
                UsefulHours: $("#UsefulHours").data("kendoNumericTextBox").value(),
                Explanation: $("#Explanation").val(),
                FisnishDate: window.finishDate,
                ToDate: $("#ToDate").val(),
                FileConfirm: $("#FileConfirm").val()
            }
            $('#processing').show();
            $.ajax({
                type: 'POST',
                url: '/hrm/ConfirmWork/Save',
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
                                CloseWindowModal();
                                GridCallback('#grdMain');
                            }
                        });
                    }
                }
            });
        
        
         
    });

    $('#btnUploadFileConfirm').click(function () {
        $('#UploadFileConfirm').click();
    });
    $('#UploadFileConfirm').change(function () {
        var data = new FormData();
        var files = $("#UploadFileConfirm").get(0).files;
        if (files.length > 0) {
            data.append("File", files[0]);
        }
        $('#processing').show();
        $.ajax({
            url: '/hrm/ConfirmWork/UploadFile',
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                $('#processing').hide();
                if (response.Status === 1) {
                    $.msgBox({
                        title: "Hệ thống",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                            $('#FileConfirm').val(response.Url);
                        }
                    });
                } else {
                    $.msgBox({
                        title: "Hệ thống",
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
                    title: "Hệ thống",
                    type: "error",
                    content: er,
                    buttons: [{ value: "Đồng ý" }]
                });
            }
        });
    });
    $('#btnRemoveFileConfirm').click(function () {
        var filePath = $('#FileConfirm').val();
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/ConfirmWork/DeleteFile',
            data: JSON.stringify({ filePath: filePath }),
            contentType: 'application/json;charset=utf-8',
            success: function (response) {
                $('#processing').hide();
                if (response.Status === 1) {
                    $.msgBox({
                        title: "Hệ thống",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                            $('#FileConfirm').val('');
                        }
                    });
                } else {
                    $.msgBox({
                        title: "Hệ thống",
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

});