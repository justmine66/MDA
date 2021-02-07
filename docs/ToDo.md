1. 所有方法增加参数、异常、形成闭环。
2. 超过MaxAge的聚合根，Checkpoint逻辑。
3. 聚合根内存上限(物理内存的80%)：MaxSize的性能测试。
4. ObjectPortMapper支持可空类型转换，比如：decimal => decimal?
5. 状态后端可靠性保障。
6. 解决Newtonsoft序列化带多个构造函数类的问题。
7. 领域事件持久化索引和载荷保证事务性。