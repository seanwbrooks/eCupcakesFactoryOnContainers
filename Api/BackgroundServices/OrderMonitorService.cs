using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using SignalRDemo.Hubs;

namespace Api.BackgroundServices{
    public class OrderMonitorService : BackgroundService
    {
        private IHubContext<OrderMonitorHub, IOrder> _orderMonitorHub;

        public OrderMonitorService(IHubContext<OrderMonitorHub, IOrder> orderMonitorHub) => this._orderMonitorHub = orderMonitorHub;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"OrderMonitoring service is running at: {DateTime.Now}");

            while (!stoppingToken.IsCancellationRequested)
            {

                //Step 1: If there is a new message in KAFKA "Orders" topic, inform the client.
                Order orderRequest = new Order(){ Key = "Test",Value="Test"};
                await _orderMonitorHub.Clients.All.InformApps(orderRequest);
                
                //Wait for 
                await Task.Delay(1000);
            }
        }
    }
}