var changesMap = {
    'MAJOR_CHANGE': 30,
    'MINOR_CHANGE': 5
};

function getLoyalty() {
    console.debug("getLoyalty()");
    var xhr = new XMLHttpRequest();
    xhr.open('GET', 'http://localhost:42424/loyalty/', true);
    xhr.setRequestHeader('Accept', 'application/json');
    xhr.onreadystatechange = function() {
        if (xhr.readyState == xhr.DONE) {
            if (xhr.status == 200) {
                var loyalty = eval('(' + xhr.responseText + ')');
                console.debug(loyalty.loyalty);

                megaLogic(loyalty.loyalty);
            }
        }
    };
    xhr.send(null);
}

function megaLogic(loyalty) {
    if (loyaltyText.text != loyalty) {
        var prevLoyalty = loyaltyText.text;
        var change = Math.abs(loyalty - prevLoyalty);

        loyaltyText.text = loyalty;

        if (loyalty > prevLoyalty) {    // positive case
            counterRect.state = "PositiveState";

            if (!megaCat.isAbsent) {
                if (change < changesMap.MINOR_CHANGE)
                    megaCat.state = "WinkState";
                else
                    megaCat.state = "MimimiState";
            }
            else
                megaCat.state = "EnteringState";
        }
        else {                          // negative case
            counterRect.state = "NegativeState";

            if (!megaCat.isAbsent) {
                if (change < changesMap.MINOR_CHANGE)
                    megaCat.state = "ScaryState";
                else if (change < changesMap.MAJOR_CHANGE)
                    megaCat.state = "FacepalmState";
                else
                    megaCat.state = "ExitAfterFacepalmState";
            }
        }
        counterRect.state = "";
    }
}


function decreaseLoyalty() {
    console.debug("decreaseLoyalty()");
    var params = "loyalty=-10";
    var xhr = new XMLHttpRequest();
    xhr.open('PUT', 'http://localhost:42424/loyalty/', true);
    xhr.setRequestHeader('Accept', 'application/json');
    xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xhr.setRequestHeader("Content-length", params.length);
    xhr.setRequestHeader("Connection", "close");
    xhr.onreadystatechange = function() {
        if (xhr.readyState == xhr.DONE) {
            if (xhr.status == 200) {
                if (megaCat.isAbsent)
                    megaCat.state = "EnteringState"
                else
                    megaCat.state = "FeedState"
            }
        }
    };
    xhr.send(params);
}
