var record = 0;
$(document).ready(function () {
    $("#grdEmployees").kendoGrid({
        dataSource: {
            data: employees,
            schema: {
                model: {
                    id: "EmployeeId"

                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 300,
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
                width:  40
            },
            {
                field: "EmployeeCode",
                title: "Mã nhân viên",
                width: 100
            },
            {
                field: "FullName",
                title: "Họ và tên",
                width: 120
            },
            {
                field: "DepartmentId",
                title: "Phòng",
                width: 200,
                values: window.departments
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
    $("#grdEmployees[event-dbclick='1']").on("dblclick", "tr.k-state-selected", function () {
        $('#btnAddEmployee').click();
    });
    $('#btnAddEmployee').click(function () {
        var entityGrid = $("#grdEmployees").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null && selectedItem != typeof undefined) {
            $('#EmployeeId').val(selectedItem.EmployeeId);
            $('#EmployeeCode').val(selectedItem.EmployeeCode + " " + selectedItem.FullName);
            CloseChildWindowModal();
        } else {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn nhân viên!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
    });
});