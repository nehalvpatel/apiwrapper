<?php
/**
 * wrapper.php
 *
 * @package XboxLeaders API Wrapper 1.0
 * @author Jason Clemons <me@jasonclemons.me>
 * @version 1.0
 */

class XboxApi {

	public $endpoint = 'https://www.xboxleaders.com/api/';
	public $timeout = 8;
	public $format = 'json'; //Can be json, xml, or php

	public function __construct() {
		if ($this->format == 'json') {
			if (!function_exists('json_decode')) {
				throw new Exception('The JSON library could not be loaded, and is needed for the JSON format.');
			}
		} else if ($this->format == 'xml') {
			if (!function_exists('simplexml_load_string')) {
				throw new Exception('The SimpleXML library could not be loaded, and is needed for the XML format.');
			}
		} else if ($this->format == 'php') {
			if (!function_exists('unserialize')) {
				throw new Exception('Could not unserialize data.');
			}
		}
	}

	private function output($data) {
		switch ($this->format) {
			case 'xml':
				return simplexml_load_string($data);
				break;
			case 'json':
				return json_decode($data);
				break;
			case 'php':
				return unserialize($data);
				break;
		}

		return false;
	}

	private function http($request, $parameters = array()) {
		$url = $this->endpoint . '.' . $this->format . (!empty($parameters)) ? '?' . http_build_query($parameters, null, '&') : '';
		$ch = curl_init();
		curl_setopt($ch, CURLOPT_URL, $url);
		curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
		curl_setopt($ch, CURLOPT_FOLLOWLOCATION, true);
		curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
		curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);
		curl_setopt($ch, CURLOPT_AUTOREFERER, true);
		curl_setopt($ch, CURLOPT_TIMEOUT, isset($this->timeout) ? $this->timeout : 8);
		curl_setopt($ch, CURLOPT_CONNECTTIMEOUT, 15);

		$result = curl_exec($ch);

		return $this->output($result);
	}

	/** Validate Gamertag **/
	private function valid_gamertag($gamertag) {
		if (!preg_match('~^(?=.{1,15}$)[a-zA-Z][a-zA-Z[0-9]]*(?: [a-zA-Z[0-9]]+)*$~', $gamertag)) {
			return true;
		}

		return false;
	}

	/** Fetch Profile Data **/
	public function fetch_profile($gamertag) {
		if ($this->valid_gamertag($gamertag)) {
			$parameters = array('gamertag' => $gamertag);
			return $this->http('profile', $gamertag);
		}

		return false;
	}

	/** Fetch List Of Played Games **/
	public function fetch_games($gamertag) {
		if ($this->valid_gamertag($gamertag) {
			$parameters = array('gamertag' => $gamertag);
			return $this->http('games', $gamertag);
		}

		return false;
	}

	/** Fetch Achievement Data **/
	public function fetch_achievements($gamertag, $gameid) {
		if ($this->valid_gamertag($gamertag) {
			$parameters = array('gamertag' => $gamertag, 'gameid' => $gameid);
			return $this->http('achievements', $parameters);
		}

		return false;
	}

	/** Fetch Friends List **/
	public function fetch_friends($gamertag) {
		if ($this->valid_gamertag($gamertag) {
			$parameters = array('gamertag' => $gamertag);
			return $this->http('friends', $gamertag);
		}

		return false;
	}

}