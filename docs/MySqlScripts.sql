CREATE DATABASE `MDA`;

USE `MDA`;

CREATE TABLE `DomainEventStream` (
  `EventId` varchar(36) NOT NULL,
  `EventVersion` int(11) NOT NULL,
  `CommandId` varchar(36) NOT NULL,
  `AggregateRootId` varchar(36) NOT NULL,
  `AggregateRootTypeName` varchar(256) NOT NULL,
  `OccurredOn` datetime NOT NULL,
  UNIQUE KEY `IX_EventStream_AggId_Version` (`AggregateRootId`,`EventVersion`),
  UNIQUE KEY `IX_EventStream_AggId_CommandId` (`AggregateRootId`,`CommandId`)
)ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;