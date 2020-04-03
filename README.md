KindaTerribleTimeTracker
=============

A .Net core application written and tested under linux that helps employees keep track of working hours.


It stores it's data into a sqlite database and updates it every 10 minutes, so sudden reboots won't cause major data loss.


ToDo:
* Export to file function
* UI


To build, execute 

	dotnet build KindaTerribleTimeTracker.sln


Example for application output of a run:

	./presenceApp 
	 Using culture en-US and DB connection Data Source=presence.db;Version=3;.
	Date: Wed 01.04.2020 Week: 14 Start Time 8:31:22 AM, End Time 4:21:48 PM
	Date: Thu 02.04.2020 Week: 14 Start Time 9:01:02 AM, End Time 3:50:22 PM
	Press Enter to exit...

Pressing enter will stop time tracking and will close the application.

To change the date/time format, modify the appsettings.json "Culture"-variable.

