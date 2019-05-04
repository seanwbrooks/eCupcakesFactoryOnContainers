using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using SignalRDemo.Hubs;
using Newtonsoft.Json;
using Confluent.Kafka;
using Api.KafkaUtil;

namespace Api.BackgroundServices
{
    public class DecorateProcessService : BackgroundService
    {
        public DecorateProcessService()
        {
        }
        public DecorateProcessService(IHubContext<OrderMonitorHub, IOrderRequest> orderMonitorHub, ProducerConfig producerConfig, ConsumerConfig consumerConfig)
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
            Console.WriteLine($"Decorator service is running at: {DateTime.Now}");

            while (!stoppingToken.IsCancellationRequested)
            {

                var consumerHelper = new ConsumerWrapper(_consumerConfig, "readytodecorate");
                string message = consumerHelper.readMessage();

                //Deserilaize 
                BakedOrder readyToDecorateRequest = JsonConvert.DeserializeObject<BakedOrder>(message);

                Console.WriteLine($"Info: Recieved order to decorate. Id# {readyToDecorateRequest.Id}");

                //Step 1: If there is a new message in KAFKA "Orders" topic, inform the client.
                Console.WriteLine($"Informing UI connected clients about the newly recieved order. Id# {readyToDecorateRequest.Id}");
                await _orderMonitorHub.Clients.All.InformNewOrderToDecorate(readyToDecorateRequest);
            }
        }
    }
}