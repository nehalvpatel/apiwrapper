Xbox API Wrapper
===================

Version: 1.0

A simple PHP wrapper for the [XboxLeaders.com Xbox API](http://www.xboxleaders.com/docs)


License
=======

The XboxLeaders API Wrapper is licensed under [Creative Commons Attribution-ShareAlike 3.0](http://creativecommons.org/licenses/by-sa/3.0/)


Requirements
============

* PHP 5.2+
* [cURL](http://php.net/curl) PHP Extension
* [JSON](http://php.net/json) PHP Extension
* [SimpleXML](http://php.net/simplexml) PHP Extension
* [PHP Serialize/Unserialize](http://php.net/unserialize)


Installation
============

Simply include the `/lib/wrapper.php` file into whatever script you are going to be using the API for, and instantiate the `XboxApi` class.


        <?php
        require 'wrapper.php';
        
        $api = new XboxApi();
        $api->format = 'json';
        $api->timeout = 6;
        
        $games = $api->fetch_games('Major Nelson');


Methods
=======

`$this->fetch_profile($gamertag);` Returns all data associated with the requested gamertag such as Gamerscore, Reputation, and Biography.

`$this->fetch_games($gamertag);` Returns all played games and associated data related to those games, except individual achievements.

`$this->fetch_achievements($gamertag, $gameid);` Returns all achievements and their associated data for the requested gamertag and game.

`$this->fetch_friends($gamertag);` Returns a list of all of the requested gamertags' friends, and their associated data.