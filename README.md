You Poor Bastard!
=============

The combination of a delicious lunch, computer white noise, and a cube that's just a touch too warm has you all prepped and ready for a nap, when your project manager shows up at the side of your chair with a request you'd rather not hear...

"Bob found a bug in the blabity blah module, can you fix it before you leave?  He mentioned that you'd be able to find the source code in, what did he call it... that "source server" database thinger you guys used to use".

Oh yeah, source safe. It gets better though; no one at the company was around when that code was written, and to top it off the last person who had working credentials to access the code retired 50 years ago.

You. Poor. Bastard.

This app won't fix your headache, but at least it will help you get into the source safe database so you can migrate code to something a bit more sensible...

Features
========

1. Crack the passwords of all users.
2. Crack the password of a single, specified user.

Examples
========

To get help

    C:\InstallationPathHere .\YouPoorBastard.exe -h

To crack the password of a single user

    C:\InstallationPathHere .\YouPoorBastard.exe "C:\SomeDataFolder\YourVSSDirectory" -u johndoe

To crack the password of all users in the source safe database

    C:\InstallationPathHere .\YouPoorBastard.exe "C:\SomeDataFolder\YourVSSDirectory"

Installation
============

1. Download the source code for the project to your workstation.
2. Compile the project in RELEASE mode.
3. Put the build output wherever you want to run it from.

NOTE: If you leave the solution in DEBUG mode, it will have a few annying pauses after performing operations. If you build with the RELEASE configuration you won't have this problem.

Roadmap
=======

1. Add support to export data to a flat file.