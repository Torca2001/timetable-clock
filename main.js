//Requirements
const {app, BrowserWindow} = require('electron');
const electron = require('electron');
const path = require('path');
const url = require('url');
const dialog = electron.dialog;
const globalShortcut = electron.globalShortcut;
const remote = electron.remote;
const Menu = electron.Menu;
const MenuItem = electron.MenuItem;
const UserInfo = require("os").userInfo().username;
const util = require("util");
const http = require("http");
const download = require("download");
const fs = require("fs");
const Positioner = require("electron-positioner");
const github = require('octonode');
const shell = electron.shell;
const downloadFile = require('download-file')
var client = github.client();

global.sharedObject = {
	tablev: "",
	asl: "",
	dayof:"",
	early:"",
	currnpd:""
};

var CURRENT_VERSION = "4.3.0";
var version_url = "";
var latest_version = "";

//Downloads subjects, and works out day of school and term.
checkSubjects();
function checkSubjects() {
	//Current Time
	var date = new Date();
	var currentHour = date.getHours();
	var currentMinute = date.getMinutes();
	var currentDay = date.getDay();
	var currentYear = date.getFullYear();
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
	if (fs.existsSync("resources/config.txt")==false) {
		fs.writeFile("resources/config.txt","070",'utf8');
	}
	
	checkInternet(function(isConnected) {
	if (isConnected){
		downloadTimetable(content, timetableUrl, saveDestination, saveDestination2);
	} else {
		console.log("Not Connected to Internet");
	}
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
	download(timetableUrl).pipe(fs.createWriteStream(saveDestination + "cur.html"));
	Promise.all([timetableUrl].map(x => download(x, saveDestination))).then(() => {
    console.log('files downloaded!');
	text=fs.readFileSync(saveDestination+"cur.html",'utf8');
	if (text.length>30){
		fs.rename(saveDestination + "cur.html", saveDestination + "index.html", function(err) {
			if (err) {
				console.log(err);
			}
		});
	}
	});	
}

//This function finds the current school day on the timetable (1-10)
function schoolTerm () {
	var date = new Date();
	var reference = date.getFullYear()+'-7-02'; //reference date of a day 1
	var reference = new Date(reference);
	reference.setHours(0); //set the reference time to be at hour 0 as by defualt its at midday
	var reference=(Math.ceil(date.getTime() - reference.getTime())); //comparing the two dates
	if (reference>0){reference=3;}
	else {reference=1;}
	return reference;
}


let win
let rightClickPosition = null

//Setting up the right-click menu
const menu = new Menu()
const menuItem = new MenuItem({
  label: 'Close Timetable',
  click: () => {
    win.close()
  }
})
const menuItem2 = new MenuItem({
  label: 'Settings',
  click: () => {
	createSettingsWindow()
  }
})
const menuItem3 = new MenuItem({
	label: 'Fullscreen',
	click: () => {
		createBigWindow()
	}
})

menu.append(menuItem3);
menu.append(menuItem2);
menu.append(menuItem);
app.on('browser-window-created', function (event, win) {
  win.webContents.on('context-menu', function (e, params) {
    menu.popup(win, params.x, params.y)
  })
  let positioner = new Positioner(win);
  positioner.move("bottomRight");
})

function checkForUpdates () {
	var jsonContent = {};
	//Gets the content of the releases page, and saves it to /resources/updatelog.txt
	client.get('https://api.github.com/repos/mrmeguyme/timetable-clock/releases/latest', {}, function (err, status, body, headers) {	
		jsonContent = body;
		fs.stat("resources/updatelog.txt", function (err, stats) {
			if (err) {
				fs.mkdirSync("resources/updatelog.txt");
			}
		});
		fs.writeFile("resources/updatelog.txt", JSON.stringify(jsonContent), 'utf8');
		if (err) {
			console.log(err);
		}
	});
	var jsonString = fs.readFileSync("resources/updatelog.txt",'utf8');
	loc1 = jsonString.search('"browser_download_url":"') + 30;
	loc2 = jsonString.search('"}],"tarball_url"');
	loc3 = jsonString.search('"browser_download_url":"') + 87;
	loc4 = loc3 + 5;
	
	
	latest_version = "";
	version_url = "";
	
	while (loc3 < loc4) {
		latest_version += jsonString[loc3];
		loc3 += 1;
	}
	
	while (loc1 < loc2) {
		version_url +=  jsonString[loc1];
		loc1 +=  1;
	}
	console.log("Latest Version URL:")
	console.log(version_url);
	
	if (CURRENT_VERSION.charAt(0) < latest_version.charAt(0)) {
		console.log('New Update');
		runUpdate();
	}
	else if (CURRENT_VERSION.charAt(2) < latest_version.charAt(2)) {
		console.log('New Update');
		runUpdate();
	}
	else if (CURRENT_VERSION.charAt(4) < latest_version.charAt(4)) {
		console.log('New Update');
		runUpdate();
	}
	else {
		console.log('up-to-date!')
	}
}

function runUpdate () {
	dialog.showMessageBox({
		type: 'info',
		message: 'Update Available',
		detail: 'Do you wish to download?',
		buttons: ['Yes', 'No']
    }, function(response){
		if (response == 0) {
			//If update is requested
			var fullUrl = "http:" + version_url;

			var file = fs.createWriteStream("resources/time_table_clock.exe");
			var request = http.get(fullUrl, function(response) {
				response.pipe(file);
				shell.openItem(path.join(__dirname, "resources/timetable_clock.exe"));
				app.quit();
			});
		}
	});
}

//This function is executed when the window starts up.
//It creates the window from which the information is displayed
function createWindow () {
  win = new BrowserWindow({
	  width: 205,
	  height: 68,
	  frame: false,
	  transparent: true,
	  resizable: false,
	  //Stops the program from appearing on the taskbar.
	  skipTaskbar: true
  });
  win.setAlwaysOnTop(true);
  //This is a keyboard shortcut (Ctrl + I + L ) which shows the info about the app.
  globalShortcut.register('CommandOrControl+Alt+O', function () {
    dialog.showMessageBox({
      type: 'info',
      message: 'App Details',
      detail: 'Version: 4.2.0\nAuthors: Joshua Harper & William Condick\nGithub: https://github.com/Mrmeguyme/timetable-clock/',
      buttons: ['OK']
    });
  });
   globalShortcut.register('CommandOrControl+Alt+K', function () {
		if (win.isVisible()==true) {
			win.hide();
			win.SetAlwaysOnTop(false);
		}
		else {
			win.show();
			win.SetAlwaysOnTop(true);
		}
  });
  
  //This opens up the webpage or the actual application itself within the window.
  win.loadURL(url.format({
    pathname: path.join(__dirname, 'index.html'),
    protocol: 'file:',
    slashes: true
  }));
  win.on('closed', () => {
    win = null
  });
}

function createBigWindow () {
	bigwin = new BrowserWindow({
	  width: 1920,
	  height: 1080,
	  fullscreen: true
  });
  bigwin.setAlwaysOnTop(true);
  globalShortcut.register('CommandOrControl+Alt+J', function () {
		win = null;
  });
    bigwin.loadURL(url.format({
    pathname: path.join(__dirname, 'big.html'),
    protocol: 'file:',
    slashes: true
  }));
  bigwin.on('closed', () => {
    win = null
  });
}

function createSettingsWindow () {
  settingsWin = new BrowserWindow({
	  width: 640,
	  height: 480,
	  frame: true,
	  transparent: true,
	  resizable: false,
	  //Stops the program from appearing on the taskbar.
	  skipTaskbar: true
  });
  settingsWin.setAlwaysOnTop(true);
  
  //This opens up the webpage or the actual application itself within the window.
  settingsWin.loadURL(url.format({
    pathname: path.join(__dirname, 'settings/index.html'),
    protocol: 'file:',
    slashes: true
  }));
  settingsWin.on('closed', () => {
    settingsWin = null
  });
}

app.on('ready', function () {
	createWindow();
	checkForUpdates();
})

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit()
	
  }
})

//This stops the keyboard shortcuts from running while the program isn't running.
app.on('will-quit', function () {
  globalShortcut.unregisterAll();
})

app.on('activate', () => {
  if (win === null) {
    createWindow()
  }
})
