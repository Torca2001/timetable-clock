const UserInfo = require("os").userInfo().username;
var util = require("util");
var http = require("http");
const UserInfo = require("os").userInfo().username;
const util = require("util");
const http = require("http");
const fs = require("fs");
const low = require("lowdb");
const FileSync = require('lowdb/adapters/FileSync');
const request = require("request");
const adapter = new FileSync('db.json');
const db = low(adapter);

db.defaults({ 
	subject: [],
	room: [],
}).write();



function currentSubject(week) {
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
			console.log(day);
		}
		else {
			console.log('Not a school day.');
		}
	}
	else {
		if (currentDay < 6) {
			day = currentDay + 5;
			console.log(day);
		}
		else {
			console.log('Not a school day.');
		}
	}
	//Getting Timetable
	var timetableUrl = "http://intranet.trinity.vic.edu.au/intranet_aux_anon/timetable/student/" + UserInfo + "/" + currentYear + "/3";
	var timetablePath = "/intranet_aux_anon/timetable/student/" + UserInfo + "/" + currentYear + "/3";
	console.log(timetableUrl);
	
	
	var content = "";
	
	content = request(timetableUrl, function(error, response, html){
	
	  return html;
	})
	console.log(content);
	var re = /\s\i\u=<h1>.*Timetable.*-(.*)-.*Term.*classname">(.*)<\/span.*classname">(.*)<\/span.*classname">(.*)<\/span.*classname">(.*)<\/span.*classname">(.*)<\/span.*/
	var classes = re.exec(content);
	
	console.log('Classes: ' + classes)
	/*db.get('subject')
	.push({ id: 1, title: })
	.write();*/
}