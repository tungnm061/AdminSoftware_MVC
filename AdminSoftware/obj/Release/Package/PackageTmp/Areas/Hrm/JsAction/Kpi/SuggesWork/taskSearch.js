var record = 0;
$(document).ready(function () {
    $('#CategoryKpiId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: categoryKpis,
        optionLabel: "Chọn nhóm Kpi",
        filter: 'contains'
    });
    $("#grdTasks").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/SuggesWork/TaskResults',
                        dataType: "json",
                        data: JSON.stringify({
                            categoryKpiId: $('#CategoryKpiId').data('kendoDropDownList').value()
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
                    id: "TaskId",
                    fields: {
                        CreateDate: { type: 'date' },
                        IsSystem: { type: 'boolean' },
                        Frequent: { type: 'boolean' }
                    }
                }
            },
            pageSize: 1000,
            serverPaging: false,
            serverFiltering: false,
            group: { field: 'GroupName' }
        },
        height: 350,
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
                  field: "GroupName",
                  title: "Nhiệm vụ",
                  width: 100,
                  hidden: true
              },
              {
                  field: "TaskCode",
                  title: "Mã Công Việc",
                  width: 150
              },
            {
                field: "TaskName",
                title: "Tên công việc",
                width: 250
            },
               {
                   field: "UsefulHours",
                   title: "Giờ hữu ích",
                   width: 110
               },
            //{
            //    field: "WorkPointConfigId", title: "Điểm CV",
            //    width: 100,
            //    values: workPointConfigs
            //},
            {
                field: "Description",
                title: "Ghi Chú",
                width: 200
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        }
    });
    $("#grdTasks[event-dbclick='1']").on("dblclick", "tr.k-state-selected", function () {
        $('#btnAddTask').click();
    });
    $('#btnAddTask').click(function () {
        var entityGrid = $("#grdTasks").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null && selectedItem != typeof undefined) {
            $('#TaskId').val(selectedItem.TaskId);
            $('#TaskName').val(selectedItem.TaskName);
            CloseChildWindowModal();
            //GridCallback('#grdWorkPlanDetail');

        } else {
            $.msgBox({
                title: "Hệ thống",
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
