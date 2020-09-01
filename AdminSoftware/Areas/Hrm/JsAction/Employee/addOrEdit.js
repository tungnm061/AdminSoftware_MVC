function OpenDetail(id) {
    $("#EmployeeMediaId").val(id);
    InitChildWindowModal('/hrm/Employee/IndexProcess', 900, 550, "Qúa trình tham gia bảo hiểm");
}
$(document).ready(function () {
    $('#Gender').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: sexs
    }); 
    $('#DepartmentCompany').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: departmentEnum,
        optionLabel: " "
    }); 
    $("#btnCreateInsurance").click(function() {
        InitChildWindowModal('/hrm/Employee/IndexInsurance', 900, 550, "Thêm mới thông tin BHXH");
    });
    $("#btnAddMedical").click(function () {
        InitChildWindowModal('/hrm/Employee/IndexMedical', 900, 550, "Thêm mới nơi khám bệnh");
    });
    $("#CategoryKpiId").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: categoryKpis,
        filter:'contains'
    });
    $("#ShiftWorkId").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: shiftWorks,
        filter: 'contains'
    });
    
    $("#ContractType").click(function () {
        InitChildWindowModal('/hrm/Employee/IndexContractType', 900, 550, "Quản lý loại hợp đồng");
    });
    $("#BtnSearchCountry").click(function() {
            InitChildWindowModal('/hrm/Employee/CountrySearch', 720, 550, "Tìm kiếm dữ liệu");
        });
    $("#BtnSearchNation").click(function () {
            InitChildWindowModal('/hrm/Employee/NationSearch', 720, 550, "Tìm kiếm dữ liệu");
        });
    $("#BtnSearchReligion").click(function () {
            InitChildWindowModal('/hrm/Employee/ReligionSearch', 720, 550, "Tìm kiếm dữ liệu");
        });
    $("#BtnSearchTrainingLevel").click(function () {
            InitChildWindowModal('/hrm/Employee/TrainingLevelSearch', 720, 550, "Tìm kiếm dữ liệu");
        });
    $("#BtnSearchPosition").click(function () {
            InitChildWindowModal('/hrm/Employee/PositionSearch', 720, 550, "Tìm kiếm dữ liệu");
    });
    $("#BtnSearchEducationLevel").click(function () {
        InitChildWindowModal('/hrm/Employee/EducationLevelSearch', 720, 550, "Tìm kiếm dữ liệu");
    });
    $("#BtnSearchSchool").click(function () {
        InitChildWindowModal('/hrm/Employee/SchoolSearch', 720, 550, "Tìm kiếm dữ liệu");
    });
    $("#BtnSearchCareer").click(function () {
        InitChildWindowModal('/hrm/Employee/CareerSearch', 720, 550, "Tìm kiếm dữ liệu");
    });
    $('#DateOfBirth,#DateOfYouthUnionAdmission,#DateOfPartyAdmission,#IdentityCardDate,#WorkedDate').kendoDatePicker({
        format: "dd/MM/yyyy",
        dateInput: true,
        max:new Date()
    });
    $('#MaritalStatus').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: maritalStatuses,
        filter: 'contains'
    });
    $('#DepartmentId').kendoDropDownTree({
        dataSource: {
            type: 'json',
            data: departmentTrees,
            schema: {
                model: {
                    id: "value",
                    children: "ChildModels"
                }
            }
        },
        dataTextField: "text",
        dataValueField: "value",
        clearButton: true,
        placeholder: "Chọn phòng ban",
        height: "200px",
        filter: 'contains'
    });
    $('#CityIdentityCard,#CityBirthPlace,#CityNativeLand').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: cities,
        optionLabel: " ",
        filter: 'contains'
    });
    $("#TemperaryDistrict,#PermanentDistrict").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        optionLabel: " ",
        filter: 'contains'
    });
    $('#TemperaryCity').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: cities,
        optionLabel: " ",
        change:function() {
            var cityId = $("#TemperaryCity").data('kendoDropDownList').value();
            if (cityId == null || cityId.trim() === "") {
                cityId = 0;
            }
            $('#processing').show();
            $.ajax({
                type: 'POST',
                url: '/hrm/Employee/DistrictsByCityId',
                data: JSON.stringify({ cityId: cityId }),
                contentType: 'application/json;charset=utf-8',
                success: function (response) {
                    $('#processing').hide();
                    $("#TemperaryDistrict").data("kendoDropDownList").setDataSource(response);
                }
            });
        },
        dataBound: function (e) {
            var cityId = $("#TemperaryCity").data('kendoDropDownList').value();
            if (cityId == null || cityId.trim() === "") {
                cityId = 0;
            }
            $('#processing').show();
            $.ajax({
                type: 'POST',
                url: '/hrm/Employee/DistrictsByCityId',
                data: JSON.stringify({ cityId: cityId }),
                contentType: 'application/json;charset=utf-8',
                success: function (response) {
                    $('#processing').hide();
                    $("#TemperaryDistrict").data("kendoDropDownList").setDataSource(response);
                    $("#TemperaryDistrict").data("kendoDropDownList").value(temperaryDistrict);
                }
            });
        },
        filter: 'contains'
    });
    $('#PermanentCity').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: cities,
        optionLabel: " ",
        change: function () {
            var cityId = $("#PermanentCity").data('kendoDropDownList').value();
            if (cityId == null || cityId.trim() === "") {
                cityId = 0;
            }
            $('#processing').show();
            $.ajax({
                type: 'POST',
                url: '/hrm/Employee/DistrictsByCityId',
                data: JSON.stringify({ cityId: cityId }),
                contentType: 'application/json;charset=utf-8',
                success: function (response) {
                    $('#processing').hide();
                    $("#PermanentDistrict").data("kendoDropDownList").setDataSource(response);
                }
            });
        },
        dataBound: function (e) {
            var cityId = $("#PermanentCity").data('kendoDropDownList').value();
            if (cityId == null || cityId.trim() === "") {
                cityId = 0;
            }
            $('#processing').show();
            $.ajax({
                type: 'POST',
                url: '/hrm/Employee/DistrictsByCityId',
                data: JSON.stringify({ cityId: cityId }),
                contentType: 'application/json;charset=utf-8',
                success: function (response) {
                    $('#processing').hide();
                    $("#PermanentDistrict").data("kendoDropDownList").setDataSource(response);
                    $("#PermanentDistrict").data("kendoDropDownList").value(permanentDistrict);
                }
            });
        },
        filter: 'contains'
    });
    $('#btnUpload').click(function () {
        $('#FileUpload').click();
    });
    $('#FileUpload').change(function () {
        var data = new FormData();
        var files = $("#FileUpload").get(0).files;
        if (files.length > 0) {
            data.append("ImageAvatar", files[0]);
        }
        $('#processing').show();
        $.ajax({
            url: '/hrm/Employee/UploadAvatar',
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
                            $('#Avatar').attr('src', response.Url);
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
                    buttons: [{ value: "Đồng ý" }],
                });
            }
        });
    });
    $('#btnRemoveImage').click(function() {
        var imageUrl = $('#Avatar').attr('src');
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/Employee/DeleteAvatar',
            data: JSON.stringify({ imageUrl: imageUrl }),
            contentType: 'application/json;charset=utf-8',
            success: function (response) {
                $('#processing').hide();
                if (response.Status === 1) {
                    $.msgBox({
                        title: "Hệ thống ERP",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function() {
                            $('#Avatar').attr('src', response.Url);
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
    $('#btnSaveEmployee').click(function() {
        var model = {
            EmployeeId: employeeId,
            EmployeeCode: $('#EmployeeCode').val(),
            FullName: $('#FullName').val(),
            DateOfBirth: $('#DateOfBirth').data('kendoDatePicker').value(),
            Gender: $('#Gender').data('kendoDropDownList').value(),
            SpecialName: $('#SpecialName').val(),
            Avatar: $('#Avatar').attr('src'),
            DepartmentId: $('#DepartmentId').data('kendoDropDownTree').value(),
            CountryId: $('#CountryId').val(),
            //NationId: $('#NationId').val(),
            ReligionId: $('#ReligionId').val(),
            MaritalStatus: $('#MaritalStatus').data('kendoDropDownList').value(),
            //CityBirthPlace: $('#CityBirthPlace').data('kendoDropDownList').value(),
            //CityNativeLand: $('#CityNativeLand').data('kendoDropDownList').value(),
            IdentityCardNumber: $('#IdentityCardNumber').val(),
            IdentityCardDate: $('#IdentityCardDate').data('kendoDatePicker').value(),
            CityIdentityCard: $('#CityIdentityCard').data('kendoDropDownList').value(),
            ShiftWorkId: $('#ShiftWorkId').data('kendoDropDownList').value(),
            //PermanentAddress: $('#PermanentAddress').val(),
            //PermanentCity: $('#PermanentCity').data('kendoDropDownList').value(),
            //PermanentDistrict: $('#PermanentDistrict').data('kendoDropDownList').value(),
            //TemperaryAddress: $('#TemperaryAddress').val(),
            //TemperaryCity: $('#TemperaryCity').data('kendoDropDownList').value(),
            //TemperaryDistrict: $('#TemperaryDistrict').data('kendoDropDownList').value(),
            //DepartmentCompany: $('#DepartmentCompany').data('kendoDropDownList').value(),
            Email: $('#Email').val(),
            PhoneNumber: $('#PhoneNumber').val(),
            PositionId: $('#PositionId').val(),
            TrainingLevelId: $('#TrainingLevelId').val(),
            HealthStatus: $('#HealthStatus').val(),
            //DateOfYouthUnionAdmission: $('#DateOfYouthUnionAdmission').data('kendoDatePicker').value(),
            //PlaceOfYouthUnionAdmission: $('#PlaceOfYouthUnionAdmission').val(),
            //DateOfPartyAdmission: $('#DateOfPartyAdmission').data('kendoDatePicker').value(),
            //PlaceOfPartyAdmission: $('#PlaceOfPartyAdmission').val(),
            //Skill: $('#Skill').val(),
            Experience: $('#Experience').val(),
            Description: $('#DescriptionEmployee').val(),
            IsActive: $('#IsActive').is(":checked"),
            //TimeSheetCode: $('#TimeSheetCode').val(),
            Status: status,
            //WorkedDate: $('#WorkedDate').data('kendoDatePicker').value(),
            //EducationLevelId: $('#EducationLevelId').val(),
            //SchoolId: $('#SchoolId').val(),
            //CareerId: $('#CareerId').val(),
            CreateBy: createBy,
            CreateDate: createDate,
            //CategoryKpiId: $('#CategoryKpiId').data('kendoDropDownList').value()
        };
        //if (model.CategoryKpiId == null || model.CategoryKpiId.trim() === "" || model.CategoryKpiId ==="0") {
        //    $.msgBox({
        //        title: "Hệ thống ERP",
        //        type: "error",
        //        content: "Kpi không được để trống!",
        //        buttons: [{ value: "Đồng ý" }],
        //        success: function () {
        //        }
        //    });
        //    return;
        //}
        if (model.EmployeeCode == null || model.EmployeeCode.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Mã nhân viên không được để trống!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.FullName == null || model.FullName.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải nhập tên nhân viên!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.Gender == null || model.Gender.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải chọn giới tính nhân viên!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.DateOfBirth == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải chọn ngày sinh nhân viên!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.DepartmentId == null || model.DepartmentId.trim() === "" || model.DepartmentId.trim() === "0") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải chọn phòng ban nhân viên!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }

        if (model.CountryId == null || model.CountryId.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải chọn quốc tịch!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        //if (model.NationId == null || model.NationId.trim() === "") {
        //    $.msgBox({
        //        title: "Hệ thống ERP",
        //        type: "error",
        //        content: "Bạn phải chọn dân tộc!",
        //        buttons: [{ value: "Đồng ý" }],
        //        success: function () {
        //        }
        //    });
        //    return;
        //}
        //if (model.ReligionId == null || model.ReligionId.trim() === "") {
        //    $.msgBox({
        //        title: "Hệ thống ERP",
        //        type: "error",
        //        content: "Bạn phải chọn tôn giáo!",
        //        buttons: [{ value: "Đồng ý" }],
        //        success: function () {
        //        }
        //    });
        //    return;
        //}
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/Employee/Save',
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
                            GridCallback('#grdMainEmployee');
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
    // Hop Dong
    $('#btnDeleteContract').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainContract');
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
                        url: '/hrm/Employee/DeleteContract',
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
                                        GridCallback('#grdMainContract');
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    });
    $('#btnCreateContract').bind("click", function () {
        var id = $('#EmployeeIdPost').val();
        if (id === '0') {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn cần phải đăng ký thông tin nhân viên trước!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Employee/Contract', 550, 410, "Thêm mới hợp đồng");
    });
    $('#btnEditContract').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainContract');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Employee/Contract?id=' + id, 550, 410, "Cập nhật hợp đồng");
    });
    $("#grdMainContract").kendoGrid({
        dataSource: {
            transport: {     
            read: function (options) {
                $.ajax(
                        {
                            type: 'POST',
                            url: '/hrm/Employee/Contracts',
                            dataType: "json",
                            data: JSON.stringify({
                                employeeId: $("#EmployeeIdPost").val()
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
                    id: "ContractId",
                    fields: {
                        CreateDate: { type: 'date' },
                        StartDate: { type: 'date' },
                        EndDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "ContractCode",
                title: "Mã hợp đồng",
                width: 150
            },
            {
                field: "ContractTypeId",
                title: "Loại hợp đồng",
                values: contractTypes,
                width: 170
            },
            {
                field: "StartDate",
                title: "Từ ngày",
                format: "{0:dd/MM/yyyy}",
                width: 150
            },
            {
                field: "EndDate",
                title: "Từ ngày",
                format: "{0:dd/MM/yyyy}",
                width: 150
            },
            {
                field: "ContractFile",
                title: "Hợp đồng",
                width: 120,
                template: "#if(ContractFile != null){#" + "<a href='#=ContractFile#' style='text-align:center'><i class='glyphicon glyphicon-download'></i></a>" + "#}#",
                filterable: false,
                sortable: false
            },
            {
                field: "ContractOthorFile",
                title: "Phụ lục",
                width: 120,
                template: "#if(ContractOthorFile != null){#" + "<a href='#=ContractOthorFile#' style='text-align:center'><i class='glyphicon glyphicon-download'></i></a>" + "#}#",
                filterable: false,
                sortable: false
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
        }
    });
    // Praise Khen thuong 
    $('#btnDeletePraise').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainPraise');
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
                        url: '/hrm/Employee/DeletePraise',
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
                                        GridCallback('#grdMainPraise');
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    });
    $('#btnCreatePraise').bind("click", function () {
        var id = $('#EmployeeIdPost').val();
        if (id === '0') {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn cần phải đăng ký thông tin nhân viên trước!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Employee/Praise', 800, 400, "Thêm mới khen thưởng");
    });
    $('#btnEditPraise').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainPraise');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Employee/Praise?id=' + id, 800, 400, "Cập nhật khen thưởng");
    });
    $("#grdMainPraise").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                            {
                                type: 'POST',
                                url: '/hrm/Employee/Praises',
                                dataType: "json",
                                data: JSON.stringify({
                                    employeeId: $("#EmployeeIdPost").val()
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
                    id: "PraiseDisciplineId",
                    fields: {
                        CreateDate: { type: 'date' },
                        PraiseDisciplineDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "PraiseDisciplineCode",
                title: "Mã KT",
                width: 150
            },
            {
                field: "Title",
                title: "Quyết định",
                width: 200
            },
            {
                field: "DecisionNumber",
                title: "Số quyết định",
                width: 150
            },
            {
                field: "PraiseDisciplineDate",
                title: "Ngày quyết định",
                format: "{0:dd/MM/yyyy}",
                width: 180
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
        }
    });
    // Ky Luat
    $('#btnDeleteDiscipline').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainDiscipline');
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
                        url: '/hrm/Employee/DeleteDiscipline',
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
                                        GridCallback('#grdMainDiscipline');
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    });
    $('#btnCreateDiscipline').bind("click", function () {
        var id = $('#EmployeeIdPost').val();
        if (id === '0') {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn cần phải đăng ký thông tin nhân viên trước!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Employee/Discipline', 800, 390, "Thêm mới kỷ luật");
    });
    $('#btnEditDiscipline').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainDiscipline');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Employee/Discipline?id=' + id, 800, 390, "Cập nhật kỷ luật");
    });
    $("#grdMainDiscipline").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                            {
                                type: 'POST',
                                url: '/hrm/Employee/Disciplines',
                                dataType: "json",
                                data: JSON.stringify({
                                    employeeId: $("#EmployeeIdPost").val()
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
                    id: "PraiseDisciplineId",
                    fields: {
                        CreateDate: { type: 'date' },
                        PraiseDisciplineDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [

            {
                field: "PraiseDisciplineCode",
                title: "Mã KL",
                width: 150
            },
            {
                field: "Title",
                title: "Quyết định",
                width: 200
            },
            {
                field: "DecisionNumber",
                title: "Số quyết định",
                width: 150
            },
            {
                field: "PraiseDisciplineDate",
                title: "Ngày quyết định",
                format: "{0:dd/MM/yyyy}",
                width: 180
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
        }
    });
    // JOB CHANGE

    $('#btnDeleteJobChange').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainJobChange');
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
                        url: '/hrm/Employee/DeleteJobChange',
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
                                        GridCallback('#grdMainJobChange');
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    });
    $('#btnCreateJobChange').bind("click", function () {
        var id = $('#EmployeeIdPost').val();
        if (id === '0') {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn cần phải đăng ký thông tin nhân viên trước!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Employee/JobChange', 900, 380, "Thêm mới thuyên chuyển công tác");
    });
    $('#btnEditJobChange').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainJobChange');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Employee/JobChange?id=' + id, 900, 380, "Cập nhật thuyên chuyển công tác");
    });
    $("#grdMainJobChange").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                            {
                                type: 'POST',
                                url: '/hrm/Employee/JobChanges',
                                dataType: "json",
                                data: JSON.stringify({
                                    employeeId: $("#EmployeeIdPost").val()
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
                    id: "JobChangeId",
                    fields: {
                        CreateDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "JobChangeCode",
                title: "Số phiếu",
                width: 150
            },
            {
                field: "JobChangeNumber",
                title: "Số quyết định",
                width: 150
            },
            {
                field: "FromDepartmentId",
                title: "Phòng cũ",
                width: 170,
                values: departments
            },
            {
                field: "ToDepartmentId",
                title: "Phòng mới",
                width: 170,
                values: departments
            },
            {
                field: "FromPositionId",
                title: "Chức vụ cũ",
                width: 170,
                values: positions
            },
            {
                field: "ToPositionId",
                title: "Chức vụ mới",
                width: 170,
                values: positions
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
                width: 170
            },
            {
                field: "Reason",
                title: "Lý do",
                width: 250
            }
        ],
        dataBinding: function () {
        }
    });
    // Salary
    $('#btnDeleteSalary').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdSalaries');
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
                        url: '/hrm/Employee/DeleteSalary',
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
                                        GridCallback('#grdSalaries');
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    });
    $('#btnCreateSalary').bind("click", function () {
        var id = $('#EmployeeIdPost').val();
        if (id === '0') {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn cần phải đăng ký thông tin nhân viên trước!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Employee/Salary', 600, 280, "Thêm mới quá trình tăng giảm lương");
    });
    $('#btnEditSalary').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdSalaries');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Employee/Salary?id=' + id, 600, 280, "Cập nhật quá trình tăng giảm lương");
    });

    $("#grdSalaries").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/Employee/Salaries',
                        dataType: "json",
                        data: JSON.stringify({
                            employeeId: $('#EmployeeIdPost').val()
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
                    id: "SalaryId",
                    fields: {
                        CreateDate: { type: 'date' },
                        ApplyDate: { type: 'date' },
                        BasicSalary: { type: 'number' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "ApplyDate",
                title: "Áp dụng từ",
                width: 170,
                format: '{0:MM/yyyy}'
            },
            {
                field: "BasicSalary",
                title: "Mức lương cơ sở",
                width: 200,
                format: '{0:###,###}'
            },
            {
                field: "BasicCoefficient",
                title: "Hệ số lương CB",
                width: 150
            },
            {
                field: "ProfessionalCoefficient",
                title: "Hệ số chuyện môn",
                width: 150
            },
            {
                field: "ResponsibilityCoefficient",
                title: "Hệ số lương TN",
                width: 150
            },
            {
                field: "CreateBy",
                title: "Người tạo",
                width: 130,
                values: users
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                format: "{0:dd/MM/yyyy}",
                width: 150
            }
        ],
        dataBinding: function () {
        }
    });
    // Lương phát sinh
    $('#FromDateIncurredSalary,#ToDateIncurredSalary').kendoDatePicker({
        max: new Date(),
        dateInput: true,
        format: '{0:dd/MM/yyyy}'
    });
    $("#grdMainIncurredSalary").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/Employee/IncurredSalaries',
                        dataType: "json",
                        data: JSON.stringify({
                            fromDate: $('#FromDateIncurredSalary').data('kendoDatePicker').value(),
                            toDate: $('#ToDateIncurredSalary').data('kendoDatePicker').value(),
                            employeeId: employeeId
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
                    id: "IncurredSalaryId",
                    fields: {
                        CreateDate: { type: 'date' },
                        SubmitDate: { type: 'date' },
                        Amount: { type: 'number' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
        filterable: true,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "Title",
                title: "Lý do",
                width: 200
            },
            {
                field: "EmployeeCode",
                title: "Mã nhân viên",
                width: 150
            },
            {
                field: "FullName",
                title: "Tên nhân viên",
                width: 200
            },
            {
                field: "Amount",
                title: "Số tiền",
                width: 150,
                format: '{0:###,###}'
            },
            {
                field: "SubmitDate",
                title: "Kỳ trả",
                width: 150,
                format: '{0:dd/MM/yyyy}'
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                width: 150,
                format: '{0:dd/MM/yyyy}'
            },
            {
                field: "CreateBy",
                title: "Người tạo",
                width: 150,
                values: users
            }
        ],
        dataBinding: function () {
        }
    });
    $('#btnSearchIncurredSalary').click(function () {
        var fromDate = $('#FromDateIncurredSalary').data('kendoDatePicker').value();
        var toDate = $('#ToDateIncurredSalary').data('kendoDatePicker').value();
        if (fromDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn ngày bắt đầu!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (toDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn ngày kết thúc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (toDate < fromDate) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Ngày bắt đầu không được lớn hơn ngày kết thúc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        var newDataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/Employee/IncurredSalaries',
                        dataType: "json",
                        data: JSON.stringify({
                            fromDate: $('#FromDateIncurredSalary').data('kendoDatePicker').value(),
                            toDate: $('#ToDateIncurredSalary').data('kendoDatePicker').value()
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
                    id: "IncurredSalaryId",
                    fields: {
                        CreateDate: { type: 'date' },
                        SubmitDate: { type: 'date' },
                        Amount: { type: 'number' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        });
        var grid = $("#grdMainIncurredSalary").data("kendoGrid");
        grid.setDataSource(newDataSource);
    });
    $('#btnEditIncurredSalary').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdMainIncurredSalary');
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
        InitChildWindowModal('/hrm/Employee/IncurredSalary?id=' + id, 600, 350, 'Cập nhật lương phát sinh');
    });
    $('#btnCreateIncurredSalary').click(function () {
        var id = $('#EmployeeIdPost').val();
        if (id === '0') {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn cần phải đăng ký thông tin nhân viên trước!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Employee/IncurredSalary',  600, 350, 'Thêm mới lương phát sinh');
    });
    $('#btnDeleteIncurredSalary').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "info",
                content: "Bạn phải chọn dữ liệu trước khi xóa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $.msgBox({
            title: "Hệ thống",
            type: "confirm",
            content: "Bạn có chắc chắn muốn xóa dữ liệu không?",
            buttons: [{ value: "Đồng ý" }, { value: "Hủy bỏ" }],
            success: function (result) {
                if (result === "Đồng ý") {
                    $('#processing').show();
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/Employee/DeleteIncurredSalary',
                        data: JSON.stringify({ id: id }),
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
                                        GridCallback('#grdMainIncurredSalary');
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    });
    // Medical
    $('#YearMedical').kendoDropDownList(
{
    dataTextField: "text",
    dataValueField: "value",
    dataSource: years,
    change: function () {
        GridCallback('#grdMainMedical');
    }
});
    $('#btnDeleteMedical').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainMedical');
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
                        url: '/hrm/Employee/DeleteInsuranceMedical',
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
                                        GridCallback('#grdMainMedical');
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    });
    $('#btnCreateMedical').bind("click", function () {
        var id = $('#EmployeeIdPost').val();
        if (id === '0') {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn cần phải đăng ký thông tin nhân viên trước!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Employee/InsuranceMedical', 600, 430, "Thêm mới thông tin bảo hiểm y tế");
    });
    $('#btnEditMedical').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainMedical');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitChildWindowModal('/hrm/Employee/InsuranceMedical?id=' + id, 600, 430, "Cập nhật thông tin bảo hiểm");
    });
    $("#grdMainMedical").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/Employee/InsuranceMedicals',
                        dataType: "json",
                        data: JSON.stringify({
                            year: $('#YearMedical').data('kendoDropDownList').value(),
                            employeeId :$('#EmployeeIdPost').val()
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
                    id: "InsuranceMedicalId",
                    fields: {
                        StartDate: { type: 'date' },
                        ExpiredDate: { type: 'date' },
                        Amount: { type: 'number' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "EmployeeCode",
                title: "Mã nhân viên",
                width: 150
            },
            {
                field: "FullName",
                title: "Tên nhân viên",
                width: 200
            },
            {
                field: "InsuranceMedicalNumber",
                title: "Mã BHYT",
                width: 150
            },
            {
                field: "StartDate",
                title: "Ngày bắt đầu",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "ExpiredDate",
                title: "Ngày hết hạn",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "Amount",
                title: "Mức đóng",
                width: 200,
                format: "{0:###,###}",
                template: "<a class='btn btn-primary btn-sm' href='javascript:void(0)'  onclick='OpenDetail(#=EmployeeId#)'>Chi Tiết</a>#=AmountString#"
            },
            {
                field: "CityId",
                title: "Tỉnh",
                width: 150,
                values: cities
            },
            {
                field: "MedicalId",
                title: "Nơi đăng ký KCB",
                width: 300,
                values: medicals
            },
             {
                 field: "Description",
                 title: "Ghi chú",
                 width: 300
             },
            {
                field: "CreateBy",
                title: "Người tạo",
                width: 150,
                values: users
            }
        ],
        dataBinding: function () {
        }
    });
    //Holiday
    $('#YearHoliday').kendoDropDownList({
        dataTextField:'text',
        dataValueField:'value',
        dataSource: years,
        change: function () {
            GridCallback('#grdMainHoliday');
        }
    });
    $("#grdMainHoliday").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/Employee/HolidayConfigs',
                        dataType: "json",
                        data: JSON.stringify({
                            year: $('#YearHoliday').data('kendoDropDownList').value(),
                            employeeId :$('#EmployeeIdPost').val()
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
                    id: "EmployeeId",
                    fields: {
                        EmployeeCode: { editable: false },
                        FullName: { editable: false },
                        DepartmentId: { editable: false },
                        Gender: { editable: false },
                        HolidayNumber: { editable: true, type: 'number', validation: { min: 0, required: true } }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 350,
        editable: true,
        filterable: true,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "HolidayNumber",
                title: "Số ngày nghỉ",
                width: 300
            }
        ],
        dataBinding: function () {
        },
        edit:function(e) {
            var input = e.container.find(".k-input");
            input.change(function () {
                var number = e.container.find("input[name='HolidayNumber']").val();
                if (number == null || number.trim() === "") {
                    return;
                }
                var rowSelected = GetGridRowSelected('#grdMainHoliday');
                if (rowSelected != null && rowSelected != typeof undefined) {
                    rowSelected.HolidayNumber = number;
                    $('#processing').show();
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/Employee/SaveHoliday',
                        data: JSON.stringify({
                            model:rowSelected
                        }),
                        contentType: 'application/json;charset=utf-8',
                        success: function (response) {
                            $('#processing').hide();
                            GridCallback('#grdMainHoliday');
                        }
                    });
                }
            });
        }
    });

});