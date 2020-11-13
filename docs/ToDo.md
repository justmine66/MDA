1. 领域事件发布到消息总线的兜底方案。
2. 聚合根支持Checkpoint机制。
3. 针对kafka的消息分区键的完整测试。
4. 所有方法增加参数、异常、形成闭环。
5. 检查所有的查询，优化表索引。
6. ITimerTask支持完全异步。
7. checkpoint后，代数未增加。
8. 超过MaxAge的聚合根，Checkpoint逻辑。
9. LRU Cache支持TTL。
10. 测试聚合根变更后序列化问题。