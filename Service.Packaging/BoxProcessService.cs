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
    public class BoxProcessService : BackgroundService
    {
        public BoxProcessService()
        {
        }
        public BoxProcessService(IHubContext<OrderMonitorHub, IOrderRequest> orderMonitorHub,ProducerConfig producerConfig, ConsumerConfig consumerConfig)
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
            Console.WriteLine($"Box service is running at: {DateTime.Now}");

            while (!stoppingToken.IsCancellationRequested)
            {

                var consumerHelper = new ConsumerWrapper(_consumerConfig, "readytobox");
                string message = consumerHelper.readMessage();

                //Deserilaize 
                DecoratedOrder readyToBoxRequest = JsonConvert.DeserializeObject<DecoratedOrder>(message);

                //TODO:: Process Order
                Console.WriteLine($"Info: boxing/packaging the order for {readyToBoxRequest.Id}");

                //Step 1: If there is a new message to readytobox topic, inform the client.
                 await _orderMonitorHub.Clients.All.InformNewOrderToPackage(readyToBoxRequest);

                //TODO: Assume you are packaging the decorated cupcakes here
                await Task.Delay(5000);

                //Step 2: Write to readytobake topic
                BoxedOrder boxedOrder = new BoxedOrder(){
                                        Id=readyToBoxRequest.Id,
                                        Flavour=readyToBoxRequest.Flavour,
                                        Quantity=readyToBoxRequest.Quantity,
                                        Size=readyToBoxRequest.Size,
                                        PackagedBy="Srinivasa",
                                        PackagedOn="24th April,2019"};

                string serializedOrder = JsonConvert.SerializeObject(boxedOrder);
                var producerHelper = new ProducerWrapper(_producerConfig,"readytoship");
                await producerHelper.writeMessage(serializedOrder);
                Console.WriteLine($"Info: Packaging process finished the order, request moved to readytoship");
                Console.Write($"------Completed Order -----------");
            }
        }
    }
}