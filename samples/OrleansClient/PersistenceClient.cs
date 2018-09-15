using Grain.interfaces.Persistence;
using Orleans;
using Orleans.Storage;
using System;
using System.Threading.Tasks;

namespace OrleansClient
{
    public class PersistenceClient
    {
        public static async Task Run(IClusterClient client)
        {
            var grain = client.GetGrain<IMyPersistenceGrain>(Guid.NewGuid());

            try
            {
                await grain.DoWrite(10);
                Console.WriteLine("设置状态：" + 10);
            }
            catch (InconsistentStateException e)
            {
                Console.WriteLine("写状态不一致异常");
            }
            catch (Exception e)
            {
                Console.WriteLine("写发生了一个未知异常");
            }

            try
            {
                var state = await grain.DoRead();
                Console.WriteLine("获取到状态：" + state);
            }
            catch (BadProviderConfigException e)
            {
                Console.WriteLine("读发生了一个存储异常");
            }
            catch (Exception e)
            {
                Console.WriteLine("读发生了一个未知异常");
            }
        }
    }
}
