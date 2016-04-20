<img src="http://memodigital.ru/upload/images/20121221174004fb5c81ed3a220004b71069645f112867_w974_h300_m5.png" width="auto" height="auto" max-height="100px">
# GrowPad Serious Game
Part of Rotterdam University of Applied Sciences and Bionic University 16' joint project.

---

The project is intended to help people living in deprived areas of Rotterdam. <br/>
It is implementing GrowPath concept together with gamification principles to make it suitable for both kids and adults.<br/>
Due to the fact that such families live in constant stress, they are unable to make long-term
plans and fight their way out of their deprived situation.<br/>
So, the main goal of the application is to help people to improve their life by developing healthy life habits via completing simple tasks one at a time under coach supervision.<br/>

---

Using the application you can:
- Assign tasks to the supervised families;
- Control the progress of each task;
- Personalize an approach to every family;
- Make difficult process of getting back on track much easier.

---

List of technologies and frameworks used:
- .NET Framework 4.5.1
- ASP.NET MVC 6
- Entity Framework 7
- ASP.NET Identity 2.0
- ES6 (JS)
- ReactJS.NET
- Semantic UI

List of tools used:
- Kestrel
- Npm
- Bower
- NuGet
- Gulp

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


