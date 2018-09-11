using Grain.interfaces.Share;
using Orleans;
using System;
using System.Threading.Tasks;

namespace OrleansClient
{
    public class StockClient
    {
        public static async Task Run(IClusterClient client)
        {
            var stock = client.GetGrain<IStockGrain>("MSFT");
            var price = await stock.GetPrice();
            Console.WriteLine(price);
        }
    }
}
