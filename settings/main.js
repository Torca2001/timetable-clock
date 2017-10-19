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