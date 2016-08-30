$(document).ready(function () {
    var url = "/Home/GetEventsBySearch";
    var table = $("#EventSummaryContainer");

    table.load(url, { filter: "" });
});

function PopulateDropDown() {
    
}

function LoadFromInput() {

    var table = $("#EventSummaryContainer");
    var raw = $("#EventSearchBox").val();
    var url = "/Home/GetEventsBySearch";

    console.log(raw);

    if (raw != undefined) {
        table.load(url, { filter: raw });
    }
}

//function LoadFromInput() {

//    var table = $("#EventSummaryContainer");
//    var url = "/Home/GetEventsBySearch";

//    table.load(url);
//}