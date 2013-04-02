VB.NET Xbox API Wrapper
===================

Version: 1.0.0.1

A compact, asynchronous wrapper for the [XboxLeaders.com Xbox API](http://www.xboxleaders.com/docs)

License
=======

The XboxLeaders API Wrapper is licensed under [Creative Commons Attribution-ShareAlike 3.0](http://creativecommons.org/licenses/by-sa/3.0/)

Installation
============

Add `XboxLeaders.vb` to an existing project, and `Import` it in the header.


        Dim xblApi As New XboxLeaders
        xblApi.Format = XboxLeaders.FormatType.XML
        MsgBox(xblApi.FetchProfile("nvpdude"))
        

Methods
=======

`xblApi.FetchProfile("gamertag")` Returns all data associated with the requested gamertag such as Gamerscore, Reputation, and Biography.

`xblApi.FetchGames("gamertag")` Returns all played games and associated data related to those games, except individual achievements.

`xblApi.FetchAchievements("gamertag", "gameid")` Returns all achievements and their associated data for the requested gamertag and game.

`xblApi.FetchFriends("gamertag")` Returns a list of all of the requested gamertags' friends, and their associated data.