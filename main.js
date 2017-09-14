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
menu.append(menuItem)

app.on('browser-window-created', function (event, win) {
  win.webContents.on('context-menu', function (e, params) {
    menu.popup(win, params.x, params.y)
  })
})

//This function is executed when the window starts up
function createWindow () {
  win = new BrowserWindow({
	  width: 210,
	  height: 68,
	  frame: false,
	  transparent: true
  });
  win.setAlwaysOnTop(true);
  //This is a keyboard shortcut (Ctrl + I) which shows the info about the app.
  globalShortcut.register('CommandOrControl+I', function () {
    dialog.showMessageBox({
      type: 'info',
      message: 'App Details',
      detail: 'Version: 4.0.0\nAuthors: Joshua Harper & William Condick\nGithub: https://github.com/Mrmeguyme/timetable-clock/',
      buttons: ['OK']
    });
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

app.on('ready', createWindow)

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit()
	
  }
})

//This stops the keyboard shortcuts from running while the program isn't running.
app.on('will-quit', function () {
  globalShortcut.unregisterAll()
})

app.on('activate', () => {
  if (win === null) {
    createWindow()
  }
})
