function showGoogleMap() {

    if (navigator.geolocation) {
        navigator.geolocation.getcurrentPosition(OnSuccess, OnError, {
            enableHighAccuracy: true,
            maximumAge: 5000,
            timeout: 1000
        });
    } else {
        $("map").Text = "No support for geolocation!";
    }

}