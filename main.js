const {app, BrowserWindow} = require('electron')
const electron = require('electron')
const path = require('path')
const url = require('url')
const dialog = electron.dialog
const globalShortcut = electron.globalShortcut
const remote = electron.remote
const Menu = electron.Menu
const MenuItem = electron.MenuItem


let win
let rightClickPosition = null

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

function createWindow () {
  win = new BrowserWindow({width: 800, height: 600, frame: false})
  win.setAlwaysOnTop(true);
  globalShortcut.register('CommandOrControl+I', function () {
    dialog.showMessageBox({
      type: 'info',
      message: 'App Details',
      detail: 'Version: 4.0.0\nAuthors: Joshua Harper & William Condick',
      buttons: ['OK']
    })
  })
  win.loadURL(url.format({
    pathname: path.join(__dirname, 'index.html'),
    protocol: 'file:',
    slashes: true
  }))
  win.on('closed', () => {
    win = null
  })
}

app.on('ready', createWindow)

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit()
	
  }
})

app.on('will-quit', function () {
  globalShortcut.unregisterAll()
})

app.on('activate', () => {
  if (win === null) {
    createWindow()
  }
})
