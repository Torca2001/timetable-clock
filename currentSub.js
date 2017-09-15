//Requirements
const UserInfo = require("os").userInfo().username;
const util = require("util");
const http = require("http");
const fs = require("fs");
const download = require("download");

/*
This is the main function which:
* Downloads the timetable html file
* reads from it (in development)
* Finds the day in the school timetable
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
	//The location of the html file after it is saved.
	var openDestination = saveDestination + "/index.html";
	
	console.log(content);
	//Regular Expression finding the subject names from the html.
	var re = /\siu=<h1>.*Timetable.*-(.*)-.*Term.*classname">(.*)<\/span.*classname">(.*)<\/span.*classname">(.*)<\/span.*classname">(.*)<\/span.*classname">(.*)<\/span.*/
	var classes = re.exec(content);
	console.log('Classes: ' + classes);
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
	//Should return the content of the file, however it doesn't do this.
	return content;
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
