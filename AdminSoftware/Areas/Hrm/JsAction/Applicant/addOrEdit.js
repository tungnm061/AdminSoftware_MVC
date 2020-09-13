$(document).ready(function () {
    $('#Sex').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: sexs,
        filter:'startswith'
    });
    $('#RecruitPlanId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: recruitPlans,
        optionLabel: 'Chọn kế hoạch',
        filter: 'startswith'
    });
    $("#ChanelId").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: chanels,
        filter: 'startswith'
    });
    $('#CityBirthPlace').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: cities,
        filter: 'startswith'
    });
    $('#TrainingLevelId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: trainingLevels,
        filter: 'startswith'
    });
    $('#CountryId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: countries,
        optionLabel: 'chưa xác định',
        filter: 'startswith'
    });
    $('#NationId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: nations,
        optionLabel: 'chưa xác định',
        filter: 'startswith'
    });
    $('#ReligionId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: religions,
        optionLabel: 'chưa xác định',
        filter: 'startswith'
    });
    $('#DateOfBirth,#CvDate').kendoDatePicker({
        format: '{0:dd/MM/yyyy}',
        dateInput:true
    });
    $('#btnSave').click(function () {
        var model = {
            ApplicantId: applicantId,
            FullName: $('#FullName').val(),
            Sex: $('#Sex').data('kendoDropDownList').value(),
            DateOfBirth: $('#DateOfBirth').data('kendoDatePicker').value(),
            CountryId: $('#CountryId').data('kendoDropDownList').value(),
            NationId: $('#NationId').data('kendoDropDownList').value(),
            ReligionId: $('#ReligionId').data('kendoDropDownList').value(),
            CityBirthPlace: $('#CityBirthPlace').data('kendoDropDownList').value(),
            PermanentAddress: $('#PermanentAddress').val(),
            IdentityCardNumber: $('#IdentityCardNumber').val(),
            PhoneNumber: $('#PhoneNumber').val(),
            Email: $('#Email').val(),
            ChanelId: $('#ChanelId').data('kendoDropDownList').value(),
            TrainingLevelId: $('#TrainingLevelId').data('kendoDropDownList').value(),
            RecruitPlanId: $('#RecruitPlanId').data('kendoDropDownList').value(),
            CvDate: $('#CvDate').data('kendoDatePicker').value(),
            Description: $('#Description').val()
        }
        if (model.FullName == null || model.FullName.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập họ tên!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.Sex == null || model.Sex.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn giới tính!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.DateOfBirth == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập ngày sinh!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.CityBirthPlace == null || model.CityBirthPlace.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn nơi sinh!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.ChanelId == null || model.ChanelId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn kênh tuyển dụng!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.PhoneNumber == null || model.PhoneNumber.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập số điện thoại!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.TrainingLevelId == null || model.TrainingLevelId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn trình độ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.RecruitPlanId == null || model.RecruitPlanId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn kế hoạch tuyển dụng!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.CvDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập ngày nộp hồ sơ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.PermanentAddress == null || model.PermanentAddress.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập địa chỉ ứng viên!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/Applicant/Save',
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
});