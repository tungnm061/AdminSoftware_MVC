var record = 0;
$(document).ready(function () {
    $('#RecruitPlanIdSelect').kendoDropDownList({
        dataTextField: 'text',
        dataValueField: 'value',
        dataSource: recruitPlans,
        optionLabel:'Chọn kế hoạch'
    });
    $('#btnDelete').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $.msgBox({
            title: "Hệ thống ERP",
            type: "confirm",
            content: "Bạn có chắc chắn muốn xóa dữ liệu không?",
            buttons: [{ value: "Đồng ý" }, { value: "Hủy bỏ" }],
            success: function (result) {
                if (result === "Đồng ý") {
                    $('#processing').show();
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/Applicant/Delete',
                        data: JSON.stringify({ id: id }),
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
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    });
    $('#btnCreate').bind("click", function () {
        InitWindowModal('/hrm/Applicant/Applicant', true, 600, 400, "Thêm mới hồ sơ ứng viên", false);
    });
    $('#btnEdit').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitWindowModal('/hrm/Applicant/Applicant?id=' + id, true, 600, 400, "Cập nhật hồ sơ ứng viên", false);
    });
    $('#btnSearch').click(function () {
        var newDataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/Applicant/Applicants',
                        dataType: "json",
                        data: JSON.stringify({
                            recruitPlanId: $('#RecruitPlanIdSelect').data('kendoDropDownList').value()
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
                    id: "ApplicantId",
                    fields: {
                        CreateDate: { type: 'date' },
                        CvDate: { type: 'date' },
                        DateOfBirth: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        });
        var grid = $("#grdMain").data("kendoGrid");
        grid.setDataSource(newDataSource);
    });
    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "danhsachungvien.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/Applicant/Applicants',
                        dataType: "json",
                        data: JSON.stringify({
                            recruitPlanId: $('#RecruitPlanIdSelect').data('kendoDropDownList').value()
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
                    id: "ApplicantId",
                    fields: {
                        CreateDate: { type: 'date' },
                        CvDate: { type: 'date' },
                        DateOfBirth:{type:'date'}
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: gridHeight,
        filterable: true,
        sortable: true,
        pageable: {
            refresh: true
        },
        selectable: 'row',
        columns: [
            {
                title: "STT",
                template: "#= ++record #",
                width: 60
            },
            {
                field: "FullName",
                title: "Họ tên",
                width: 170
            },
            {
                field: "Sex",
                title: "Giới tính",
                width: 130,
                values:sexs
            },
            {
                field: "DateOfBirth",
                title: "Ngày sinh",
                width: 150,
                format:'{0:dd/MM/yyyy}'
            },
            {
                field: "PhoneNumber",
                title: "Điện thoại",
                width: 150
            },
            {
                field: "Email",
                title: "Email",
                width: 150
            },
            {
                field: "CityBirthPlace",
                title: "Nơi sinh",
                width: 150,
                values: cities
            },
            {
                field: "PermanentAddress",
                title: "Địa chỉ",
                width: 250
            },
            {
                field: "IdentityCardNumber",
                title: "Số CMND",
                width: 150
            },
            {
                field: "TrainingLevelId",
                title: "Trình độ",
                width: 150,
                values: trainingLevels
            },
            {
                field: "CvDate",
                title: "Ngày nộp",
                format: "{0:dd/MM/yyyy}",
                width: 150
            },
            {
                field: "CreateBy",
                title: "Người tạo",
                width: 150,
                values: users
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                format: "{0:dd/MM/yyyy hh:mm tt}",
                width: 180
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});