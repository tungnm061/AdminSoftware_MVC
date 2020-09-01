
$(document).ready(function () {
    $("#UsefulHours,#UsefulHoursTask").kendoNumericTextBox({
        format: "{0:###,###.##}"
    });
    $("#grdMainExplanation").kendoGrid({
        dataSource: {
            data: window.explanations,
            schema: {
                model: {
                    id: "CreateDate",
                    fields: {
                        CreateDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 150,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "CreateDate",
                title: "Thời gian",
                format: "{0:dd/MM/yyyy hh:mm:ss tt}",
                width: 140
            },
            {
                field: "ExplanationText",
                title: "Giải trình",
                width: 500
            }

        ]
    });
    $("#grdMainWorkingNote").kendoGrid({
        dataSource: {
            data: window.workingNotes,
            schema: {
                model: {
                    id: "WorkingNoteId",
                    fields: {
                        CreateDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 150,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "CreateDate",
                title: "Thời gian",
                format: "{0:dd/MM/yyyy hh:mm:ss tt}",
                width: 140
            },
            {
                field: "TextNote",
                title: "Công việc thực hiện",
                width: 500
            }

        ]
    });
 

});