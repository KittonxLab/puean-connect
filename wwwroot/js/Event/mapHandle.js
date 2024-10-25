window.initMap = initMap;

const latInput = document.getElementById("location-lat");
const lngInput = document.getElementById("location-lng");
const clearLocationBtn = document.getElementById("location-clear-btn");

function initMap() {
    let curLat = parseFloat(latInput.value); 
    let curLng = parseFloat(lngInput.value);
    
    const defaultLatLng = new google.maps.LatLng(
        latInput.value !== ""? curLat: 13.7298889, 
        lngInput.value !== ""? curLng: 100.7782323
    );
    const zoom = 17;
    const map = new google.maps.Map(document.getElementById("google-map"), {
        center: defaultLatLng,
        zoom: zoom,
    });
    
    const input = document.getElementById("location-input");
    const autocomplete = new google.maps.places.Autocomplete(input, {
        fields: ["place_id", "geometry", "name", "formatted_address"],
    });

    autocomplete.bindTo("bounds", map);

    const geocoder = new google.maps.Geocoder();
    const marker = new google.maps.Marker({
        map: map,
    });
    
    if(latInput.value !== "" && lngInput.value !== "") {
        marker.setPosition(defaultLatLng);
    }

    google.maps.event.addListener(map, "click", async function (event) {
        marker.setPosition(event.latLng);
        marker.setVisible(true);
        map.panTo(event.latLng);
        // console.log(latInput.value, lngInput.value)
        geocoder
            .geocode({ location: event.latLng })
            .then(({ results }) => {
                input.value = results[0].formatted_address;
                addLatLng(event.latLng);
            })
            .catch((e) => {
                window.alert("Geocoder failed due to: " + event.latLng.lng());
                input.value = `https://www.google.co.th/maps/@${event.latLng.lat()},${event.latLng.lng()},18z?entry=ttul`;
            });
    });
    
    clearLocationBtn.addEventListener("click", () => {
        let locationName = document.getElementById("location-input");
        
        latInput.value = null;
        lngInput.value = null;
        locationName.value = null;
        
        marker.setVisible(false);
        alert("cleared");
    })
    
    autocomplete.addListener("place_changed", () => {
        const place = autocomplete.getPlace();

        if (!place.place_id) {
            return;
        }

        geocoder
            .geocode({ placeId: place.place_id })
            .then(({ results }) => {
                map.setZoom(zoom);
                map.setCenter(results[0].geometry.location);

                marker.setPosition(results[0].geometry.location);
                marker.setVisible(true);
                addLatLng(results[0].geometry.location);
            })
            .catch((e) => window.alert("Geocoder failed due to: " + e));
    });
}

function addLatLng(location) {
    latInput.value = location.lat();
    lngInput.value = location.lng();
}
