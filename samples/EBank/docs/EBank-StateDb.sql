Create Database If Not Exists `ebank_statedb` Character Set UTF8;

USE `ebank_statedb`;

DROP TABLE `aggregate_root_checkpoint_indices`;
DROP TABLE `aggregate_root_checkpoints`;
DROP TABLE `domain_event_indices`;
DROP TABLE `domain_events`;

CREATE TABLE `aggregate_root_checkpoint_indices`
(
  `pkId` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增长标识',
  `AggregateRootId` varchar(36) DEFAULT '' NOT NULL COMMENT '聚合根标识',
  `AggregateRootType` varchar(256) DEFAULT '' NOT NULL COMMENT '聚合根类型完全限定名',
  `AggregateRootVersion` int(11) DEFAULT 0 NOT NULL COMMENT '聚合根版本号',
  `AggregateRootGeneration` int(11) DEFAULT 0 NOT NULL COMMENT '聚合根生命周期代数',
  `CreatedTimestamp` bigint(20) DEFAULT 0 NOT NULL COMMENT '创建时间，时间戳，单位：毫秒',
  PRIMARY KEY (`pkId`),
  KEY `IX_AggregateRoot_Version` (`AggregateRootId`,`AggregateRootVersion`,`AggregateRootGeneration`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='聚合根检查点索引表';

CREATE TABLE `aggregate_root_checkpoints`
(
  `pkId` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增长标识',
  `AggregateRootId` varchar(36) DEFAULT '' NOT NULL COMMENT '聚合根标识',
  `Payload` BLOB NOT NULL COMMENT '有效载荷',
  PRIMARY KEY (`pkId`),
  KEY `IX_AggregateRootId` (`AggregateRootId`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='聚合根检查点有效载荷表';

CREATE TABLE `domain_event_indices`
(
  `pkId` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增长标识',
  `DomainCommandId` varchar(36) DEFAULT '' NOT NULL COMMENT '领域命令标识',
  `DomainCommandType` varchar(256) DEFAULT '' NOT NULL COMMENT '领域命令类型完全限定名',
  `DomainCommandVersion` int(11) DEFAULT 0 NOT NULL COMMENT '领域命令版本号',
  `AggregateRootId` varchar(36) DEFAULT '' NOT NULL COMMENT '聚合根标识',
  `AggregateRootType` varchar(256) DEFAULT '' NOT NULL COMMENT '聚合根类型完全限定名',
  `AggregateRootVersion` int(11) DEFAULT 0 NOT NULL COMMENT '聚合根版本号',
  `AggregateRootGeneration` int(11) DEFAULT 0 NOT NULL COMMENT '聚合根生命周期代数',
  `DomainEventId` varchar(36) DEFAULT '' NOT NULL COMMENT '领域事件标识',
  `DomainEventType` varchar(256) DEFAULT '' NOT NULL COMMENT '领域事件类型完全限定名',
  `DomainEventVersion` int(11) DEFAULT 0 NOT NULL COMMENT '领域事件版本号',
  `DomainEventPayloadBytes` int(11) DEFAULT 0 NOT NULL COMMENT '领域事件载荷容量，单位：字节',
  `CreatedTimestamp` bigint(20) DEFAULT 0 NOT NULL COMMENT '创建时间，时间戳，单位：毫秒',
  PRIMARY KEY (`pkId`),
  UNIQUE KEY `IX_Command_AggregateRoot_Event` (`DomainCommandId`,`AggregateRootId`,`DomainEventId`),
  KEY `IX_AggregateRoot_Generation_Version` (`AggregateRootId`,`AggregateRootGeneration`,`AggregateRootVersion`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='领域事件索引表';

CREATE TABLE `domain_events`
(
  `pkId` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增长标识',
  `DomainEventId` varchar(36) DEFAULT '' NOT NULL COMMENT '领域事件标识',
  `Payload` BLOB NOT NULL COMMENT '有效载荷',
  PRIMARY KEY (`pkId`),
  UNIQUE KEY `IX_DomainEventId` (`DomainEventId`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='领域事件有效载荷表';