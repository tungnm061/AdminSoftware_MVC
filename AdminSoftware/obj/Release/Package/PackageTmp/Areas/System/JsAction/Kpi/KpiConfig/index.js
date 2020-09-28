var heightGridNew = "300px";
$(document).ready(function () {
    $("#MinHours").kendoNumericTextBox();
    $("#MaxHours").kendoNumericTextBox();
    $("#Notification").kendoNumericTextBox({
        format: "p0",
        min: 0,
        max: 1,
        step: 0.01
    });

    //CẤU HÌNH KPI
    $("#PlanningHourMin,#PlanningHourMax,#HourConfirmMin,#HourConfirmMax").kendoTimePicker({
        dateInput: true,
        format: '{0:HH:mm}'
    });

    $("#PlanningDay").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: dayNum
    });
    $('#btnSave').click(function () {

        var model = {
            KpiConfigId: kpiConfigId,
            MinHours: $('#MinHours').data('kendoNumericTextBox').value(),
            MaxHours: $('#MaxHours').data('kendoNumericTextBox').value(),
            PlanningHourMin: $('#PlanningHourMin').val(),
            PlanningHourMax: $('#PlanningHourMax').val(),
            PlanningDay: $('#PlanningDay').data('kendoDropDownList').value(),
            HourConfirmMax: $('#HourConfirmMax').val(),
            HourConfirmMin: $('#HourConfirmMin').val(),
            Notification: $('#Notification').data('kendoNumericTextBox').value()
        };

        if (model.Notification == null ) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn phòng chức năng !",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if ($('#HourConfirmMin').data('kendoTimePicker').value() == null || $('#HourConfirmMax').data('kendoTimePicker').value() == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn giờ xác nhận hoàn thành!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if ($('#HourConfirmMin').data('kendoTimePicker').value() > $('#HourConfirmMax').data('kendoTimePicker').value()) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Giờ xác nhận hoàn thành phải lớn hơn tối thiểu ",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.MinHours == null ) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập giờ hữu ích tối thiểu!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.PlanningDay == null || model.PlanningDay.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn ngày lập kế hoạch!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if ($('#PlanningHourMin').data('kendoTimePicker').value() == null || $('#PlanningHourMax').data('kendoTimePicker').value() == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn giờ lập kế hoạch!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if ($('#PlanningHourMin').data('kendoTimePicker').value() > $('#PlanningHourMax').data('kendoTimePicker').value() ) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Giờ lập kế hoạch phải lớn hơn tối thiểu!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.MaxHours == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập giờ hữu ích!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.MinHours > model.MaxHours) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Thời gian tối thiểu phải nhỏ hơn hoặc bằng thời gian!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/system/KpiConfig/Save',
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
                        if (result == 'Đồng ý' && response.Status === 1) {
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
    //GridmainJob
    //$("#grdMainJob").kendoGrid({
    //    dataSource: {
    //        transport: {
    //            read: '/system/KpiConfig/JobConfigs',
    //            dataType: "json"
    //        },
    //        schema: {
    //            model: {
    //                id: "JobConfigId",
    //                fields: {
    //                    JobType: { editable: false },
    //                    JobPointMin: { editable: true,type:'number' },
    //                    JobPointMax: { editable: true, type: 'number' },
    //                    JobConditionMin: { editable: true, type: 'number' },
    //                    JobConditionMax: { editable: true, type: 'number' }
    //                }
    //            }
    //        },
    //        pageSize: 1000,
    //        serverPaging: false,
    //        serverFiltering: false
    //    },
    //    height: heightGridNew,
    //    sortable: true,
    //    editable :true,
    //    pageable: false,
    //    selectable: 'row',
    //    columns: [
    //          {
    //              field: "JobType",
    //              title: "Mức độ",
    //              width: 80
    //          },

    //        {
    //            field: "JobPointMin",
    //            title: "Điểm tối thiểu",
    //            width: 125,
    //            hidden: true
    //        }, 
    //        {
    //            field: "JobPointMax",
    //            title: "Điểm",
    //            width: 125
    //        },
    //     {
    //        title: "Điều kiện đánh giá",
    //            columns: [{
    //               field: "JobConditionMin",
    //               title: "Điều kiện tối thiểu",
    //               width: 125
    //                  }, {
    //               field: "JobConditionMax",
    //               title: "Điều kiện",
    //               width: 125
    //                  }]
    //       }
    //    ],
    //    edit: function(e) {
    //        var input = e.container.find(".k-input");
    //        input.change(function() {
    //            var rowSelected = GetGridRowSelected('#grdMainJob');
    //            if (rowSelected != null && rowSelected != typeof undefined) {

    //                if (e.container.find("input[name='JobPointMax']").length > 0) {
    //                    rowSelected.JobPointMax = e.container.find("input[name='JobPointMax']").val();
    //                    rowSelected.JobPointMin = e.container.find("input[name='JobPointMax']").val();
    //                }
    //                if (e.container.find("input[name='JobConditionMin']").length > 0) {
    //                    rowSelected.JobConditionMin = e.container.find("input[name='JobConditionMin']").val();
    //                    if (rowSelected.JobConditionMax != null && rowSelected.JobConditionMax != typeof undefined && rowSelected.JobConditionMin > rowSelected.JobConditionMax) {
    //                        $.msgBox({
    //                            title: "Hệ thống",
    //                            type: "error",
    //                            content: "Điều kiện tối thiểu không được lớn hơn điều kiện !",
    //                            buttons: [{ value: "Đồng ý" }],
    //                            success: function () {
    //                            }
    //                        });
    //                        GridCallback('#grdMainJob');
    //                        return;
    //                    }
    //                }
    //                if (e.container.find("input[name='JobConditionMax']").length > 0) {
    //                    rowSelected.JobConditionMax = e.container.find("input[name='JobConditionMax']").val();
    //                    if (rowSelected.JobConditionMax < rowSelected.JobConditionMin && rowSelected.JobConditionMax.length >0) {
    //                        $.msgBox({
    //                            title: "Hệ thống",
    //                            type: "error",
    //                            content: "Điều kiện không được nhỏ hơn điều kiện tối thiểu !",
    //                            buttons: [{ value: "Đồng ý" }],
    //                            success: function () {
    //                            }
    //                        });

    //                        GridCallback('#grdMainJob');
    //                        return;
    //                    }
    //                }
    //                $('#processing').show();
    //                $.ajax({
    //                    type: 'POST',
    //                    url: '/system/KpiConfig/SaveJob',
    //                    data: JSON.stringify({
    //                        model: rowSelected
    //                    }),
    //                    contentType: 'application/json;charset=utf-8',
    //                    success: function (response) {
    //                        GridCallback('#grdMainJob');
    //                        $('#processing').hide();
    //                        var type;
    //                        if (response.Status === 1)
    //                            type = 'info';
    //                        else
    //                            type = 'error';
    //                        $.msgBox({
    //                            title: "Hệ thống",
    //                            type: type,
    //                            content: response.Message,
    //                            buttons: [{ value: "Đồng ý" }],
    //                            success: function (result) {
    //                            } 
    //                        });
    //                    },
    //                    error: function (response) {
    //                        $('#processing').hide();
    //                        $.msgBox({
    //                            title: "Hệ thống",
    //                            type: "error",
    //                            content: response.Msg,
    //                            buttons: [{ value: "Đồng ý" }]
    //                        });
    //                    }

    //                });
    //            }
    //        });
    //    }
    //    });
    // GridmainFinish
    //$("#grdMainFinish").kendoGrid({
    //    dataSource: {
    //        transport: {
    //            read: '/system/KpiConfig/FinishConfigs',
    //            dataType: "json"
    //        },
    //        schema: {
    //            model: {
    //                id: "FinishConfigId",
    //                fields: {
    //                    FinishType: { editable: false },
    //                    FinishPointMin: { editable: true, type: 'number' },
    //                    FinishPointMax: { editable: true, type: 'number' },
    //                    FinishConditionMin: { editable: true, type: 'number' },
    //                    FinishConditionMax: { editable: true, type: 'number' }
    //                }
    //            }
    //        },
    //        pageSize: 100,
    //        serverPaging: false,
    //        serverFiltering: false
    //    },
    //    height: heightGridNew,
    //    sortable: true,
    //    pageable: false,
    //    editable: true,
    //    selectable: 'row',
    //    columns: [
    //          {
    //              field: "FinishType",
    //              title: "Mức độ",
    //              width: 80
    //          },
    //        {
    //            field: "FinishPointMin",
    //            title: "Điểm tối thiểu",
    //            width: 125,
    //            hidden : true
    //        }, {
    //            field: "FinishPointMax",
    //            title: "Điểm",
    //            width: 125
    //    },
    //              {
    //        title: "Điều kiện đánh giá",
    //        columns: [{
    //            field: "FinishConditionMin",
    //            title: "Điều kiện tối thiểu",
    //            width: 125,
    //            format: "{0:p0}"
    //        }, {
    //            field: "FinishConditionMax",
    //            title: "Điều kiện",
    //            width: 125,
    //            format: "{0:p0}"
    //        }]
    //    }

    //    ],
    //    edit: function (e) {
    //        var input = e.container.find(".k-input");
    //        input.change(function () {
    //            var rowSelected = GetGridRowSelected('#grdMainFinish');
    //            if (rowSelected != null && rowSelected != typeof undefined) {
    //                if (e.container.find("input[name='FinishPointMax']").length > 0) {
    //                    rowSelected.FinishPointMax = e.container.find("input[name='FinishPointMax']").val();
    //                    rowSelected.FinishPointMin = e.container.find("input[name='FinishPointMax']").val();
    //                }
    //                if (e.container.find("input[name='FinishConditionMin']").length > 0) {
    //                    rowSelected.FinishConditionMin = e.container.find("input[name='FinishConditionMin']").val();
    //                    if (rowSelected.FinishConditionMax != null && rowSelected.FinishConditionMax != typeof undefined && rowSelected.FinishConditionMin > rowSelected.FinishConditionMax) {
    //                        $.msgBox({
    //                            title: "Hệ thống",
    //                            type: "error",
    //                            content: "Điều kiện tối thiểu không được lớn hơn điều kiện !",
    //                            buttons: [{ value: "Đồng ý" }],
    //                            success: function () {
    //                            }
    //                        });
    //                        GridCallback('#grdMainFinish');
    //                        return;
    //                    }
    //                }
    //                if (e.container.find("input[name='FinishConditionMax']").length > 0) {
    //                    rowSelected.FinishConditionMax = e.container.find("input[name='FinishConditionMax']").val();
    //                    if (rowSelected.FinishConditionMax < rowSelected.FinishConditionMin && rowSelected.FinishConditionMax.length > 0) {
    //                        $.msgBox({
    //                            title: "Hệ thống",
    //                            type: "error",
    //                            content: "Điều kiện không được nhỏ hơn điều kiện tối thiểu !",
    //                            buttons: [{ value: "Đồng ý" }],
    //                            success: function () {
    //                            }
    //                        });
    //                        GridCallback('#grdMainFinish');
    //                        return;
    //                    }
    //                }
    //                $('#processing').show();
    //                $.ajax({
    //                    type: 'POST',
    //                    url: '/system/KpiConfig/SaveFinish',
    //                    data: JSON.stringify({
    //                        model: rowSelected
    //                    }),
    //                    contentType: 'application/json;charset=utf-8',
    //                    success: function (response) {
    //                        GridCallback('#grdMainFinish');
    //                        $('#processing').hide();
    //                        var type;
    //                        if (response.Status === 1)
    //                            type = 'info';
    //                        else
    //                            type = 'error';
    //                        $.msgBox({
    //                            title: "Hệ thống",
    //                            type: type,
    //                            content: response.Message,
    //                            buttons: [{ value: "Đồng ý" }],
    //                            success: function (result) {
    //                            }
    //                        });
    //                    },
    //                    error: function (response) {
    //                        $('#processing').hide();
    //                        $.msgBox({
    //                            title: "Hệ thống",
    //                            type: "error",
    //                            content: response.Msg,
    //                            buttons: [{ value: "Đồng ý" }]
    //                        });
    //                    }
    //                });
    //            }
    //        });
    //    }
    //});
    // GridmainAccept
    //$("#grdMainAccept").kendoGrid({
    //    dataSource: {
    //        transport: {
    //            read: '/system/KpiConfig/AcceptConfigs',
    //            dataType: "json"
    //        },
    //        schema: {
    //            model: {
    //                id: "AcceptConfigId",
    //                fields: {
    //                    AcceptType: { editable: false },
    //                    AcceptPointMin: { editable: true,type:'number' },
    //                    AcceptPointMax: { editable: true, type: 'number' },
    //                    AcceptConditionMin: { editable: true, type: 'number' },
    //                    AcceptConditionMax: { editable: true, type: 'number' }
    //                }
    //            }
    //        },
    //        pageSize: 100,
    //        serverPaging: false,
    //        serverFiltering: false
    //    },
    //    height: heightGridNew,
    //    sortable: true,
    //    editable: true,
    //    pageable: false,
    //    selectable: 'row',
    //    columns: [
    //          {
    //              field: "AcceptType",
    //              title: "Mức độ",
    //              width: 80
    //          },
    //     {
    //           field: "AcceptPointMin",
    //           title: "Điểm tối thiểu",
    //           width: 125,
    //           hidden : true

    //       }, {
    //           field: "AcceptPointMax",
    //           title: "Điểm",
    //           width: 125
    //       },
    //     {
    //         title: "Điều kiện đánh giá",
    //         columns: [{
    //             field: "AcceptConditionMin",
    //             title: "Điều kiện tối thiểu",
    //             width: 125
    //         }, {
    //             field: "AcceptConditionMax",
    //             title: "Điều kiện",
    //             width: 125
    //         }]
    //     }
    //    ],
    //    edit: function(e) {
    //        var input = e.container.find(".k-input");
    //        input.change(function() {
    //            var rowSelected = GetGridRowSelected('#grdMainAccept');
    //            if (rowSelected != null && rowSelected != typeof undefined) {
  
    //                if (e.container.find("input[name='AcceptPointMax']").length > 0) {
    //                    rowSelected.AcceptPointMax = e.container.find("input[name='AcceptPointMax']").val();
    //                    rowSelected.AcceptPointMin = e.container.find("input[name='AcceptPointMax']").val();
    //                }
    //                if (e.container.find("input[name='AcceptConditionMin']").length > 0) {
    //                    rowSelected.AcceptConditionMin = e.container.find("input[name='AcceptConditionMin']").val();
    //                    if (rowSelected.AcceptConditionMax != null && rowSelected.AcceptConditionMax != typeof undefined && rowSelected.AcceptConditionMin > rowSelected.AcceptConditionMax) {
    //                        $.msgBox({
    //                            title: "Hệ thống",
    //                            type: "error",
    //                            content: "Điều kiện tối thiểu không được lớn hơn điều kiện !",
    //                            buttons: [{ value: "Đồng ý" }],
    //                            success: function () {
    //                            }
    //                        });
    //                        GridCallback('#grdMainAccept');
    //                        return;
    //                    }
    //                }
    //                if (e.container.find("input[name='AcceptConditionMax']").length > 0) {
    //                    rowSelected.AcceptConditionMax = e.container.find("input[name='AcceptConditionMax']").val();
    //                    if (rowSelected.AcceptConditionMax < rowSelected.AcceptConditionMin && rowSelected.AcceptConditionMax.length > 0) {
    //                        $.msgBox({
    //                            title: "Hệ thống",
    //                            type: "error",
    //                            content: "Điều kiện không được nhỏ hơn điều kiện tối thiểu !",
    //                            buttons: [{ value: "Đồng ý" }],
    //                            success: function () {
    //                            }
    //                        });
    //                        GridCallback('#grdMainAccept');
    //                        return;
    //                    }
    //                }
    //                $('#processing').show();
    //                $.ajax({
    //                    type: 'POST',
    //                    url: '/system/KpiConfig/SaveAccept',
    //                    data: JSON.stringify({
    //                        model: rowSelected
    //                    }),
    //                    contentType: 'application/json;charset=utf-8',
    //                    success: function (response) {
    //                        GridCallback('#grdMainAccept');
    //                        $('#processing').hide();
    //                        var type;
    //                        if (response.Status === 1)
    //                            type = 'info';
    //                        else
    //                            type = 'error';
    //                        $.msgBox({
    //                            title: "Hệ thống",
    //                            type: type,
    //                            content: response.Message,
    //                            buttons: [{ value: "Đồng ý" }],
    //                            success: function (result) {
    //                            }
    //                        });
    //                    },
    //                    error: function (response) {
    //                        $('#processing').hide();
    //                        $.msgBox({
    //                            title: "Hệ thống",
    //                            type: "error",
    //                            content: response.Msg,
    //                            buttons: [{ value: "Đồng ý" }]
    //                        });
    //                    }
    //                });
    //            }
    //        });
    //    }
    //});
    //GridmainFactor
    $("#grdMainFactor").kendoGrid({
        dataSource: {
            transport: {
                read: '/system/KpiConfig/FactorConfigs',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "FactorConfigId",
                    fields: {
                        FactorType: { editable: false },
                        FactorPointMin: { editable: true, type: 'number' },
                        FactorPointMax: { editable: true, type: 'number' },
                        FactorConditionMin: { editable: true, type: 'number' },
                        FactorConditionMax: { editable: true, type: 'number' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: heightGridNew,
        sortable: true,
        editable: true,
        pageable: false,
        selectable: 'row',
        columns: [
              {
                  field: "FactorType",
                  title: "Mức độ",
                  width: 80
              },
     {

             field: "FactorPointMin",
             title: "Điểm tối thiểu",
             width: 125,
             hidden : true
         }, {
             field: "FactorPointMax",
             title: "Điểm",
             width: 125

     },
         {
             title: "Điều kiện đánh giá",
             columns: [{
                 field: "FactorConditionMin",
                 title: "Điều kiện tối thiểu",
                 width: 125,
                 format: "{0:p0}"
             }, {
                 field: "FactorConditionMax",
                 title: "Điều kiện tối đa",
                 width: 125,
                 format: "{0:p0}"
             }]
         }
        ],
        edit: function (e) {
            var input = e.container.find(".k-input");
            input.change(function () {
                var rowSelected = GetGridRowSelected('#grdMainFactor');
                if (rowSelected != null && rowSelected != typeof undefined) {
               
                    if (e.container.find("input[name='FactorPointMax']").length > 0) {
                        rowSelected.FactorPointMax = e.container.find("input[name='FactorPointMax']").val();
                        rowSelected.FactorPointMin = e.container.find("input[name='FactorPointMax']").val();
                    }
                    if (e.container.find("input[name='FactorConditionMin']").length > 0) {
                        rowSelected.FactorConditionMin = e.container.find("input[name='FactorConditionMin']").val();
                        if (rowSelected.FactorConditionMax != null && rowSelected.FactorConditionMax != typeof undefined && rowSelected.FactorConditionMin > rowSelected.FactorConditionMax) {
                            $.msgBox({
                                title: "Hệ thống",
                                type: "error",
                                content: "Điều kiện tối thiểu không được lớn hơn điều kiện  !",
                                buttons: [{ value: "Đồng ý" }],
                                success: function () {
                                }
                            });
                            GridCallback('#grdMainFactor');
                            return;
                        }
                    }
                    if (e.container.find("input[name='FactorConditionMax']").length > 0) {
                        rowSelected.FactorConditionMax = e.container.find("input[name='FactorConditionMax']").val();
                        if (rowSelected.FactorConditionMax < rowSelected.FactorConditionMin && rowSelected.FactorConditionMax.length > 0) {
                            $.msgBox({
                                title: "Hệ thống",
                                type: "error",
                                content: "Điều kiện không được nhỏ hơn điều kiện tối thiểu  !",
                                buttons: [{ value: "Đồng ý" }],
                                success: function () {
                                }
                            });
                            GridCallback('#grdMainFactor');
                            return;
                        }
                    }
                    $('#processing').show();
                    $.ajax({
                        type: 'POST',
                        url: '/system/KpiConfig/SaveFactor',
                        data: JSON.stringify({
                            model: rowSelected
                        }),
                        contentType: 'application/json;charset=utf-8',
                        success: function (response) {
                            GridCallback('#grdMainFactor');
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
                                }
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
            });
        }
    });
});