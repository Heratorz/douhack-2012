
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
                if (loyaltyText.text != loyalty.loyalty) {
                    var prevLoyalty = loyaltyText.text;
                    loyaltyText.text = loyalty.loyalty;
                    if (loyalty.loyalty > prevLoyalty)
                        counterRect.state = "PositiveState";
                    else
                        counterRect.state = "NegativeState";
                    counterRect.state = "";
                }
            }
        }
    };
    xhr.send(null);
}
