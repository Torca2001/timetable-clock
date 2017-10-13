var tableout;
var classes="";
var classes2="";
var datl="";
const fs = require("fs");
var SchoolTerm=schoolTerm();
var date = new Date();
var currentYear = date.getFullYear();
const main = require('electron').remote.require('./main')
var tableout=table("resources/" + UserInfo + currentYear + SchoolTerm + "/index.html")
var a=[[8,15,8,15,"School Start",8,15],
[8,45,8,25,"Form/House",8,45],
[8,50,8,30,"Gt1",8,50],
[9,40,9,20,"P1",9,35],
[9,45,9,25,"Gt2",9,40],
[10,35,10,15,"P2",10,25],
[10,50,10,30,"Recess",10,40],
[10,55,10,35,"Gt3",10,45],
[11,45,11,25,"P3",11,30],
[11,50,11,30,"Gt4",11,35],
[12,40,12,15,"P4",12,20],
[13,25,13,25,"Lunch",12,55],
[13,30,13,30,"Gt5",13,0],
[14,20,14,20,"P5",13,45],
[14,25,14,25,"Gt6",13,50],
[15,15,15,15,"P6",14,35],
];
if (tableout=="failed"){
	a[2][4]="Go to Period 1";
	a[3][4]="Period 1";
	a[4][4]="Go to Period 2";
	a[5][4]="Period 2";
	a[7][4]="Go to Period 3";
	a[8][4]="Period 3";
	a[9][4]="Go to Period 4";
	a[10][4]="Period 4";
	a[12][4]="Go to Period 5";
	a[13][4]="Period 5";
	a[14][4]="Go to Period 6";
	a[15][4]="Period 6";
}
var confag = fs.readFileSync("resources/config.txt",'utf8');
var dayoff=confag[1];
var kl=confag[0];
function run(){
var confag = fs.readFileSync("resources/config.txt",'utf8');
var dayoff=confag[1];
var kl=confag[0];
var offset=-30;
var date = new Date();
var day=0; //2 for Wednesday 5 for Early Finish
var totalm=(date.getHours()*60*60)+(date.getMinutes()*60)+date.getSeconds()-offset;
var out=0;
var xma=1;
var color="#ffffff";
if (date.getDay()==3){day=2}else if(kl==1){day=5;color="#00ffff";}
for(count = 0; count < a.length; count++){
	xma=((a[count][0+day]*60*60)+(a[count][1+day]*60))-totalm;
	if (xma>0){break}else{xma=0}}
if (count==a.length){out="End"}else{out=a[count][4]}
if (date.getDay()==6||date.getDay()==0){out="Weekend";xma=0;}
out = period(out);
document.getElementById("counter").innerHTML = out[0];
document.getElementById("locate").innerHTML = out[1];
var hours=Math.floor(xma/3600);
var minutes=Math.floor(xma/60)%60;
var seconds=xma%60;
if (hours<10){hours="0"+hours};
if (minutes<10){minutes="0"+minutes};
if (seconds<10){seconds="0"+seconds};
document.getElementById("counterout").innerHTML = hours +":"+minutes+":"+seconds;
document.getElementById("days").innerHTML = "Day:"+schoolDay(dayoff);
document.getElementById('counter').style.color = color;

if (count+1<a.length){km=period(a[count+1][4]);document.getElementById('counter').title = km[0]+" "+km[1] ;}
else {document.getElementById('counter').title = "";}
}
function early(){kl=1-kl;
var confag = fs.readFileSync("resources/config.txt",'utf8');
fs.writeFile("resources/config.txt",kl+confag[1]+confag[2],'utf8');
run();
}
function daychg() {dayoff=7-dayoff;
var confag = fs.readFileSync("resources/config.txt",'utf8');
fs.writeFile("resources/config.txt",confag[0]+dayoff+confag[2],'utf8');
run();
}

function srtboot() {//this runs on boot of the html

	
}


function table (openDestination) {
	var newd = "";
	var theList = [];
	var incrementCounter = 0;
	if (fs.existsSync(openDestination)==false){return "failed"};
	// First I want to read the file
	var text = fs.readFileSync(openDestination,'utf8');
	var classes = text.search('Timetable - ') + 12;
	var classes2 = text.search(" - Term ");
	datl = ""; //fetch all items between two points and return it
	while (classes<classes2){
	datl += text[classes];
	classes += 1;
	}
	theList[120] = datl; //set item 120 in list to the name fetched from the page
	while (incrementCounter < 120){ //Loop get all periods 60 periods in total + 60 for location
		var tl = 0;
		var datl = "";

		var classes = text.search('sname">')+7; //find the beginning of class name, add offset
		var classes2 = text.search(" \n    \\("); //find end

		if (classes + 20 < classes2){
			classes2 = classes+7;
			tl = 1;
		} //check whether it accidentally went to the next period for things like Chapel
		datl = ""; //fetch all items between two points and return it
		while (classes<classes2){
		datl += text[classes];
		classes += 1;
		}
		theList[incrementCounter] = datl; //save period name to list

		if (tl == 1){
			classes = classes2;   //Check to set location to Null in cases like Chapel
		} else {
			classes = classes2 + 7; //reuse previous search add offset due to length of search
			var classes2 = text.search("\\)"); //find end of location
		}
		datl = ""; //fetch all items between two points and return it
		while (classes<classes2){
		datl += text[classes];
		classes += 1;
		}
		theList[incrementCounter + 1] = datl; //save location to list
		incrementCounter += 2; //add 2 to list to move on
		classes2 += 1; //Very makes sure to move past previous items
		var newd = ""; //clear variable

		while (classes2 < text.length){
			newd += text[classes2];
			classes2 += 1;
		} //recreate string to remove already fetched item
		text = newd; //save it to repeat
	}
	return theList;
}

function readName (list) {
		return list[120];
}

function period (curr) {
	currnt=schoolDay(dayoff)*2-2
	if (currnt>8){currnt+=50;}
	if (curr=="Gt1"){
		return ["Go to "+tableout[currnt],tableout[currnt+1]];
	}
	else if (curr=="P1"){
		return [tableout[currnt],tableout[currnt+1]];
	}
	else if (curr=="Gt2"){
		return ["Go to "+tableout[currnt+10],tableout[currnt+11]];
	}
	else if (curr=="P2"){
		return [tableout[currnt+10],tableout[currnt+11]];
	}
	else if (curr=="Gt3"){
		return ["Go to "+tableout[currnt+20],tableout[currnt+21]];
	}
	else if (curr=="P3"){
		return [tableout[currnt+20],tableout[currnt+21]];
	}
	else if (curr=="Gt4"){
		return ["Go to "+tableout[currnt+30],tableout[currnt+31]];
	}
	else if (curr=="P4"){
		return [tableout[currnt+30],tableout[currnt+31]];
	}
	else if (curr=="Gt5"){
		return ["Go to "+tableout[currnt+40],tableout[currnt+41]];
	}
	else if (curr=="P5"){
		return [tableout[currnt+40],tableout[currnt+41]];
	}
	else if (curr=="Gt6"){
		return ["Go to "+tableout[currnt+50],tableout[currnt+51]];
	}
	else if (curr=="P6") {
		return [tableout[currnt+50],tableout[currnt+51]];
	}
	else {return [curr,""];}
}


setInterval(run,1000);
