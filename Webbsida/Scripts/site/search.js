$(document).ready(function () {
    var url = "GetEventsBySearch";

    $.get(url, { query: null });
});

function LoadFromInput() {

    var table = $("#EventSummaryContainer");
    var raw = $("#autocomplete").val();
    var url = "GetEventsBySearch";

    if (raw != undefined) {
        table.load(url, { query: raw });
    }

    console.writeln(raw);
}