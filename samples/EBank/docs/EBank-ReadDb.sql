Create Database If Not Exists `ebank_readdb` Character Set UTF8;

USE `ebank_readdb`;

CREATE TABLE `bank_accounts`
(
  `pkId` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增长标识',
  `Id` bigint(20) DEFAULT 0 NOT NULL COMMENT '账号',
  `Name` varchar(32) DEFAULT '' NOT NULL COMMENT '账户名',
  `Bank` varchar(64) DEFAULT '' NOT NULL COMMENT '开户行',
  `Balance` DECIMAL(18,2) DEFAULT 0 NOT NULL COMMENT '余额',
  `Creator` varchar(36) DEFAULT '' NOT NULL COMMENT '创建者',
  `CreatedTimestamp` bigint(20) DEFAULT 0 NOT NULL COMMENT '创建时间，时间戳，单位：毫秒',
  PRIMARY KEY (`pkId`),
  UNIQUE KEY `IX_Account_Id` (`Id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='银行账户表';

CREATE TABLE `deposit_transactions`
(
  `pkId` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增长标识',
  `Id` bigint(20) DEFAULT 0 NOT NULL COMMENT '交易号',
  `AccountId` bigint(20) DEFAULT 0 NOT NULL COMMENT '账号',
  `AccountName` varchar(32) DEFAULT '' NOT NULL COMMENT '账户名',
  `Bank` varchar(64) DEFAULT '' NOT NULL COMMENT '开户行',
  `amount` DECIMAL(18,2) DEFAULT 0 NOT NULL COMMENT '金额',
  `Status` varchar(16) DEFAULT '' NOT NULL COMMENT '状态',
  `Creator` varchar(36) DEFAULT '' NOT NULL COMMENT '创建者',
  `CreatedTimestamp` bigint(20) DEFAULT 0 NOT NULL COMMENT '创建时间，时间戳，单位：毫秒',
  PRIMARY KEY (`pkId`),
  UNIQUE KEY `IX_Account_Id` (`Id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='存款交易表';