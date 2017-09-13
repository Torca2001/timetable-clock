const UserInfo = require("os").userInfo().username;
const util = require("util");
const http = require("http");
const fs = require("fs");
const download = require("download");

function checkSubjects(week) {
	//Current Time
	var date = new Date();
	var currentHour = date.getHours();
	var currentMinute = date.getMinutes();
	var currentDay = date.getDay();
	var currentYear = date.getFullYear();
	//School Day On Timetable
	var SchoolDay = schoolDay(week);
	if (day == 99) {
		
	}
	//Getting Timetable
	var timetableUrl = "http://intranet.trinity.vic.edu.au/intranet_aux_anon/timetable/student/" + UserInfo + "/" + currentYear + "/3";
	var timetablePath = "/intranet_aux_anon/timetable/student/" + UserInfo + "/" + currentYear + "/3";
	console.log(timetableUrl);
	var saveDestination = "res/" + UserInfo + currentYear + "/";	
	var saveDestination2 = "res/" + UserInfo + currentYear + "/index.html";
	
	fs.stat(saveDestination, function (err, stats) {
		 if (err) {
		    fs.mkdirSync(saveDestination);
		 }
	});
	
	var content = downloadTimetable(content, timetableUrl, saveDestination, saveDestination2);

	var openDestination = saveDestination + "/index.html";
	
	console.log(content);
	var re = /\siu=<h1>.*Timetable.*-(.*)-.*Term.*classname">(.*)<\/span.*classname">(.*)<\/span.*classname">(.*)<\/span.*classname">(.*)<\/span.*classname">(.*)<\/span.*/
	var classes = re.exec(content);
	console.log('Classes: ' + classes);
	return classes;
}

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
	
	return content;
}

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
			day = 99;
		}
	}
	else {
		if (currentDay < 6) {
			day = currentDay + 5;
		}
		else {
			console.log('Not a school day.');
			day = 99;
		}
	}
	return day;
}