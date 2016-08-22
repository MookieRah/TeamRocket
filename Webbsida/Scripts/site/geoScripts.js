var map;


function initMap(userLat, userLong) {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: userLat, lng: userLong },
        zoom: 8
    });
}

function getUserLocation() {

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(OnSuccess, OnError, {
            enableHighAccuracy: true,
            maximumAge: 5000,
            timeout: 1000
        });


    } else {
        document.getElementById("map").innerHTML = "GeoLocation not supported";
    }
}

function OnSuccess(position) {

    var userCurrentLat = position.coords.latitude;
    var userCurrentLong = position.coords.longitude;

    initMap(userCurrentLat, userCurrentLong);
    getEventsFromGeoController();
}

function OnError(error) {
    //var mapDiv = document.getElementById("map");
    //switch (error.code) {
    //    case error.PERMISSION_DENIED:
    //        mapDiv.innerHTML = "User denied the request for Geolocation.";
    //        break;
    //    case error.POSITION_UNAVAILABLE:
    //        mapDiv.innerHTML = "Location information is unavailable.";
    //        break;
    //    case error.TIMEOUT:
    //        mapDiv.innerHTML = "The request to get user location timed out.";
    //        break;
    //    case error.UNKNOWN_ERROR:
    //        mapDiv.innerHTML = "An unknown error occurred.";
    //        break;
    //}


    //Could not get user location, center map on nigeria
    initMap(10, 10);
    getEventsFromGeoController();
}

function getEventsFromGeoController() {
    //debugger;
    var allEvents;
    $.ajax({
        type: "GET",
        url: "Geo/GetEventsToJson",
        //data: { userId: Id },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {

            $.each(data, function (id, event) {
                addEventMarker(event.Name, event.Latitude, event.Longitude);
            });
        },
        error: function (response) {
            alert("Could not retrieve data from server!");
        }
    });

}

function addEventMarker(name, lat, long){

    var marker = new google.maps.Marker
    (
        {
            position: new google.maps.LatLng(lat, long),
            map: map,
            title: name
        }
    );

}