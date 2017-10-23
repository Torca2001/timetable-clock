const fs = require('fs');

function setLoc (loc) {
	document.getElementById("locopt").innerHTML = loc;
}
function saveOptions () {
	var path = "settings.txt";
	var file = new File(path);
	loc = document.getElementById("locopt").innerHTML;
	
}