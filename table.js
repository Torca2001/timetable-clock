const remote = require('electron').remote;
a=remote.getGlobal('sharedObject').asl;
tableout=remote.getGlobal('sharedObject').tablev;
function run () {
	document.getElementById("counts").innerHTML =tableout[120];
	var out=period(a[3][4]);
	document.getElementById("P1").innerHTML =out[0];
	document.getElementById("P1L").innerHTML =out[1];
	var out=period(a[5][4]);
	document.getElementById("P2").innerHTML =out[0];
	document.getElementById("P2L").innerHTML =out[1];
	var out=period(a[8][4]);
	document.getElementById("P3").innerHTML =out[0];
	document.getElementById("P3L").innerHTML =out[1];
	var out=period(a[10][4]);
	document.getElementById("P4").innerHTML =out[0];
	document.getElementById("P4L").innerHTML =out[1];
	var out=period(a[13][4]);
	document.getElementById("P5").innerHTML =out[0];
	document.getElementById("P5L").innerHTML =out[1];
	var out=period(a[15][4]);
	document.getElementById("P6").innerHTML =out[0];
	document.getElementById("P6L").innerHTML =out[1];
	kl=remote.getGlobal('sharedObject').currnpd;
	var P1D="#ffffff";
	var P2D="#ffffff";
	var P3D="#ffffff";
	var P4D="#ffffff";
	var P5D="#ffffff";
	var P6D="#ffffff";
	if (kl<5){
		P1D="#00ffff";
	}else if (kl<7) {
		P2D="#00ffff";
	}
	else if (kl<9) {
		P3D="#00ffff";
	}
	else if (kl<11) {
		P4D="#00ffff";
	}
	else if (kl<14) {
		P5D="#00ffff";
	}
	else {P6D="#00ffff";}
	document.getElementById("P1").style.color=P1D;
	document.getElementById("P2").style.color=P2D;
	document.getElementById("P3").style.color=P3D;
	document.getElementById("P4").style.color=P4D;
	document.getElementById("P5").style.color=P5D;
	document.getElementById("P6").style.color=P6D;
	document.getElementById("P1L").style.color=P1D;
	document.getElementById("P2L").style.color=P2D;
	document.getElementById("P3L").style.color=P3D;
	document.getElementById("P4L").style.color=P4D;
	document.getElementById("P5L").style.color=P5D;
	document.getElementById("P6L").style.color=P6D;
}

function period (curr) {
	currnt=schoolDay(remote.getGlobal('sharedObject').dayof)*2-2
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

function schoolDay (off) {
	//Current Time
	var off=Number(off);
	if (off>-1){}else{var off=0;}
	var date = new Date();
	var reference = '2017-8-28'; //reference date of a day 1
	var reference = new Date(reference);
	reference.setHours(0); //set the reference time to be at hour 0 as by defualt its at midday
	var reference=(Math.ceil((date.getTime()- reference.getTime())/86400000)-off)%14; //comparing two dates in milliseconds, then dividing the milliseconds into days then rounding up and modulo by 14
	if (reference==6||reference==7||reference==13){reference=0;}
	if (reference>7){reference=date.getDay()+5}
	return reference;
}

setInterval(run,1000);