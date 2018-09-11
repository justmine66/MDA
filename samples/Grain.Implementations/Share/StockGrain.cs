using Grain.interfaces.Share;
using Orleans;
using System.Net.Http;
using System.Threading.Tasks;

namespace Grain.Implementations.Share
{
    public class StockGrain : Orleans.Grain, IStockGrain
    {
        string price;

        public override async Task OnActivateAsync()
        {
            string stock;
            this.GetPrimaryKey(out stock);
            await UpdatePrice(stock);

            await base.OnActivateAsync();
        }

        public Task<string> GetPrice()
        {
            return Task.FromResult(price);
        }

        async Task UpdatePrice(string stock)
        {
            price = await GetPriceFromYahoo(stock);
        }

        async Task<string> GetPriceFromYahoo(string stock)
        {
            var uri = "http://download.finance.yahoo.com/d/quotes.csv?f=snl1c1p2&e=.csv&s=" + stock;

            using (var http = new HttpClient())
            using (var resp = await http.GetAsync(uri))
            {
                return await resp.Content.ReadAsStringAsync();
            }
        }
    }
}
