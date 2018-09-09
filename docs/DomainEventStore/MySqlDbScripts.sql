CREATE DATABASE `MDA`;

USE `MDA`;

CREATE TABLE `DomainEventStream` (
  `EventId` varchar(36) NOT NULL,
  `EventBody` varchar(4000) NOT NULL,
  `EventSequence` int(11) NOT NULL,
  `CommandId` varchar(36) NOT NULL,
  `AggregateRootId` varchar(36) NOT NULL,
  `AggregateRootTypeName` varchar(256) NOT NULL,
  `OccurredOn` datetime NOT NULL,
  UNIQUE KEY `IX_DomainEventStream_AggId_Version` (`AggregateRootId`,`EventId`,`EventSequence`),
  UNIQUE KEY `IX_DomainEventStream_AggId_CommandId` (`AggregateRootId`,`CommandId`)
)ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;