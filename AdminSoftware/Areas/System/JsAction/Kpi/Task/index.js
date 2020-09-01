var record = 0;
$(document).ready(function () {
    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "DanhSachCongViec.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: '/system/Task/Tasks',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "TaskId",
                    fields: {
                        CreateDate: { type: 'date' },
                        IsSystem: { type: 'boolean' },
                        Frequent: { type: 'boolean' }
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
                  field: "TaskCode",
                  title: "Mã công việc",
                  width: 140
              },
            {
                field: "TaskName",
                title: "Tên công việc",
                width: 250
            },
            {
                field: "CategoryKpiId",
                title: "Kpi",
                width: 250,
                values: categoryKpis
            },
            {
                field: "UsefulHours",
                title: "Giờ hữu ích",
                width: 120
            },
            //{
            //    field: "WorkPointConfigId",
            //    title: "Điểm CV",
            //    width: 120,
            //    values : workPointConfigs
            //},
            {
                field: "CalcType",
                title: "Quy cách",
                width: 150,
                values: calcType1
            },
            {
                field: "Frequent",
                title: "Thường Xuyên",
                width: 140,
                template: '<div class="text-center"><i class="#= Frequent ? "glyphicon glyphicon-ok" : "glyphicon glyphicon-ban-circle" #"></i></div>'

            },
            {
                field: "Description",
                title: "Ghi Chú",
                width: 200
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                format: "{0:dd/MM/yyyy}",
                width: 120
            },
            {
                field: "CreateBy",
                title: "Người Tạo",
                width: 120,
                values: employees

            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

    $('#btnCreate').click(function () {
        InitWindowModal('/system/Task/Task', false, 600, 455, 'Thêm mới công việc', false);
    });
    $('#btnExcel').click(function () {
        InitWindowModal('/system/Task/TaskExcel', false, 550, 220, 'Import file Excel', false);
    });
    $('#btnEdit').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
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
        InitWindowModal('/system/Task/Task?id=' + id, false, 600, 455, 'Cập nhật công việc', false);
    });

    $('#btnDelete').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn dữ liệu cần xóa!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
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
                        url: '/system/Task/Delete',
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
});