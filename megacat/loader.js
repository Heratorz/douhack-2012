
function getLoyalty() {
    console.debug("getLoyalty()");
    var xhr = new XMLHttpRequest();
    xhr.open('GET', 'http://localhost:5000/loyalty/', true);
    xhr.setRequestHeader('Accept', 'application/json');
    xhr.onreadystatechange = function() {
        if (xhr.readyState == xhr.DONE) {
            if (xhr.status == 200) {
                var loyalty = eval('(' + xhr.responseText + ')');
                console.log(loyalty.loyalty);
            }
        }
    };
    xhr.send(null);
}
