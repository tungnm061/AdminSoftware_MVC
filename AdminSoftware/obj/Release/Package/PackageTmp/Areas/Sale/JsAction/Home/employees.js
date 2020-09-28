var record = 0;
$(document).ready(function () {
    $("#grdEmployees").kendoGrid({
        dataSource: {
            data: userSearch,
            schema: {
                model: {
                    id: "UserId"
  
                }
            },
            pageSize: 100000,
            serverPaging: false,
            serverFiltering: false
        },
        height: gridHeight,
        filterable: true,
        sortable: true,
        pageable:false,
        selectable: 'row',
        columns: [
            {
                field: "EmployeeCode",
                title: "Mã nhân viên"
            },
            {
                field: "FullName",
                title: "Họ và tên"
            },
            {
                field: "UserName",
                title: "Tên đăng nhập"
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
            $('#UserId').val(selectedItem.UserId);
            $('#UserName').val(selectedItem.UserName + "  " + selectedItem.FullName);
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