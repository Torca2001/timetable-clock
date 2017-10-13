//Requirements
const {app, BrowserWindow} = require('electron');
const electron = require('electron');
const Positioner = require("electron-positioner");

let win

function createSettingsWindow () {
	win = new BrowserWindow({
	  width: 640,
	  height: 480,
	  frame: true,
	  transparent: false,
	  resizable: false
  });	
}
