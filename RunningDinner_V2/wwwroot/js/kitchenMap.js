function createKitchenMap(mapCenterLat, mapCenterLong, kitchen, routes, party, apiKey) {
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
    var ui = H.ui.UI.createDefault(map, defaultLayers, "de-DE");

    // Enable the event system on the map instance:
    var mapEvents = new H.mapevents.MapEvents(map);

    // Instantiate the default behavior, providing the mapEvents object: 
    new H.mapevents.Behavior(mapEvents);

    // Resize the map when the window is resized
    window.addEventListener("resize",
        function () {
            map.getViewPort().resize();
        }
    );

    // add the isoline
    addIsoline(mapCenterLat, mapCenterLong, map, platform);

    // add the kitchen markers
    for (index = 0; index < kitchen.length; ++index) {
        addMarkerToMap(map, ui, kitchen[index]);
    } 

    // add the routes
    addPolylineToMap(map, routes);
    // add the party location
    addTextMarkerToMap(map, ui, party, "&#x1F379;");
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

function addMarkerToMap(map, ui, coordinate) {
    var group = new H.map.Group();
    map.addObject(group);
    let coordinateArray = coordinate.split(';', 3);

    // add 'tap' event listener, that opens info bubble, to the group
    group.addEventListener("tap", function (evt) {
        // event target is the marker itself, group is a parent event target
        // for all objects that it contains
        var bubble = new H.ui.InfoBubble(evt.target.getGeometry(), {
            content: coordinateArray[2]
        });
        // show info bubble
        ui.addBubble(bubble);
    }, false);

    var position = {
        lat: parseFloat(coordinateArray[0]),
        lng: parseFloat(coordinateArray[1])
    };

    var marker = new H.map.Marker(position);
    group.addObject(marker);
}

function addPolylineToMap(map, coordinates) {
    // Polyline
    var colors = ["Plum", "Red", "Orange", "Olive", "Green", "Purple", "Fuchsia", "Lime", "Teal", "Aqua", "Blue", "Navy", "Black", "Gray", "Silver"];
    var colorCounter = 0;
    for (let i = 0; i < coordinates.length; i += 3) {
        let coordinatesArray0 = coordinates[i].split(';');
        let coordinatesArray1 = coordinates[i+1].split(';');
        let coordinatesArray2 = coordinates[i+2].split(';');
        // Initialize a linestring and add all the points to it:
        let linestring = new H.geo.LineString();
        linestring.pushPoint(new H.geo.Point(parseFloat(coordinatesArray0[0]), parseFloat(coordinatesArray0[1])));
        linestring.pushPoint(new H.geo.Point(parseFloat(coordinatesArray1[0]), parseFloat(coordinatesArray1[1])));
        linestring.pushPoint(new H.geo.Point(parseFloat(coordinatesArray2[0]), parseFloat(coordinatesArray2[1])));
        // Initialize a polyline with the linestring:
        let polyline = new H.map.Polyline(linestring, {
            style: { 
                lineWidth: 10,
                fillColor: 'white',
                strokeColor: colors[colorCounter],
                lineDash: [0, 2],
                lineTailCap: 'arrow-tail',
                lineHeadCap: 'arrow-head'
            }
        });

        colorCounter++;
        // Add the polyline to the map:
        map.addObject(polyline);
    }
}

function addTextMarkerToMap(map, ui, coordinate, text) {
    let coordinateArray = coordinate.split(';', 3);
    var group = new H.map.Group();
    // add 'tap' event listener, that opens info bubble, to the group
    group.addEventListener("tap", function (evt) {
        // event target is the marker itself, group is a parent event target
        // for all objects that it contains
        var bubble = new H.ui.InfoBubble(evt.target.getGeometry(), {
            content: coordinateArray[2]
        });
        // show info bubble
        ui.addBubble(bubble);
    }, false);

    // Define a variable holding SVG mark-up that defines an icon image:
    var svgMarkup = '<svg width="36" height="36" ' +
        'xmlns="http://www.w3.org/2000/svg">' +
        '<rect stroke="#1b468d" fill="#1b468d" x="1" y="1" width="36" ' +
        'height="36" /><text x="18" y="26" font-size="18pt" ' +
        'font-family="Arial" font-weight="bold" text-anchor="middle" ' +
        'fill="white">' + text + "</text></svg>";
    
    var position = {
        lat: parseFloat(coordinateArray[0]),
        lng: parseFloat(coordinateArray[1])
    };

    var icon = new H.map.Icon(svgMarkup),
        marker = new H.map.Marker(position, { icon: icon });
    group.addObject(marker);
    map.addObject(group);
}
