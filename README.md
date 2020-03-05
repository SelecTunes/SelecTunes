[![pipeline status](https://git.linux.iastate.edu/cs309/spring-2020/JR_2/badges/master/pipeline.svg)](https://git.linux.iastate.edu/cs309/spring-2020/JR_2/commits/master)

# SelecTunes 309 Project

For this project, we have decided to make an application called SelecTunes. Have you ever been at a party where you don't like the music? Or maybe there were gaps between songs and no one can agree? Well now you have the ability to queue your favorite songs easily! With a quick search you can add your choice of song to liven up the party.

## Features for party hosts and partyers

### For the Party Hosts
* Ability to monitor who is in the party, with the ability to ban by phone number
* Remove songs from the queue that would be considered "memes"
* Kick people who are potentially messing up the queue or being a general nuisance
//TODO

### For the Party Guests
* Easy join code
* Anynomous join to party, only need phone number (not visible to anyone) and a desired username
//TODO

## Installation and Setup

### Back End (Windows)
* //TODO

### Back End (Mac/Linux)
* Download [dotnet core](https://dotnet.microsoft.com/download/dotnet-core/3.1) onto your system
* Clone the repository
* Move to the directory of the .sln file
* `dotnet-ef update database` to update the database with the correct schema
* `dotnet run` to start the development server
