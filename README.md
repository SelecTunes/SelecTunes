[![pipeline status](https://git.linux.iastate.edu/cs309/spring-2020/JR_2/badges/master/pipeline.svg)](https://git.linux.iastate.edu/cs309/spring-2020/JR_2/commits/master)
[![License: MIT](https://img.shields.io/badge/license-Apache%202.0-blue)](https://git.linux.iastate.edu/cs309/spring-2020/JR_2/blob/master/LICENSE)
# SelecTunes 309 Project

For this project, we have decided to make an application called SelecTunes. Have you ever been at a party where you don't like the music? Or maybe there were gaps between songs and no one can agree? Well now you have the ability to queue your favorite songs easily! With a quick search you can add your choice of song to liven up the party.

## Features for party hosts and partyers

### For Both Hosts and Guests
* Searching of any song in the Spotify database to add to the party
* Adding songs to the queue for the whole party to listen
* Upvoting and downvoting songs that you like or dislike
  * Enough upvotes locks the song into the queue at the top
  * Enough downvotes removes the song from the queue
* Seeing all guests at the party
* Chat room to talk to other party guests

### For the Party Hosts
* Ability to monitor who is in the party, with the ability to ban by phone number
* Remove songs from the queue that would be considered "memes"
* Kick people who are potentially messing up the queue or being a general nuisance
* Approval or denial of "explicit" songs depending on the event

## Installation and Setup

### Back End (Windows)
* Download [dotnet core](https://dotnet.microsoft.com/download/dotnet-core/3.1) onto your system
* Using Visual Studio
  * Open the Solution file in VS
  * //POWERSHELL COMMANDS TOTO
* Using Powershell
  * //WINDOWS IS HARD

### Back End (Mac/Linux)
* Download [dotnet core](https://dotnet.microsoft.com/download/dotnet-core/3.1) onto your system
* Clone the repository
* Move to the directory of the .sln file
* `dotnet-ef update database` to update the database with the correct schema
* `dotnet run` to start the development server

### Front End
* Download Open JDK 11 (target jdk)
* Open the app in Android Studio
* Run with corresponding run configuration

