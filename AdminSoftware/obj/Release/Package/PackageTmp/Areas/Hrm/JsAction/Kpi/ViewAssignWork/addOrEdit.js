$(document).ready(function() {

    $("#Status").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: window.statusAssignWorks
    });
});
