var kl=0
function run(){
var date = new Date(); //2 for Wednesday 5 for Early Finish
var offset=-30;
var day=0;
var totalm=(date.getHours()*60*60)+(date.getMinutes()*60)+date.getSeconds()-offset;
var out=0;
var xma=1;
var a=[[8,15,8,15,"School Start",8,15],
[8,45,8,25,"Form/House",8,45],
[8,50,8,30,"Go to Period 1",8,50],
[9,40,9,20,"Period 1",9,35],
[9,45,9,25,"Go to Period 2",9,40],
[10,35,10,15,"Period 2",10,25],
[10,50,10,30,"Recess",10,40],
[10,55,10,35,"Go to Period 3",10,45],
[11,45,11,25,"Period 3",11,30],
[11,50,11,30,"Go to Period 4",11,35],
[12,40,12,15,"Period 4",12,20],
[13,25,13,25,"Lunch",12,55],
[13,30,13,30,"Go to Period 5",13,0],
[14,20,14,20,"Period 5",13,45],
[14,25,14,25,"Go to Period 6",13,50],
[15,15,15,15,"Period 6",14,35],
];
if (date.getDay()==3){day=2}else if(kl==1){day=5}
for(count = 0; count < a.length; count++){
	xma=((a[count][0+day]*60*60)+(a[count][1+day]*60))-totalm;
	if (xma>0){break}else{xma=0}}
if (count==a.length){out="End"}else{out=a[count][4]}
document.getElementById("counter").innerHTML = out;
var hours=Math.floor(xma/3600);
var minutes=Math.floor(xma/60)%60;
var seconds=xma%60;
if (hours<10){hours="0"+hours};
if (minutes<10){minutes="0"+minutes};
if (seconds<10){seconds="0"+seconds};
document.getElementById("counterout").innerHTML = hours +":"+minutes+":"+seconds;
}
function early(){kl=1-kl;}
setInterval(run,1000);