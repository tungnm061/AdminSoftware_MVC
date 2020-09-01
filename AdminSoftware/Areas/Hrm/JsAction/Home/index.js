$(document).ready(function () {
    $(document).on("change", "input[type='radio'][name=status]", function () {
        var action = this.value;
        $("#CheckAction").val(action);
        var scheduler = $("#scdMain").data("kendoScheduler");
        var dataSource = new kendo.data.SchedulerDataSource({
            batch: true,
            transport: {
                read: {
                    url: "/hrm/Home/WorkDetailSchedulers?month=" + (new Date().getMonth() + 1) + "&year=" + (new Date().getFullYear()) + "&viewStyle=" + $("#CheckAction").val(),
                    dataType: "json"
                }
            },
            schema: {
                model: {
                    id: "DateId",
                    fields: {
                        taskID: { from: "DateId" },
                        title: { from: "Title" },
                        start: { type: "date", from: "DateDay" },
                        end: { type: "date", from: "EndDate" }
                    }
                }
            }
        });
        scheduler.setDataSource(dataSource);
        scheduler.refresh();
    });
    $("#grdMainHaiLong").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/Home/Complains',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "ComplainId",
                    fields: {
                        CreateDate: { type: 'date' },
                        Status: { type: 'number' },
                        ConfirmedDate: { type: 'date' }
                    }
                }
            },
            pageSize: 10000,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
              {
                  field: "Description",
                  title: "Hành vi",
                  width: 300
              },
              {
                  field: "ConfirmedDate",
                  title: "Ngày xác nhận",
                  format: "{0:dd/MM/yyyy}",
                  width: 120
              }

        ],
        dataBinding: function () {
        }
    });
    $("#grdMainTheoDoi").kendoGrid({
        dataSource: {
            transport: {
                read: '/accounting/Home/AssignWorkFollowBys',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "AssignWorkId",
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
        height: 300,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [

              {
                  field: "CreateByName",
                  title: "Người giao ",
                  width: 110
              },
               {
                   field: "AssignName",
                   title: "Người thực hiện ",
                   width: 130
               },
              {
                  field: "TaskName",
                  title: "Tên CV",
                  width: 280
              },
               {
                   field: "Quantity",
                   title: "SL",
                   width: 50
               },
            {
                field: "FromDate",
                title: "Ngày bắt đầu",
                width: 140,
                format: "{0:dd/MM/yyyy}"
            },
              {
                  field: "ToDate",
                  title: "Ngày kết thúc",
                  width: 140,
                  format: "{0:dd/MM/yyyy}"
              }

        ],
        dataBinding: function () {
        }
    });
    $("#grdMainDeXuat").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/Home/SuggesWorks',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "WorkDetailId",
                    fields: {
                        ToDate: { type: 'date' },
                        FromDate: { type: 'date' }
                    }
                }
            },
            pageSize: 10000,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [

              {
                  field: "TaskName",
                  title: "Tên Công việc",
                  width: 300
              },
            {
                field: "DepartmentId",
                title: "Phòng chức năng",
                width: 200,
                values: departments,
                hidden: true
            },
            {
                field: "Quantity",
                title: "Số lượng",
                width: 80
            },
            {
                field: "FromDate",
                title: "Ngày bắt đầu",
                format: "{0:dd/MM/yyyy}",
                width: 120

            },
            {
                field: "ToDate",
                title: "Ngày kết thúc",
                format: "{0:dd/MM/yyyy}",
                width: 120
            }
        ],
        dataBinding: function () {
        }
    });
    $('#btnDeXuat').click(function () {
        var grid = $("#grdMainDeXuat").data("kendoGrid");
        var selectedItem = grid.dataItem(grid.select());
        var id = selectedItem.WorkDetailId;
        var workType = selectedItem.WorkType;
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
        InitWindowModal('/hrm/Home/EditWork?id=' + id + '&workType=' + workType, false, 800, 550, 'Thực hiện công việc', false);
    });
    $("#grdMainGiaoViec").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/Home/AssignWorkCreateBys',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "AssignWorkId",
                    fields: {
                        CreateDate: { type: 'date' }
                    }
                }
            },
            pageSize: 10000,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
             {
                 field: "StatusDisplay",
                 title: "Trạng Thái",
                 width: 100,
                 values: workDetailStatusEnum,
                 attributes: {
                     style: "#=Color#"
                 }
             },
              {
                  field: "AssignName",
                  title: "NV được giao ",
                  width: 110
              },
              {
                  field: "TaskName",
                  title: "Tên CV",
                  width: 280
              },
               {
                   field: "Quantity",
                   title: "SL",
                   width: 50
               },
            {
                field: "CreateDate",
                title: "Ngày giao",
                width: 100,
                format: "{0:dd/MM/yyyy}"
            }

        ],
        dataBinding: function () {
        }
    });
    $("#grdMainDuocGiao").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/Home/AssignWorkAssignBys',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "AssignWorkId",
                    fields: {
                        CreateDate: { type: 'date' }
                    }
                }
            },
            pageSize: 10000,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [

              {
                  field: "CreateByName",
                  title: "Người giao ",
                  width: 110
              },
              {
                  field: "TaskName",
                  title: "Tên CV",
                  width: 280
              },
               {
                   field: "Quantity",
                   title: "SL",
                   width: 50
               },
            {
                field: "CreateDate",
                title: "Ngày giao",
                width: 100,
                format: "{0:dd/MM/yyyy}"
            }

        ],
        dataBinding: function () {
        }
    });
    $("#grdMainQuaHan").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/Home/WorkOutDates',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "WorkDetailId",
                    fields: {
                        ToDate: { type: 'date' }
                    }
                }
            },
            pageSize: 10000,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [

           {
               field: "TaskName",
               title: "Tên CV",
               width: 280
           },
            {
                field: "Quantity",
                title: "SL",
                width: 50
            },
            {
                field: "ToDate",
                title: "Ngày hoàn thành",
                format: "{0:dd/MM/yyyy}",
                width: 110
            }
        ],
        dataBinding: function () {
        }
    });
    $("#grdMainDangTh").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/Home/WorkNeedCompletes',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "WorkDetailId",
                    fields: {
                        ToDate: { type: 'date' }
                    }
                }
            },
            pageSize: 10000,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
           {
               field: "TaskName",
               title: "Tên CV",
               width: 280
           },
            {
                field: "Quantity",
                title: "SL",
                width: 50
            },
            {
                field: "ToDate",
                title: "Ngày hoàn thành",
                format: "{0:dd/MM/yyyy}",
                width: 110
            }
        ],
        dataBinding: function () {
        }
    });
    $('#btnCapNhatAssignWork').click(function () {
        var grid = $("#grdMainDuocGiao").data("kendoGrid");
        var selectedItem = grid.dataItem(grid.select());
        var id = selectedItem.AssignWorkId;
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
        InitWindowModal('/hrm/Home/EditWork?id=' + id + '&workType=' + 4, false, 800, 550, 'Thực hiện công việc', false);
    });
    $('#btnThucHienCv').click(function () {
        var grid = $("#grdMainDangTh").data("kendoGrid");
        var selectedItem = grid.dataItem(grid.select());
        var id = selectedItem.WorkDetailId;
        var workType = selectedItem.WorkType;
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
        InitWindowModal('/hrm/Home/EditWork?id=' + id + '&workType=' + workType, false, 800, 550, 'Thực hiện công việc', false);
    });
    $('#btnGiaiTrinh').click(function ()     {
        var grid = $("#grdMainQuaHan").data("kendoGrid");
        var selectedItem = grid.dataItem(grid.select());
        var id = selectedItem.WorkDetailId;
        var workType = selectedItem.WorkType;
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
        InitWindowModal('/hrm/Home/WorkDetailExplanation?id=' + id + '&workType=' + workType, false, 600, 300, 'Giải trình công việc');
    });
    $('#btnGiaoViec').click(function () {
        InitWindowModal('/hrm/Home/AssignWork', false, 680, 370, 'Giao việc',false);
    });
});
