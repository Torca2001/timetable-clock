const UserInfo = require("os").userInfo().username;
var util = require("util");
var http = require("http");



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
	
	var options = {
	  host: 'intranet.trinity.vic.edu.au',
      port: 80,
      path: timetablePath
	};
	
	var content = "";
	
	var req = http.request(options, function(res) {
	  console.log('STATUS: ' + res.statusCode);
	  console.log('HEADERS: ' + JSON.stringify(res.headers));
	  res.setEncoding('utf8');
	  res.on('data', function (chunk) {
        console.log('BODY: ' + chunk);
		content += chunk;
	  });
	});

	// write data to request body
	req.write('data\n');
	req.write('data\n');
	req.end();

	console.log(content);
}