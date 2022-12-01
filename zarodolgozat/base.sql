-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2022. Nov 24. 11:16
-- Kiszolgáló verziója: 10.4.25-MariaDB
-- PHP verzió: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `colorball`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `palya`
--

CREATE TABLE `palya` (
  `palya_id` int(11) NOT NULL,
  `palya_name` varchar(20) COLLATE utf8_hungarian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

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

CREATE TABLE `player` (
  `player_id` int(11) NOT NULL,
  `player_name` varchar(20) COLLATE utf8_hungarian_ci NOT NULL,
  `player_join_date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `player`
--

INSERT INTO `player` (`player_id`, `player_name`, `player_join_date`) VALUES
(1, 'bance23', '2022-11-23'),
(2, 'Playmaker1210', '2022-11-24'),
(11, 'undefined', '2022-11-24');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `score`
--

CREATE TABLE `score` (
  `score_id` int(11) NOT NULL,
  `score_playerid` int(11) NOT NULL,
  `score_palyaid` int(11) NOT NULL,
  `score_points` int(11) NOT NULL,
  `score_date` date NOT NULL,
  `score_time` time NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `score`
--

INSERT INTO `score` (`score_id`, `score_playerid`, `score_palyaid`, `score_points`, `score_date`, `score_time`) VALUES
(1, 1, 1, 275, '2022-11-23', '00:01:15'),
(2, 2, 2, 350, '2022-11-24', '00:02:32'),
(3, 1, 2, 325, '2022-11-24', '00:02:21'),
(4, 2, 1, 300, '2022-11-24', '00:01:45'),
(5, 1, 2, 213, '2022-11-02', '05:51:12');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `palya`
--
ALTER TABLE `palya`
  ADD PRIMARY KEY (`palya_id`);

--
-- A tábla indexei `player`
--
ALTER TABLE `player`
  ADD PRIMARY KEY (`player_id`);

--
-- A tábla indexei `score`
--
ALTER TABLE `score`
  ADD PRIMARY KEY (`score_id`),
  ADD KEY `score_playerid` (`score_playerid`),
  ADD KEY `score_palyaid` (`score_palyaid`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `palya`
--
ALTER TABLE `palya`
  MODIFY `palya_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT a táblához `player`
--
ALTER TABLE `player`
  MODIFY `player_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT a táblához `score`
--
ALTER TABLE `score`
  MODIFY `score_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `score`
--
ALTER TABLE `score`
  ADD CONSTRAINT `score_ibfk_1` FOREIGN KEY (`score_playerid`) REFERENCES `player` (`player_id`),
  ADD CONSTRAINT `score_ibfk_2` FOREIGN KEY (`score_palyaid`) REFERENCES `palya` (`palya_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
