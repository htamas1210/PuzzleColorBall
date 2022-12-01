-- phpMyAdmin SQL Dump
-- version 4.9.0.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2022. Nov 24. 09:31
-- Kiszolgáló verziója: 10.4.6-MariaDB
-- PHP verzió: 7.3.8

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;



-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `palya`
--

CREATE TABLE IF NOT EXISTS `palya` (
  `palya_id` int(11) NOT NULL AUTO_INCREMENT,
  `palya_name` varchar(20) COLLATE utf8_hungarian_ci NOT NULL,
  PRIMARY KEY (`palya_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `palya`
--

INSERT INTO `palya` (`palya_id`, `palya_name`) VALUES
(1, 'Easy1'),
(2, 'Normal2');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `player`
--

CREATE TABLE IF NOT EXISTS `player` (
  `player_id` int(11) NOT NULL AUTO_INCREMENT,
  `player_name` varchar(20) COLLATE utf8_hungarian_ci NOT NULL,
  `player_join_date` date NOT NULL,
  PRIMARY KEY (`player_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `player`
--

INSERT INTO `player` (`player_id`, `player_name`, `player_join_date`) VALUES
(1, 'bance23', '2022-11-23'),
(2, 'PlayMaker1210', '2022-11-24');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `scores`
--

CREATE TABLE IF NOT EXISTS `scores` (
  `score_id` int(11) NOT NULL AUTO_INCREMENT,
  `score_playerid` int(11) NOT NULL,
  `score_palyaid` int(11) NOT NULL,
  `score_points` int(11) NOT NULL,
  `score_date` date NOT NULL,
  `score_time` time NOT NULL,
  PRIMARY KEY (`score_id`),
  KEY `score_playerid` (`score_playerid`),
  KEY `score_palyaid` (`score_palyaid`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `scores`
--

INSERT INTO `scores` (`score_id`, `score_playerid`, `score_palyaid`, `score_points`, `score_date`, `score_time`) VALUES
(1, 1, 1, 275, '2022-11-23', '00:01:15'),
(2, 2, 2, 350, '2022-11-24', '00:02:32'),
(3, 1, 2, 325, '2022-11-24', '00:02:21'),
(4, 2, 1, 300, '2022-11-24', '00:01:45');

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `scores`
--
ALTER TABLE `scores`
  ADD CONSTRAINT `scores_ibfk_1` FOREIGN KEY (`score_playerid`) REFERENCES `player` (`player_id`),
  ADD CONSTRAINT `scores_ibfk_2` FOREIGN KEY (`score_palyaid`) REFERENCES `palya` (`palya_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
