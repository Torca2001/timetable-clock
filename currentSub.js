//Requirements
const UserInfo = require("os").userInfo().username;
const util = require("util");
const http = require("http");
const fs = require("fs");
const download = require("download");

/*
This is the main function which:
* Downloads the timetable html file
* Finds the day in the school timetable
* Calls the function to read from the timetable
*/
function checkSubjects(week) {
	//Current Time
	var date = new Date();
	var currentHour = date.getHours();
	var currentMinute = date.getMinutes();
	var currentDay = date.getDay();
	var currentYear = date.getFullYear();
	
	//School Day On Timetable
	var SchoolDay = schoolDay(week);
	
	//Getting Timetable
	var timetableUrl = "http://intranet.trinity.vic.edu.au/intranet_aux_anon/timetable/student/" + UserInfo + "/" + currentYear + "/3";
	var timetablePath = "/intranet_aux_anon/timetable/student/" + UserInfo + "/" + currentYear + "/3";
	console.log(timetableUrl);
	
	//The destination for the timetable html file.
	var saveDestination = "resources/" + UserInfo + currentYear + "/";	
	var saveDestination2 = "resources/" + UserInfo + currentYear + "/index.html";
	
	//Checking if the directory for the html file exists.
	//If it doesn't, it creates it.
	fs.stat(saveDestination, function (err, stats) {
		 if (err) {
		    fs.mkdirSync(saveDestination);
		 }
	});
	
	//Downloads the timetable html file, and is supposed to save contents to variable. However, at the moment it doesn't do this.
	var content = downloadTimetable(content, timetableUrl, saveDestination, saveDestination2);
	
	openDestination = saveDestination2;
	var classes = [];
	
	classes = readTable(openDestination);
	personName = readName(classes);
	
	console.log(classes);
	console.log(personName);
	return classes;
}

//This is the function which actually downloads the files
function downloadTimetable (content, timetableUrl, saveDestination, saveDestination2) {
	
	download(timetableUrl, saveDestination).then(() => {
      console.log('Downloaded File.');
	});
	
	download(timetableUrl).then(data => {
    fs.writeFileSync(saveDestination2, data);
	content = data;
	});
 
	download(timetableUrl).pipe(fs.createWriteStream(saveDestination2));
 
	Promise.all([
    timetableUrl
	].map(x => download(x, saveDestination))).then(() => {
    console.log('files downloaded!');
	});
}

//This function finds the current school day on the timetable (1-10)
function schoolDay (week) {
	//Current Time
	var date = new Date();
	var currentHour = date.getHours();
	var currentMinute = date.getMinutes();
	var currentDay = date.getDay();
	var currentYear = date.getFullYear();
	var day = 0;
	//Getting Day on Timetable
	if (week == 1) {
		if (currentDay < 6) {
			day = currentDay;
		}
		else {
			console.log('Not a school day.');
			//If it isn't a school day, set the day variable to 99.
			day = 99;
		}
	}
	else {
		if (currentDay < 6) {
			day = currentDay + 5;
		}
		else {
			console.log('Not a school day.');
			//If it isn't a school day, set the day variable to 99.
			day = 99;
		}
	}
	return day;
}

function readTable (openDestination) {
	var newd = "";
	var theList = [];
	var incrementCounter = 0;

	// First I want to read the file
	var text = fs.readFileSync(openDestination,'utf8');
	var classes = text.search('Timetable - ') + 12;
	var classes2 = text.search(" - Term ");

	theList[120] = gather(); //set item 120 in list to the name fetched from the page

	while (incrementCounter < 120){ //Loop get all periods 60 periods in total + 60 for location
		var tl = 0;
		var data = "";

		var classes = text.search('sname">')+7; //find the beginning of class name, add offset
		var classes2 = text.search(" \n    \\("); //find end

		if (classes + 20 < classes2){
			classes2 = classes+7;
			tl = 1;
		} //check whether it accidentally went to the next period for things like Chapel
		
		theList[incrementCounter] = gather(); //save period name to list

		if (tl == 1){
			classes = classes2;   //Check to set location to Null in cases like Chapel
		} else {
			classes = classes2 + 7; //reuse previous search add offset due to length of search
			var classes2 = text.search("\\)"); //find end of location
		}
		
		theList[incrementCounter + 1] = gather(); //save location to list
		incrementCounter += 2; //add 2 to list to move on
		classes2 += 1; //Very makes sure to move past previous items
		var newd = ""; //clear variable

		while (classes2 < text.length){
			newd += text[classes2];
			classes2 += 1;
		} //recreate string to remove already fetched item
		text = newd; //save it to repeat
	}
	return theList;
}

function gather () {
	var data = ""; //fetch all items between two points and return it
	while (classes<classes2){
	data += text[classes];
	classes += 1;
	}
	return data
}

function readName(list) {
		return list[120];
}