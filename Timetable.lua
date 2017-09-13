a={{8,15,8,15,"School Start",8,15},
{8,45,8,25,"Form/House",8,45},
{8,50,8,30,"Go to Period 1",8,50},
{9,40,9,20,"Period 1",9,35},
{9,45,9,25,"Go to Period 2",9,40},
{10,35,10,15,"Period 2",10,25},
{10,50,10,30,"Recess",10,40},
{10,55,10,35,"Go to Period 3",10,45},
{11,45,11,25,"Period 3",11,30},
{11,50,11,30,"Go to Period 4",11,35},
{12,40,12,15,"Period 4",12,20},
{13,25,13,25,"Lunch",12,55},
{13,30,13,30,"Go to Period 5",13,0},
{14,20,14,20,"Period 5",13,45},
{14,25,14,25,"Go to Period 6",13,50},
{15,15,15,15,"Period 6",14,35},
}
flash=255
green=255
blue=255
days=0
offset=0
i=1
kl=0
function Update()
	offset= tonumber(SKIN:GetVariable('offset'))
	kl=kl+1
	if (kl>5) then
	kl=0
	i=1
	end
	if (os.date("%A") == "Wednesday") then
	days=2
	elseif (os.date("%A")== "Saturday") or (os.date("%A")== "Sunday") then
	SKIN:Bang('[!SetVariable class1 "No school"]')
	SKIN:Bang('[!SetOption MeterString FontColor 255,0,0,255]')
	return "0:00:00"
	else
	days=0
	end
	stats= tonumber(SKIN:GetVariable('state'))
	if (stats==1) then
	SKIN:Bang('[!SetOption Title FontColor 60,60,255,255]')
	days=5
	elseif (stats==0)then
	elseif (stats==2) then
	SKIN:Bang('[!SetVariable class1 "Switch On"]')
	SKIN:Bang('[!SetOption MeterString FontColor 255,0,0,255]')
	SKIN:Bang('[!SetOption Title FontColor 255,255,255,255]')
	return "OFF"
	else
	SKIN:Bang('[!WriteKeyValue Variables State "0"][!SetVariable State "0"]')
	SKIN:Bang('[!SetOption Title FontColor 255,255,255,255]')
	end
	while a[i] do
	Hours= os.date("%H")
	Mins = os.date("%M")
	Secs = os.date("%S")
	totalt= ((Hours*60)+Mins)*60+Secs
	totalm= ((a[i][1+days]*60)+a[i][2+days])*60+offset
	totalm= totalm-totalt
	if totalm>0 then
	break
	end
	i=i+1
	end
	if i>16 then
	SKIN:Bang('[!SetVariable class1 "End"]')
	totalm=-10
	else
	SKIN:Bang('[!SetVariable class1 "'..a[i][5]..'"]')
	end
	if totalm<901 then
	blue=0
	if totalm<301 then
	green=0
	else
	green=121
	end
	else
	blue=255
	green=255
	end
	SKIN:Bang('[!SetOption MeterString FontColor 255,'..green..','..blue..','..flash..']')
	if totalm<0 then
	totalm=0
	output="0:00:00"
	flash=255-flash
	else
	flash=255
	output=math.floor(totalm/3600)..":"..os.date("%M",totalm)..":"..os.date("%S",totalm)
	end
	return output
end