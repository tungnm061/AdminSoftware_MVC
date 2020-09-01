
$(document).ready(function () {
    $("#FromDate,#ToDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"

    });
    $('#ModalTask').click(function () {
        $("#CheckTaskSearch").val("1");
        InitChildOfChildWindowModal('/hrm/WorkStream/TaskSearch', 800, 520, 'Danh sách công việc ', false);
    });
    $('#ModalAssignWork').click(function () {
        InitChildWindowModal('/hrm/WorkStream/AssignWorkSearch', 800, 400, 'Danh sách công việc được giao ', false);
    });
    $("#select").kendoMultiSelect({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: users,
        value: performers,
        autoClose: false
    });
    $("#grdWorkStreamDetail").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/WorkStream/WorkStreamDetails',
                dataType: "json"
            },
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
        height: 200,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
             {
                 field: "DepartmentName",
                 title: "Phòng",
                 width: 160
             },
              {
                  field: "CreateByCode",
                  title: "Mã nhân viên",
                  width: 120
              },
              {
                  field: "CreateByName",
                  title: "Họ và tên",
                  width: 120
              },
            {
                field: "TaskCode",
                title: "Mã công việc",
                width :140

            },
            {
                field: "TaskName",
                title: "Tên công việc",
                width: 280
            },
              {
                  field: "Quantity",
                  title: "Số lượng",
                  width: 90
              },
            {
                field: "FromDate",
                title: "Ngày bắt đầu",
                format: "{0:dd/MM/yyyy}",
                width :120

            },
            {
                field: "ToDate",
                title: "Ngày hoàn thành",
                format: "{0:dd/MM/yyyy}",
                width: 130
            },
            {
                field: "Description",
                title: "Mô tả công việc",
                width: 260
            }

        ]
    });
    $('#btnAddDetail').click(function () {
        InitChildWindowModal('/hrm/WorkStream/WorkStreamDetail', 550, 310, "Thêm công việc nhóm");
    });
    $("#grdWorkStreamDetail[event-dbclick='1']").on("dblclick", "tr.k-state-selected", function () {
        $('#btnEditDetail').click();
    });
    $('#btnEditDetail').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdWorkStreamDetail');
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
        InitChildWindowModal('/hrm/WorkStream/WorkStreamDetail?id=' + id, 550, 280, "Cập nhật công việc");
    });
    $('#btnRemoveDetail').click(function () {
        var id = GetGridRowSelectedKeyValue("#grdWorkStreamDetail");
        if (id != null && id != typeof undefined) {
            $('#processing').show();
            $.ajax({
                type: 'POST',
                url: '/hrm/WorkStream/DeleteDetail',
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
                                GridCallback('#grdWorkStreamDetail');
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
    $('#btnSave').click(function () {
        if ($('#TaskName').val() == null || $('#TaskName').val()==="") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải chọn công việc phối hợp các phòng!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        var dateFromOld = kendo.toString(kendo.parseDate(window.formDateOlde));
        var dateNowParse = kendo.toString(kendo.parseDate(window.dateNow));
        var model = {
            WorkStreamId: workStreamId,
            WorkStreamCode: $('#WorkStreamCode').val(),
            FromDate: $('#FromDate').data('kendoDatePicker').value(),
            ToDate: $('#ToDate').data('kendoDatePicker').value(),
            CreateBy: createby,
            CreateDate: createdate,
            Description: $('#Description').val(),
            AssignWorkId: $('#AssignWorkId').val(),
            TaskId: $('#TaskId').val(),
            Status: $('#Status').val(),
            PerformerBys: $("#select").data("kendoMultiSelect").value(),
            ApprovedDate: approvedDate,
            ApprovedBy : approvedBy

        }
        if (model.PerformerBys == null || model.PerformerBys.length === 0) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn người thực hiện!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if ($('#ToDate').data('kendoDatePicker').value() == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải chọn ngày hoàn thành!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if ($('#FromDate').data('kendoDatePicker').value() == null) {
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
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/WorkStream/Save',
            dataType: "json",
            data: JSON.stringify({
                model: model
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