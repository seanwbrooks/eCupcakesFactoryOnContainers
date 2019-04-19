using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Api.BackgroundServices{
    public class OrderMonitorService : BackgroundService
    {
        protected OrderMonitorService()
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"OrderMonitoring service is running at: {DateTime.Now}");
            throw new System.NotImplementedException();
        }
    }
}