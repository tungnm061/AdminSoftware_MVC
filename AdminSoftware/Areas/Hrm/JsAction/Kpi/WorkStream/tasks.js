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
                        url: '/hrm/WorkStream/TaskResults',
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
                  width: 130
              },
            {
                field: "TaskName",
                title: "Tên công việc",
                width: 250
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
            //    values: workPointConfigs
            //},
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
    $("#grdTasks[event-dbclick='1']").on("dblclick", "tr.k-state-selected", function () {
        $('#btnAddTask').click();
    });
    $('#btnAddTask').click(function () {
        var entityGrid = $("#grdTasks").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null && selectedItem != typeof undefined) {
            if ($("#CheckTaskSearch").val() === "1") {
                $('#TaskId').val(selectedItem.TaskId);
                $('#TaskName').val(selectedItem.TaskName);
            }
            if ($("#CheckTaskSearch").val() === "2") {
                $('#TaskName2').val(selectedItem.TaskName);
                $('#TaskId2').val(selectedItem.TaskId);
                $('#TaskNameModel').val(selectedItem.TaskName);
                $('#TaskCodeModel').val(selectedItem.TaskCode);
            }

            CloseChildOfChildWindowModal();
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
