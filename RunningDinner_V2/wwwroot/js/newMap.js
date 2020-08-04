function createMap(mapCenterLat, mapCenterLong, apiKey) {
    var platform = new H.service.Platform({
        apikey: apiKey
    });

    // Displaying the map
    var defaultLayers = platform.createDefaultLayers();

    var map = new H.Map(
        document.getElementById('map-container'),
        defaultLayers.vector.normal.map, {
        center: { lat: mapCenterLat, lng: mapCenterLong },
        zoom: 12,
        pixelRatio: window.devicePixelRatio || 1
    });

    // Create the default UI:
    H.ui.UI.createDefault(map, defaultLayers, "de-DE");

    // Enable the event system on the map instance:
    var mapEvents = new H.mapevents.MapEvents(map);

    // Instantiate the default behavior, providing the mapEvents object: 
    new H.mapevents.Behavior(mapEvents);

    // Resize the map when the window is resized
    window.addEventListener("resize",
        function () {
            map.getViewPort().resize();
        });

    // add the isoline
    addIsoline(mapCenterLat, mapCenterLong, map, platform);
}

function addIsoline(mapCenterLat, mapCenterLong, map, platform) {
    // Assumption: the platform is instantiated
    var router = platform.getRoutingService();
    var calculateIsolineParams = {
        'mode': "fastest;car;motorway:-3",
        'start': "geo!" + mapCenterLat + "," + mapCenterLong,
        'range': "610",
        'rangetype': "time",
        'quality': "3"
    },
        onResult = function (result) {
            var isolineCoords = result.response.isoline[0].component[0].shape,
                linestring = new H.geo.LineString();

            // Add the returned isoline coordinates to a linestring:
            isolineCoords.forEach(function (coords) {
                linestring.pushLatLngAlt.apply(linestring, coords.split(","));
            });

            // Create a polygon and a marker representing the isoline:
            var isolinePolygon = new H.map.Polygon(linestring);

            // Add the polygon and marker to the map:
            map.addObjects([isolinePolygon]);
        },

        onError = function (error) {
            console.log(error);
        };

    // add the map outline
    router.calculateIsoline(calculateIsolineParams, onResult, onError);
}