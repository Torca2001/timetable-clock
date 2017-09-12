var date = new Date();
var hours = date.getHours();
var minutes= date.getMinutes();
var seconds=date.getSeconds();
var day=0
var totalm=(hours*60*60)+(minutes*60)+seconds
var xma=1
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
]
for(count = 0; count < a.length; count++){
	xma=((a[count][0+day]*60*60)+(a[count][1+day]*60))-totalm
	if (xma>0){break}
}
console.log(a[count][4]);