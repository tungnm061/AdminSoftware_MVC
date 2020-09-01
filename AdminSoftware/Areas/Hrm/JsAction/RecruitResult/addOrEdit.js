var recordDetail = 0;
$(document).ready(function () {
    $('#Result').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: recruitResults
    });
    $('#EmployeeId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: employees,
        filter: 'startswith',
        optionLabel:' '
    });
    $("#grdRecruitResultDetails").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/RecruitResult/RecruitResultDetails',
                        dataType: "json",
                        contentType: 'application/json;charset=utf-8',
                        success: function (response) {
                            options.success(response);
                        }
                    });
                }
            },
            schema: {
                model: {
                    id: "RecruitResultDetailId",
                    fields: {
                        InterviewDate: { type: 'date' }
                    }
                }
            },
            pageSize: 1000,
            serverPaging: false,
            serverFiltering: false
        },
        height: 200,
        filterable: false,
        sortable: false,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                title: "STT",
                template: "#= ++recordDetail #",
                width: 60
            },
            {
                field: "EmployeeId",
                title: "Người PV",
                width: 200,
                values: employees
            },
            {
                field: "Result",
                title: "Kết quả",
                width: 120,
                values: recruitResults
            },
            {
                field: "Description",
                title: "Ghi chú"
            },
            {
                field: "InterviewDate",
                title: "Ngày PV",
                format: "{0:dd/MM/yyyy}",
                width: 150
            }
        ],
        dataBinding: function () {
            recordDetail = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
    $('#btnCreateResult').click(function() {
        InitChildWindowModal('/hrm/RecruitResult/RecruitResultDetail?id=""', 600, 350, "Cập nhật kết quả");
    });
    $('#btnEditResult').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdRecruitResultDetails');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/RecruitResult/RecruitResultDetail?id=' + id, 600, 350, "Cập nhật kết quả");
    });
    $('#btnDeleteResult').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdRecruitResultDetails');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi xóa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/RecruitResult/DeleteRecruitResultDetail',
            data: JSON.stringify({ id: id }),
            contentType: 'application/json;charset=utf-8',
            success: function () {
                $('#processing').hide();
                GridCallback('#grdRecruitResultDetails');
            }
        });
    });
    $('#btnSave').click(function () {
        var model = {
            RecruitResultId: recruitResultId,
            ApplicantId: $('#ApplicantId').val(),
            RecruitPlanId: $('#RecruitPlanId').val(),
            Result: $('#Result').data('kendoDropDownList').value(),
            EmployeeId: $('#EmployeeId').data('kendoDropDownList').value(),
            Description: $('#Description').val()
        }
        if (model.ApplicantId == null || model.ApplicantId.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Không có ứng viên!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.RecruitPlanId == null || model.RecruitPlanId.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Không có kế hoạch tuyển dụng!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.Result == null || model.Result.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn kết quả!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/RecruitResult/Save',
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
                            CloseWindowModal();
                            GridCallback('#grdMain');
                        }
                    } //
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