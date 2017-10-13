//Requirements
const UserInfo = require("os").userInfo().username;
const util = require("util");
const http = require("http");
const download = require("download");
/*
This is the main function which:
* Downloads the timetable html file
* reads from it (in development)
* Finds the day in the school timetable
*/
function checkSubjects() {
	//Current Time
	var date = new Date();
	var currentHour = date.getHours();
	var currentMinute = date.getMinutes();
	var currentDay = date.getDay();
	var currentYear = date.getFullYear();
	//School Day On Timetable
	var SchoolDay = schoolDay(0);
	var SchoolTerm=schoolTerm();
	var content="";
	
	//Getting Timetable
	var timetableUrl = "http://intranet.trinity.vic.edu.au/intranet_aux_anon/timetable/student/" + UserInfo + "/" + currentYear + "/" + SchoolTerm;
	var timetablePath = "/intranet_aux_anon/timetable/student/" + UserInfo + "/" + currentYear + "/" + SchoolTerm;
	console.log(timetableUrl);
	//The destination for the timetable html file.
	var saveDestination = "resources/" + UserInfo + currentYear + SchoolTerm + "/";	
	var saveDestination2 = "resources/" + UserInfo + currentYear + SchoolTerm + "/index.html";
	
	//Checking if the directory for the html file exists.
	//If it doesn't, it creates it.
	fs.stat(saveDestination, function (err, stats) {
		 if (err) {
		    fs.mkdirSync(saveDestination);
		 }
	});
	
	//Downloads the timetable html file, and is supposed to save contents to variable. However, at the moment it doesn't do this.
	
	checkInternet(function(isConnected) {
	if (isConnected){
		downloadTimetable(content, timetableUrl, saveDestination, saveDestination2);
	} else {console.log("Not Connected to Internet");}
	});
	//The location of the html file after it is saved.
	var openDestination = saveDestination + "/index.html";
}

function checkInternet(cb){
	require('dns').lookup('google.com',function(err) {
		if (err && err.code == "ENOTFOUND") {
		cb(false);	
		} else {
		cb(true);
		}
	})
}

//This is the function which actually downloads the files
function downloadTimetable (content, timetableUrl, saveDestination, saveDestination2) {
	download(timetableUrl).pipe(fs.createWriteStream(saveDestination+"cur.html"));
	Promise.all([
    timetableUrl
	].map(x => download(x, saveDestination))).then(() => {
    console.log('files downloaded!');
	text=fs.readFileSync(saveDestination+"cur.html",'utf8');
	if (text.length>30){
		fs.rename(saveDestination+"cur.html",saveDestination+"index.html",function(err){if (err){console.log(err);}});
	}
	});
	
	//console.log(download(timetableUrl,saveDestination2));
	
	
	//Should return the content of the file, however it doesn't do this.
}

//This function finds the current school day on the timetable (1-10)
function schoolDay (off) {
	//Current Time
	var off=Number(off);
	if (off>-1){}else{var off=0;}
	var date = new Date();
	var reference = '2017-8-28'; //reference date of a day 1
	var reference = new Date(reference);
	reference.setHours(0); //set the reference time to be at hour 0 as by defualt its at midday
	var reference=(Math.ceil((date.getTime()- reference.getTime())/86400000)-off)%14; //comparing two dates in milliseconds, then dividing the milliseconds into days then rounding up and modulo by 14
	if (reference==6||reference==7||reference==13){reference=0;}
	if (reference>7){reference=date.getDay()+5}
	return reference;
}
function schoolTerm () {
	var date = new Date();
	var reference = date.getFullYear()+'-7-02'; //reference date of a day 1
	var reference = new Date(reference);
	reference.setHours(0); //set the reference time to be at hour 0 as by defualt its at midday
	var reference=(Math.ceil(date.getTime()- reference.getTime())); //comparing the two dates
	if (reference>0){reference=3;}
	else {reference=1;}
	return reference;
}
