CREATE DATABASE `MDA`;

USE `MDA`;

CREATE TABLE `mda_domain_event_log_headers` (
  `id` int(11) NOT NULL,
  `business_principal_id` varchar(36) NOT NULL,
  `command_id` varchar(36) NOT NULL,
  `command_time` varchar(36) NOT NULL,
  `event_id` varchar(36) NOT NULL,
  `event_sequence` int(11) NOT NULL,
  `event_time` datetime NOT NULL,
  UNIQUE KEY `IX_Business_Command_Version` (`id`,`business_principal_id`,`command_id`),
  UNIQUE KEY `IX_DomainEventStream_AggId_CommandId` (`AggregateRootId`,`CommandId`)
)ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `mda_domain_event_log_payloads` (
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