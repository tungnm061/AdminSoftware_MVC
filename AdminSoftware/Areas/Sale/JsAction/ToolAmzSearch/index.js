var record = 0;

function Search() {
    var keySearch = $("#TextSearch").val();
    var page = $("#Page").data("kendoNumericTextBox").value();
    var newDataSource = new kendo.data.DataSource({
        transport: {
            read: function (options) {
                $.ajax(
                    {
                        type: 'POST',
                        url: '/sale/ToolAmzSearch/GetDataSearch',
                        dataType: "json",
                        data: JSON.stringify({
                            keySearch: keySearch,
                            page : page
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
                id: "Id"
            }
        },
        pageSize: 9999,
        serverPaging: false,
        serverFiltering: false
    });
    var grid = $("#grdMain").data("kendoGrid");
    grid.setDataSource(newDataSource);
}

$(document).ready(function () {
    $("#Page").kendoNumericTextBox({
        format: "{0:n0}",
        min: 1,
        step: 1
    });

    $("#btnSearchDate").click(function () {
        Search()
    });

    $("#btnSearchDateLeft").click(function () {
        var page = $("#Page").data("kendoNumericTextBox").value();

        if (page == 1 || page == "1") {
            return;
        }
        $("#Page").data("kendoNumericTextBox").value(page - 1);
        Search()
    });

    $("#btnSearchDateRight").click(function () {
        var page = $("#Page").data("kendoNumericTextBox").value();
        $("#Page").data("kendoNumericTextBox").value(page + 1);
        Search()
    });


    $("#grdMain").kendoGrid({
        toolbar: [],
        excel: {
            fileName: "AmazoneRanking.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                        {
                            type: 'POST',
                            url: '/sale/ToolAmzSearch/GetDataSearch',
                            dataType: "json",
                            data: JSON.stringify({
                                keySearch : $("#TextSearch").val(),
                                page: $("#Page").data("kendoNumericTextBox").value()
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
                    id: "Id"
                }
            },
            pageSize: 9999,
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
                field: "Title",
                title: "Tiêu đề sản phẩm",
                width: 200
            },
            {
                field: "UrlImage",
                title: "Hình ảnh sản phẩm",
                width: 500,
                template: "#if(UrlImage != null){#" + "<a width='150' height='150' href='#=UrlImage#' target='_blank' style='text-align:center'><img width='150' height='150' src='#=UrlImage #' alt='image' /></a>" + "#}#",

            }
            //template: "#if(FilePath != null){#" + "<a href='#=FilePath#' target='_blank' style='text-align:center'><i class='glyphicon glyphicon-download'></i></a>" + "#}#",

        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});