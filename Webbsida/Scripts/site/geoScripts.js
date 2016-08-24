var map;
var geocoder;
//var userCoordinates;

function initMap(userLat, userLong) {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: userLat, lng: userLong },
        zoom: 8
    });

    getEventsFromGeoController();
}

function getUserLocation() {

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(OnSuccess, OnError, {
            //Default values below:
            enableHighAccuracy: false,
            timeout: Infinity,
            maximumAge: 0
        });


    } else {
        document.getElementById("map").innerHTML = "GeoLocation not supported";
    }
}

function OnSuccess(position) {

    var userCurrentLat = position.coords.latitude;
    var userCurrentLong = position.coords.longitude;

    //userCoordinates = position.coords;

    initMap(userCurrentLat, userCurrentLong);
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
}

function getEventsFromGeoController() {
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

function addEventMarker(name, lat, long) {

    var marker = new google.maps.Marker
    (
        {
            position: new google.maps.LatLng(lat, long),
            map: map,
            title: name
        }
    );

}



//
// -> GeoPicker
//
var userMarker = false; ////Has the user plotted their location marker? 
function geoPicker() {
    initGeoPickerMap(63.8, 20.3);
}

function initGeoPickerMap(userLat, userLong) {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: userLat, lng: userLong },
        zoom: 10
    });

    google.maps.event.addListener(map, "click", function (event) {
        //Get the location that the user clicked.
        var clickedLocation = event.latLng;
        //If the marker hasn"t been added.
        if (userMarker === false) {
            //Create the marker.
            userMarker = new google.maps.Marker({
                position: clickedLocation,
                map: map,
                draggable: true //make it draggable
            });
            //Listen for drag events!
            google.maps.event.addListener(userMarker, "dragend", function (event) {
                markerLocation();
            });
        } else {
            //Marker has already been added, so just change its location.
            userMarker.setPosition(clickedLocation);
        }
        //Get the marker"s location.
        markerLocation();
    });
}

//This function will get the marker"s current location and then add the lat/long
//values to our textfields so that we can save the location.
function markerLocation() {
    //Get location.
    var currentLocation = userMarker.getPosition();
    //Add lat and lng values to a field that we can save.
    document.getElementById("lat").value = currentLocation.lat(); //latitude
    document.getElementById("lng").value = currentLocation.lng(); //longitude
}


// Request coordinates from address in a form-field
// The insane way (extra roundtrip to the server)
function requestCoordinatesFromAddress() {
    var getAddressFormValue = $("#input_address").val();

    $.get("RequestCoordinatesFromAddress", "address=" + getAddressFormValue, function (data) {
        alert(data.Latitude + ":" + data.Longitude);
    });

}


// Make the marker move to the address inputted.
// (The sane way)
function setMarkerByUserInputAddress() {

    if (geocoder == null) {
        geocoder = new google.maps.Geocoder();
    }
    geocoder.geocode({ "address": $("#input_address").val() }, function (results, status) {
        if (status === google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);

            if (userMarker === false) {
                //Create the marker.
                userMarker = new google.maps.Marker({
                    position: results[0].geometry.location,
                    map: map,
                    draggable: true //make it draggable
                });
                //Listen for drag events!
                google.maps.event.addListener(userMarker, "dragend", function(event) {
                    markerLocation();
                });
            } else {
                userMarker.setPosition(results[0].geometry.location);
            }

            markerLocation();
        }
        else {
            alert("Geocode was not successful for the following reason: " + status);
        }
    });
}

function getAddressFromCoordinates() {

    if (geocoder == null) {
        geocoder = new google.maps.Geocoder();
    }

    var input = $("#lat").val() + "," + $("#lng").val();
    var latlngStr = input.split(',', 2);
    var latlng = { lat: parseFloat(latlngStr[0]), lng: parseFloat(latlngStr[1]) };


    geocoder.geocode({ 'location': latlng }, function (results, status) {
        if (status === 'OK') {
            if (results[1]) {
                alert(results[0].formatted_address);
            } else {
                window.alert('No results found');
            }
        } else {
            window.alert('Geocoder failed due to: ' + status);
        }
    });

}

// Server-side = wild roundtrip, DONT USE
function getAddressFromCoordinatesSERVERSIDE() {
    //debugger;
    var lat = $("#lat").val();
    var lon = $("#lng").val();


    $.get("RequestAddressFromCoordinates", "lat=" + lat +"&lon=" + lon, function (data) {
        alert(data.addressResult);
    });
}