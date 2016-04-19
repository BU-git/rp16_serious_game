<img src="http://memodigital.ru/upload/images/20121221174004fb5c81ed3a220004b71069645f112867_w974_h300_m5.png" width="auto" height="auto" max-height="100px">
# GrowPad Serious Game
Part of Rotterdam University of Applied Sciences and Bionic University 16' joint project.

---

The project is intended to help people living in deprived areas of Rotterdam. <br/>
It is implementing GrowPath concept together with gamification principles to make it suitable for both kids and adults.<br/> 

---

List of technologies and frameworks used:
- ASP.NET MVC 6
- Semantic UI
- ReactJS.NET
- ES6
- Entity Framework 7

List of tools used:
- Gulp
- Dnx451
- IIS 10
- Npm
- Bower
- NuGet

---

#####To run this project, follow next steps:
```
git clone git@github.com:BU-git/rp16_serious_game.git
cd rp16_serious_game
dnu restore
cd src/WebUI
dnx ef database update -p DAL
cd ../..
dnu restore
dnu pack src/*
cd src/WebUI
dnx web
```


