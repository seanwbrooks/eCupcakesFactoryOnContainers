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
        public DecorateProcessService(IHubContext<OrderMonitorHub, IOrderRequest> orderMonitorHub,ProducerConfig producerConfig, ConsumerConfig consumerConfig)
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

                //TODO:: Process Order
                Console.WriteLine($"Info: processing the order for {readyToDecorateRequest.Id}");

                //Step 1: If there is a new message to readytodecorate topic, inform the client.
                 await _orderMonitorHub.Clients.All.InformNewOrderToDecorate(readyToDecorateRequest);

                //TODO: Assume you are decorating on baked cupcakes here
                await Task.Delay(5000);

                //Step 2: Write to readytobake topic
                DecoratedOrder bakedOrder = new DecoratedOrder(){
                                        Id=readyToDecorateRequest.Id,
                                        Flavour=readyToDecorateRequest.Flavour,
                                        Quantity=readyToDecorateRequest.Quantity,
                                        Size=readyToDecorateRequest.Size,
                                        DecoratedBy="Srinivasa",
                                        DecoratedOn="24th April,2019"};

                string serializedOrder = JsonConvert.SerializeObject(bakedOrder);
                var producerHelper = new ProducerWrapper(_producerConfig,"readytobox");
                await producerHelper.writeMessage(serializedOrder);
                Console.WriteLine($"Info: Decorate process finished the order, request moved to Package process");
            }
        }
    }
}