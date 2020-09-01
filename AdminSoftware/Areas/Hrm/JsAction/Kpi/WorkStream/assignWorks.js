var record = 0;
$(document).ready(function () {
    $("#grdAssignWorks").kendoGrid({
        dataSource: {
            data: tasks,
            schema: {
                model: {
                    id: "TaskId",
                    fields: {
                        CreateDate: { type: 'date' }
                    }
                }
            },
            pageSize: 10000,
            serverPaging: false,
            serverFiltering: false
        },
        height: 250,
        filterable: true,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                title: "STT",
                template: "#= ++record #",
                width: 60
            },
              {
                  field: "TaskCode",
                  title: "Mã Công Việc",
                  width: 130
              },
            {
                field: "TaskName",
                title: "Tên công việc",
                width: 250
            },
            {
                field: "Description",
                title: "Mô tả công việc",
                width: 200
            }

        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
    $("#grdAssignWorks[event-dbclick='1']").on("dblclick", "tr.k-state-selected", function () {
        $('#btnAddAssignWork').click();
    });
    $('#btnAddAssignWork').click(function () {
        var entityGrid = $("#grdAssignWorks").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null && selectedItem != typeof undefined) {
            $('#AssignWorkId').val(selectedItem.AssignWorkId);
            $('#TaskName').val(selectedItem.TaskName);
            CloseChildWindowModal();
            //GridCallback('#grdWorkPlanDetail');

        } else {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn công việc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
    });
});