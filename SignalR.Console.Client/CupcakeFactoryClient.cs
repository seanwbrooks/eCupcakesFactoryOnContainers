using System;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.SignalR.Client;
namespace CupcakeFactory.Clients
{
    public class CupcakeFactoryClient: BackgroundService
    {
        private HubConnection _connection;

        public CupcakeFactoryClient()
        {
            _connection = new HubConnectionBuilder()
                            .WithUrl("http://localhost:5000/ordermonitorhub")
                            .Build();

            _connection.On<OrderRequest>("InformNewOrderToMix", 
                        o => _ = RecieveOrderNotification(o));           
        }

       
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"OrderMonitoring service is running at: {DateTime.Now}");

            //while(true){
                try
                        {
                            await _connection.StartAsync(stoppingToken);

                           // break;
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine("Error while starting a connect with HUB"+ ex.InnerException.Message);
                            await Task.Delay(1000);
                        }
            //}

            // Loop is here to wait until the server is running
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
            }
        }
        public  override Task StopAsync(CancellationToken cancellationToken)
        {
            return _connection.DisposeAsync();
        }

        public  Task RecieveOrderNotification(OrderRequest o){
            Console.WriteLine("================================");
            Console.WriteLine("Recieved a request order !!!!!!");
            Console.WriteLine($"Flavour:{o.Flavour}");
            Console.WriteLine($"Size:{o.Size}");
            Console.WriteLine($"Quantity:{o.Quantity}");
            Console.WriteLine("================================");
            return Task.CompletedTask;
        }

    }
    
}