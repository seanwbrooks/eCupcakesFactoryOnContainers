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
    public class BakeProcessService : BackgroundService
    {
        public BakeProcessService()
        {
        }
        public BakeProcessService(IHubContext<OrderMonitorHub, IOrderRequest> orderMonitorHub,ProducerConfig producerConfig, ConsumerConfig consumerConfig)
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
            Console.WriteLine($"Bake service is running at: {DateTime.Now}");

            while (!stoppingToken.IsCancellationRequested)
            {

                var consumerHelper = new ConsumerWrapper(_consumerConfig, "readytobake");
                string message = consumerHelper.readMessage();

                //Deserilaize 
                MixedOrder readyToBakeRequest = JsonConvert.DeserializeObject<MixedOrder>(message);

                Console.WriteLine($"Info: Recieved order to bake. Id# {readyToBakeRequest.Id}");

                //Step 1: If there is a new message in KAFKA "Orders" topic, inform the client.
                Console.WriteLine($"Informing UI connected clients about the newly recieved order. Id# {readyToBakeRequest.Id}");
                 await _orderMonitorHub.Clients.All.InformNewOrderToBake(readyToBakeRequest);

            }
        }
    }
}