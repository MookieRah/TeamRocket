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



// GeoPicker
var userMarker = false; ////Has the user plotted their location marker? 
function geoPicker() {
    initGeoPickerMap(63.8, 20.3);

}

function initGeoPickerMap(userLat, userLong) {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: userLat, lng: userLong },
        zoom: 10
    });

    google.maps.event.addListener(map, 'click', function(event) {                
        //Get the location that the user clicked.
        var clickedLocation = event.latLng;
        //If the marker hasn't been added.
        if(marker === false){
            //Create the marker.
            marker = new google.maps.Marker({
                position: clickedLocation,
                map: map,
                draggable: true //make it draggable
            });
            //Listen for drag events!
            google.maps.event.addListener(marker, 'dragend', function(event){
                markerLocation();
            });
        } else{
            //Marker has already been added, so just change its location.
            marker.setPosition(clickedLocation);
        }
        //Get the marker's location.
        markerLocation();
    });
}

//This function will get the marker's current location and then add the lat/long
//values to our textfields so that we can save the location.
function markerLocation() {
    //Get location.
    var currentLocation = marker.getPosition();
    //Add lat and lng values to a field that we can save.
    document.getElementById('lat').value = currentLocation.lat(); //latitude
    document.getElementById('lng').value = currentLocation.lng(); //longitude
}