let path;
let map;

const searchBtn = document.getElementById("searchBtn");
const searchBox = document.getElementById("placeInput");
const resetPathBtn = document.getElementById("resetPathBtn");
const submitPathBtn = document.getElementById("submitPathBtn");

searchBtn.addEventListener("click", function () { searchButtonClicked() }, false);
resetPathBtn.addEventListener("click", function () { deletePath() }, false);
submitPathBtn.addEventListener("click", function () { postCoordinates() }, false);

function searchButtonClicked() {
    let searchQuery = searchBox.value
    request = {
        query: searchQuery,
        fields: ["name", "geometry.location"],
    };
    searchPlace(request);
}

function searchPlace(requestInfo) {
    service = new google.maps.places.PlacesService(map);
    service.findPlaceFromQuery(requestInfo, (results, status) => {
        if (status === google.maps.places.PlacesServiceStatus.OK && results) {
            map.setCenter(results[0].geometry.location);
        }
    });
}

function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 9.854328, lng: -83.610901 },
        zoom: 10,
        mapTypeId: 'satellite'
    });
    path = new google.maps.Polyline({
        strokeColor: "#ffb703",
        strokeOpacity: 1.0,
        strokeWeight: 1.5,
    });
    path.setMap(map);
    map.addListener("click", addLatLng);
}

function addLatLng(event) {
    if (!path) {
        path = new google.maps.Polyline({
            strokeColor: "#ffb703",
            strokeOpacity: 1.0,
            strokeWeight: 1.5,
        });
        path.setMap(map);
    }
    const pathArray = path.getPath();
    pathArray.push(event.latLng);
}

function deletePath() {
    path.setMap(null);
    path = null;
}

function postCoordinates() {
    let array = path.getPath();
    let coordinates = [];
    for (let i = 0; i < array.Kd.length; i++) {
        coordinates.push({ "latitude": array.Kd[i].lat(), "longitude": array.Kd[i].lng(), "sequence": i, "name": "", "description": "", "id_stage": document.getElementById("stageID").innerHTML, "id_route": document.getElementById("stageRoute").innerHTML });
    }
    var JsonCoordinates = JSON.stringify(coordinates);
    $.post({
        url: '/Coordinate/addCoordinates',
        data: 'coordinates=' + JsonCoordinates,
        success: function () {
            window.location.href = 'stageList';
        }
    })
}

window.initMap = initMap;
