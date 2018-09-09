CREATE DATABASE [MDA]
GO
USE [MDA]
GO

CREATE TABLE [dbo].[DomainEventStream] (
  [EventId] NVARCHAR(36) NOT NULL,
  [EventBody] NVARCHAR(4000) NOT NULL,
  [EventSequence] INT NOT NULL,
  [CommandId] NVARCHAR(36) NOT NULL,
  [AggregateRootId] NVARCHAR(36) NOT NULL,
  [AggregateRootTypeName] NVARCHAR(256) NOT NULL,
  [OccurredOn] DATETIME NOT NULL
)
GO
CREATE UNIQUE INDEX [IX_DomainEventStream_AggId_Version]   ON [dbo].[DomainEventStream] ([AggregateRootId] ASC, [EventId] ASC, [EventSequence] ASC)
GO
CREATE UNIQUE INDEX [IX_DomainEventStream_AggId_CommandId] ON [dbo].[DomainEventStream] ([AggregateRootId] ASC, [CommandId] ASC)