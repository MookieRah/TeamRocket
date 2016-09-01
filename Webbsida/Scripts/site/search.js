$(document).ready(function () {
    var url = "/Home/GetEventsBySearch";
    var table = $("#EventSummaryContainer");

    table.load("/Home/GetProcessing");

    table.load(url, { filter: "" });

    PopulateDropDown();
});

function PopulateDropDown() {
    var url = "/Home/GetTagsAndNamesBySearch";
    var table = $("#SearchDropDown");

    var options = {};
    options.url = url;
    options.type = "GET";
    options.data = { "filter": table.val() };
    options.dataType = "json";
    options.success = function(data) {
        table.empty();
        for (var i = 0; i < data.length; i++) {
            table.append("<option value='" +
                data[i] + "' label='" +
                data[i] + "'></option>");
        }
    };
    $.ajax(options);
};

function LoadFromInput() {
    var table = $("#EventSummaryContainer");
    var raw = $("#EventSearchBox").val();
    var url = "/Home/GetEventsBySearch";


    if (raw != undefined) {
        table.load(url, { filter: raw });
    }
}

function LoadOnEnter(e) {
    if (e.keyCode == 13) {
        LoadFromInput();
    }
}
