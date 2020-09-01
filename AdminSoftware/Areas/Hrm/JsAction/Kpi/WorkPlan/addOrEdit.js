
$(document).ready(function () {
    $("#FromDate,#ToDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"

    });

    $("#grdWorkPlanDetail").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/WorkPlan/WorkPlanDetails',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "WorkPlanDetailId",
                    fields: {
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' },
                        UsefulHourTask: { type: 'number' }
                    }
                }
            },
            pageSize: 10000,
            serverPaging: false,
            serverFiltering: false
        },
        height: 385,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            //{
            //    title: "Loại Công Việc",
            //    width: 140,
            //    template: '#= AssignWorkId  != null ? "Được giao" : "Đề xuất" #'
            //},
            {
                field: "TaskCode",
                title: "Mã công việc",
                width :140

            },
            {
                field: "TaskName",
                title: "Tên công việc",
                width: 300
            },
             {
                 field: "UsefulHourTask",
                 title: "Giờ hữu ích quy định",
                 width: 160
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
                width: 120

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
                width: 280
            }

        ]
    });
    $('#btnAddWorkPlanDetail').click(function () {
        InitChildWindowModal('/hrm/WorkPlan/WorkPlanDetail', 550, 330, "Thêm công việc");
    });
    $("#grdWorkPlanDetail[event-dbclick='1']").on("dblclick", "tr.k-state-selected", function () {
        $('#btnEditWorkPlanDetail').click();
    });
    $('#btnEditWorkPlanDetail').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdWorkPlanDetail');
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
        InitChildWindowModal('/hrm/WorkPlan/WorkPlanDetail?id=' + id, 550, 330, "Cập nhật công việc");
    });
    $('#btnRemoveWorkPlanDetail').click(function () {
        var id = GetGridRowSelectedKeyValue("#grdWorkPlanDetail");
        if (id != null && id != typeof undefined) {
            $('#processing').show();
            $.ajax({
                type: 'POST',
                url: '/hrm/WorkPlan/DeleteDetail',
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
                                GridCallback('#grdWorkPlanDetail');
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
        var model = {
            WorkPlanId: workPlanId,
            WorkPlanCode: $('#WorkPlanCode').val(),
            FromDate: $('#FromDate').data('kendoDatePicker').value(),
            ToDate: $('#ToDate').data('kendoDatePicker').value(),
            CreateBy: createby,
            CreateDate: createdate,
            Description: $('#Description').val()
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
            url: '/hrm/WorkPlan/Save',
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