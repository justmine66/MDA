using Grain.interfaces.DeclarativePersistence;
using Orleans;
using System;
using System.Threading.Tasks;

namespace OrleansClient
{
    public class DeclarativePersistenceClient
    {
        public static async Task Run(IClusterClient client)
        {
            var ids = new string[] {
        "42783519-d64e-44c9-9c29-399e3afaa625",
        "d694a4e0-1bc3-4c3f-a1ad-ba95103622bc",
        "9a72b0c6-33df-49db-ac05-14316edd332d",
        "6526a751-b9ac-4881-9bfb-836ecce2ca9f",
        "ae4b106f-3c96-464a-b48d-3583ed584b17",
        "b715c40f-d8d2-424d-9618-76afbc0a2a0a",
        "5ad92744-a0b1-487b-a9e7-e6b91e9a9826",
        "e23a55af-217c-4d76-8221-c2b447bf04c8",
        "2eef0ac5-540f-4421-b9a9-79d89400f7ab"
    };

            var e0 = client.GetGrain<IEmployee>(Guid.Parse(ids[0]));
            var e1 = client.GetGrain<IEmployee>(Guid.Parse(ids[1]));
            var e2 = client.GetGrain<IEmployee>(Guid.Parse(ids[2]));
            var e3 = client.GetGrain<IEmployee>(Guid.Parse(ids[3]));
            var e4 = client.GetGrain<IEmployee>(Guid.Parse(ids[4]));

            var m0 = client.GetGrain<IManager>(Guid.Parse(ids[5]));
            var m1 = client.GetGrain<IManager>(Guid.Parse(ids[6]));

            m0.AddDirectReport(e0).Wait();
            m0.AddDirectReport(e1).Wait();
            m0.AddDirectReport(e2).Wait();

            m1.AddDirectReport(await m0.AsEmployee()).Wait();
            m1.AddDirectReport(e3).Wait();

            m1.AddDirectReport(e4).Wait();
        }
    }
}
