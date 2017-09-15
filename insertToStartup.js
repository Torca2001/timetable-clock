const UserInfo = require("os").userInfo().username;
var ws = require('windows-shortcuts');
var pathLoc = "C:/Users/" + UserInfo + "/AppData/Roaming/Microsoft/Windows/Start Menu/Programs/Startup/timetable.lnk";
var currentFile = __dirname + "run.bat";
ws.create(pathLoc, currentFile);