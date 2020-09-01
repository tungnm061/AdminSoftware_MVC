var record = 0;
$(document).ready(function () {
    $("#Checkin,#Checkout").kendoTimePicker({
        dateInput: true,
        format: '{0:HH:mm}'
    });
    $("#btnSearchDate").click(function () {           
        GridCallback("#grdMain");
    });
    $('#btnImport').click(function() {
        InitWindowModal('/hrm/TimeSheet/Import', false, 450, 200, "Import chấm công", false);
    });
    $("#DateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy",
        max: new Date()
    });
    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/TimeSheet/TimeSheets',
                        dataType: "json",
                        data: JSON.stringify({
                            timeSheetDate: $("#DateSearch").data("kendoDatePicker").value()
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
                    id: "TimeSheetId",
                    fields: {
                        EmployeeCode: { editable: false },
                        FullName: { editable: false },
                        Checkin: { editable: true, type: 'Date' },
                        Checkout: { editable: true, type: 'Date' },
                        TimeSheetDate: { editable: false, type: 'Date' },
                        RealDays: { editable: false, type: 'number' },
                        LateMinutes: { editable: false, type: 'number' },
                        EarlyMinutes: { editable: false, type: 'number' }
                    }
                }
            },
            pageSize: 1000,
            serverPaging: false,
            serverFiltering: false,
            group: { field: 'DepartmentName' }
        },
        height: gridHeight,
        sortable: true,
        editable: true,
        pageable: false,
        selectable: 'row',
        columns: [
             {
                 title: "STT",
                 template: "#= ++record #",
                 width: 60
             },
              {
                  field: "DepartmentName",
                  title: "Phòng Ban",
                  width: 200,
                  hidden: true
              },
             {
                 field: "TimeSheetDate",
                 title: "Ngày",
                 width: 120,
                 format: "{0:dd/MM/yyyy}",        
                 hidden: true
             },
              {
                  field: "EmployeeId",
                  title: "Mã nhân viên",
                  width: 120,
                  hidden: true
              },
              {
                  field: "EmployeeCode",
                  title: "Mã nhân viên",
                  width: 120
              },
              {
                  field: "FullName",
                  title: "Tên nhân viên",
                  width: 120
              },

            {
                field: "Checkin",
                title: "Giờ đến",
                format: "{0:HH:mm}",
                width: 125,
                editor: function(container, options){
                    var input = $("<input/>"); 
                    input.attr("name",options.field); 
                              
                    input.appendTo(container); 
                              
                    input.kendoTimePicker({});
                }
                
            },
            {
                field: "Checkout",
                title: "Giờ nghỉ",
                format: "{0:HH:mm}",
                width: 125,
                editor: function (container, options) {
                    var input = $("<input/>");
                    input.attr("name", options.field);

                    input.appendTo(container);

                    input.kendoTimePicker({});
                }
            },
             {
                 field: "RealDays",
                 title: "Chấm công",
                 width: 125
             },
              {
                  field: "LateMinutes",
                  title: "Đến muộn(Phút)",
                  width: 125
              },
               {
                   field: "EarlyMinutes",
                   title: "Nghỉ sớm(Phút)",
                   width: 125
               }

        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        edit: function (e) {
            var input = e.container.find(".k-input");
            input.change(function () {
                var rowSelected = GetGridRowSelected('#grdMain');
                if (rowSelected != null && rowSelected != typeof undefined) {

                    if (e.container.find("input[name='Checkin']").length > 0) {
                        rowSelected.Checkin = e.container.find("input[name='Checkin']").val();
                    }
                    if (e.container.find("input[name='Checkout']").length > 0) {
                        rowSelected.Checkout = e.container.find("input[name='Checkout']").val();
                        if (rowSelected.Checkin > rowSelected.Checkout) {
                            $.msgBox({
                                title: "Hệ thống",
                                type: "error",
                                content: "Giờ đến không được lớn hơn giờ nghỉ!",
                                buttons: [{ value: "Đồng ý" }],
                                success: function () {
                                }
                            });
                            GridCallback('#grdMain');
                            return;
                        }
                    }
                   
                    $('#processing').show();
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/TimeSheet/Save',
                        data: JSON.stringify({
                            model: rowSelected
                        }),
                        contentType: 'application/json;charset=utf-8',
                        success: function (response) {
                            GridCallback('#grdMain');
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