using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using SignalRDemo.Hubs;
using Newtonsoft.Json;
using Confluent.Kafka;

namespace Api.BackgroundServices
{
    public class OrderMonitorService : BackgroundService
    {
        public OrderMonitorService()
        {
        }
        public OrderMonitorService(IHubContext<OrderMonitorHub, IOrderRequest> orderMonitorHub,ProducerConfig producerConfig, ConsumerConfig consumerConfig)
        {
            this._orderMonitorHub = orderMonitorHub;
            this._producerConfig = producerConfig;
            this._consumerConfig = consumerConfig;
        }
        private IHubContext<OrderMonitorHub, IOrderRequest> _orderMonitorHub;
        private ProducerConfig _producerConfig;
        private ConsumerConfig _consumerConfig;

        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"OrderMonitoring service is running at: {DateTime.Now}");

            while (!stoppingToken.IsCancellationRequested)
            {

                var consumerHelper = new ConsumerWrapper(_consumerConfig, "orderrequests");
                string message = consumerHelper.readMessage();

                //Deserilaize 
                OrderRequest orderRequest = JsonConvert.DeserializeObject<OrderRequest>(message);

                //TODO:: Process Order
                Console.WriteLine($"Info: processing the order for {orderRequest.Id}");

                //Step 1: If there is a new message in KAFKA "Orders" topic, inform the client.
                 await _orderMonitorHub.Clients.All.InformNewOrder(orderRequest);

                //Wait for 
                await Task.Delay(5000);
            }
        }
    }
}