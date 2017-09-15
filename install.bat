@echo off
cd "C:\Users\%username%"
npm install download
npm install electron
npm install node-win-shortcut
node insertToStartup.js
electron .