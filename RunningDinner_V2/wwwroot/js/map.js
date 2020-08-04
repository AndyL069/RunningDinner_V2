function createMap(mapCenterLat, mapCenterLong, appId, appCode) {
    var mapContainer = document.getElementById("map-container");
    var platform = new H.service.Platform({
        app_id: appId,
        app_code: appCode,
        useHTTPS: true
    });

    var startCoordinates = new H.geo.Point(mapCenterLat, mapCenterLong);

    // Displaying the map
    var mapOptions = {
        center: startCoordinates,
        zoom: 14
    };

    var defaultLayers = platform.createDefaultLayers();
    var map = new H.Map(
        mapContainer,
        defaultLayers.normal.map,
        mapOptions);

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

            // Center and zoom the map so that the whole isoline polygon is
            // in the viewport:
            map.setViewBounds(isolinePolygon.getBounds());
        },

        onError = function (error) {
            console.log(error);
        };

    router.calculateIsoline(calculateIsolineParams, onResult, onError);
}