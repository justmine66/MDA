CREATE DATABASE `MDA`;

USE `MDA`;

CREATE TABLE `EventStream` (
  `EventId` bigint(20) NOT NULL AUTO_INCREMENT,
  `TypeName` varchar(256) NOT NULL,
  `OccurredOn` datetime NOT NULL,
  `EventBody` varchar(4000) NOT NULL,
  PRIMARY KEY (`EventId`),
)ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `LockKey` (
  `Name` varchar(128) NOT NULL,
  PRIMARY KEY (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;