$(document).ready(function () {
    var url = "Home/GetEventsBySearch";

    $.get(url, { query: null });
});

function LoadFromInput() {

    var table = $("#EventSummaryContainer");
    var raw = $("#EventSearch").val();
    var url = "Home/GetEventsBySearch";

    console.log(raw);

    if (raw != undefined) {
        table.load(url, { query: raw });
    }
}