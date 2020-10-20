Create Database If Not Exists `mda` Character Set UTF8;

USE `mda`;

CREATE TABLE `domain_event_indices`
(
  `pkId` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增长标识',
  `DomainCommandId` varchar(36) DEFAULT '' NOT NULL COMMENT '领域命令标识',
  `DomainCommandType` varchar(256) DEFAULT '' NOT NULL COMMENT '领域命令类型完全限定名',
  `DomainCommandVersion` bigint(20) DEFAULT 0 NOT NULL COMMENT '领域命令版本号',
  `AggregateRootId` varchar(36) DEFAULT '' NOT NULL COMMENT '聚合根标识',
  `AggregateRootType` varchar(256) DEFAULT '' NOT NULL COMMENT '聚合根类型完全限定名',
  `AggregateRootGeneration` int(11) DEFAULT 0 NOT NULL COMMENT '第几代聚合根,随着检查点(Checkpoint)快照而递增.',
  `AggregateRootVersion` bigint(20) DEFAULT 0 NOT NULL COMMENT '聚合根版本',
  `DomainEventId` varchar(36) DEFAULT '' NOT NULL COMMENT '领域事件标识',
  `DomainEventType` varchar(256) DEFAULT '' NOT NULL COMMENT '领域事件类型完全限定名',
  `DomainEventVersion` bigint(20) DEFAULT 0 NOT NULL COMMENT '领域事件版本号',
  `CreatedTimestamp` bigint(20) DEFAULT 0 NOT NULL COMMENT '创建时间，时间戳，单位：毫秒',
  PRIMARY KEY (`pkId`),
  UNIQUE KEY `IX_Command_AggregateRoot_Event` (`DomainCommandId`,`AggregateRootId`,`DomainEventId`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='领域事件索引表';

CREATE TABLE `domain_event_payloads`
(
  `pkId` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增长标识',
  `DomainEventId` varchar(36) DEFAULT '' NOT NULL COMMENT '领域事件标识',
  `DomainEventVersion` bigint(20) DEFAULT 0 NOT NULL COMMENT '领域事件版本号',
  `Payload` BLOB NOT NULL,
  PRIMARY KEY (`pkId`),
  UNIQUE KEY `IX_DomainEventId` (`DomainEventId`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='领域事件有效载荷表';