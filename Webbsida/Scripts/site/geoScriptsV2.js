// An IIFE, resulting in something like a C# class.
var GoogleMapsEventController = function () {

    // "private member fields" for GoogleMapsEventController
    var map;
    var geocoder;
    var userMarker = false;

    // Use geolocation to try to fetch user location, if successful; invoke "onSuccess" else "onError"
    function getUserPosition() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(onSuccess, onError, {
                //Default values used for now here:
                enableHighAccuracy: false,
                timeout: Infinity,
                maximumAge: 0
            });

        } else {
            document.getElementById("map").innerHTML = "GeoLocation not supported";
        }
    }

    function onSuccess(position) {
        setUserLocation(position.coords.latitude, position.coords.longitude);
    }

    function onError(error) {
        switch (error.code) {
            case error.PERMISSION_DENIED:
                alert("User denied the request for Geolocation.");
                break;
            case error.POSITION_UNAVAILABLE:
                alert("Location information is unavailable.");
                break;
            case error.TIMEOUT:
                alert("The request to get user location timed out.");
                break;
            case error.UNKNOWN_ERROR:
                alert("An unknown error occurred.");
                break;
        }

        //Could not get user location, center map on Holmogadd for debug!
        setUserLocation(63.589507, 20.750420);
    }

    // saves the retrieved position to localStorage.
    function setUserLocation(lat, long) {
        localStorage.setItem("pos_lat", lat);
        localStorage.setItem("pos_long", long);
    }


    // Three different main entry points when you want to use a map:
    // 1. Start with a map displaying ALL events
    // TODO: Just list active events?
    var initAllEventsDisplayingMap = function () {

        getUserPosition();

        map = new google.maps.Map(document.getElementById("map"), {
            center: { lat: parseFloat(localStorage.getItem("pos_lat")), lng: parseFloat(localStorage.getItem("pos_long")) },
            zoom: 8
        });

        $.ajax({
            type: "GET",
            //TODO: We really need to use WebApi here to get the url correct!?
            url: "/Geo/GetEventsToJson",
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
    };



    // 2. Map displaying single event
    var initSingleEventDisplayingMap = function () {


        var eventId = $("#Id").val();
        // TODO: No need for wild roundtrip to server, just get the data from the view in jQuery!?
        // ALTOUGH, might set this on hold, maybe you want a singelEventMap without having to feed data?
        //var latitude = $("#latitude");
        //var longitude = $("#longitude");
        //var name = $("#name");

        $.ajax({
            type: "GET",
            url: "/Geo/GetSingleEventToJson",
            data: { id: eventId },
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (data) {

                map = new google.maps.Map(document.getElementById("map"), {
                    center: { lat: parseFloat(data.Latitude), lng: parseFloat(data.Longitude) },
                    zoom: 11
                });

                // TODO get one event, mark it
                addEventMarker(data.Name, data.Latitude, data.Longitude);

            },
            error: function (response) {
                alert("Could not retrieve data from server!");
            }
        });

    };

    function addEventMarker(name, lat, long) {
        new google.maps.Marker
        (
            {
                position: new google.maps.LatLng(lat, long),
                map: map,
                title: name
            }
        );
    }

    // 3. Start with a pickermap
    var initPickerMap = function () {

        getUserPosition();

        map = new google.maps.Map(document.getElementById("map"), {
            center: { lat: parseFloat(localStorage.getItem("pos_lat")), lng: parseFloat(localStorage.getItem("pos_long")) },
            zoom: 10
        });

        google.maps.event.addListener(map, "click", function (event) {
            //Get the location that the user clicked.
            var clickedLocation = event.latLng;
            //If the marker hasn't been added.
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
    };


    // 4. Distance calculations
    // When you get your results, start looping trough them and calculate distances,
    // return a sorted dictionary!

    var getEventsWithDistance = new function () {

        // TODO: If this fails return!
        getUserPosition();

        //var queryString = "lat=" + localStorage.getItem("pos_lat") + "?long=" + localStorage.getItem("pos_long");
        var result = [];
        $.get("/api/GeoData", function (data) {

            $.each(data, function (id, event) {

                // TODO: Sorting value NOT CORRECT
                result.push({
                    key: event.Id,
                    value: getDistance({ lat: localStorage.getItem("pos_lat"), lng: localStorage.getItem("pos_long") },
                    { lat: event.Latitude, lng: event.Longitude }
                    )
                });

            });

            console.log(result); // original dictionary
            console.log(sortJsObject(result)); // sorted dict

            return sortJsObject(result);

        });

        function sortJsObject(dict) {

            var keys = [];
            for (var key in dict) {
                keys[keys.length] = key;
            }

            var values = [];
            for (var i = 0; i < keys.length; i++) {
                values[values.length] = dict[keys[i]];
            }

            var sortedValues = values.sort(sortNumber);
            //alert(sortedValues);

            //console.log(sortedValues);
            return sortedValues;
        }

        // this is needed to sort values as integers
        function sortNumber(a, b) {
            return a - b;
        }

        // used in distance calculation getDistance
        var rad = function (x) {
            return x * Math.PI / 180;
        };

        var getDistance = function (p1, p2) {
            var R = 6378137; // Earth’s mean radius in meter
            var dLat = rad(p2.lat - p1.lat);
            var dLong = rad(p2.lng - p1.lng);
            var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
              Math.cos(rad(p1.lat)) * Math.cos(rad(p2.lat)) *
              Math.sin(dLong / 2) * Math.sin(dLong / 2);
            var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
            var d = R * c;
            return d; // returns the distance in meter
        };

    };
    // END DISTANCE

    function setMarkerByUserInputAddress() {

        geocoder = new google.maps.Geocoder();

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
                    google.maps.event.addListener(userMarker, "dragend", function (event) {
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

    function markerLocation() {
        //Get location.
        var currentLocation = userMarker.getPosition();
        //Add lat and lng values to a field that we can save.
        document.getElementById("lat").value = currentLocation.lat(); //latitude
        document.getElementById("lng").value = currentLocation.lng(); //longitude
    }


    // THIS IS THE PUBLIC METHODS THAT GETS EXPOSED,
    // Call them in the script tag for the googleMaps api, like so :
    // GoogleMapsEventController.initPickerMap

    // Or to just save userPosition to localStorage:
    // GoogleMapsEventController.getUserPosition();
    // retrieve with: localStorage.getItem("pos_lat") & localStorage.getItem("pos_long")
    return {
        getUserPosition: getUserPosition,
        initAllEventsDisplayingMap: initAllEventsDisplayingMap,
        initSingleEventDisplayingMap: initSingleEventDisplayingMap,
        initPickerMap: initPickerMap,
        setMarkerByUserInputAddress: setMarkerByUserInputAddress,
        getEventsWithDistance: getEventsWithDistance
    };

}();



//
// -> Old GeoPicker stuff not used
//

//function getAddressFromCoordinates() {

//    if (geocoder == null) {
//        geocoder = new google.maps.Geocoder();
//    }

//    var input = $("#lat").val() + "," + $("#lng").val();
//    var latlngStr = input.split(',', 2);
//    var latlng = { lat: parseFloat(latlngStr[0]), lng: parseFloat(latlngStr[1]) };


//    geocoder.geocode({ 'location': latlng }, function (results, status) {
//        if (status === 'OK') {
//            if (results[1]) {
//                alert(results[0].formatted_address);
//            } else {
//                window.alert('No results found');
//            }
//        } else {
//            window.alert('Geocoder failed due to: ' + status);
//        }
//    });

//}

//// Request coordinates from address in a form-field
//// The insane way (extra roundtrip to the server)
//function requestCoordinatesFromAddress() {
//    var getAddressFormValue = $("#input_address").val();

//    $.get("RequestCoordinatesFromAddress", "address=" + getAddressFormValue, function (data) {
//        alert(data.Latitude + ":" + data.Longitude);
//    });

//}

//// Server-side = wild roundtrip, DONT USE
//function getAddressFromCoordinatesSERVERSIDE() {
//    //debugger;
//    var lat = $("#lat").val();
//    var lon = $("#lng").val();


//    $.get("RequestAddressFromCoordinates", "lat=" + lat +"&lon=" + lon, function (data) {
//        alert(data.addressResult);
//    });
//}