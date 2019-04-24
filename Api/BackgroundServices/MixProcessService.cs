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
    public class MixProcessService : BackgroundService
    {
        public MixProcessService()
        {
        }
        public MixProcessService(IHubContext<OrderMonitorHub, IOrderRequest> orderMonitorHub,ProducerConfig producerConfig, ConsumerConfig consumerConfig)
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
                 await _orderMonitorHub.Clients.All.InformNewOrderToMix(orderRequest);

                //TODO: Assume you are mixing the required stuff here
                await Task.Delay(5000);

                //Step 2: Write to readytobake topic
                MixedOrder mixedOrder = new MixedOrder(){Id=orderRequest.Id,Flavour=orderRequest.Flavour,Quantity=orderRequest.Quantity,Size=orderRequest.Size,MixedBy="Srinivasa",MixedOn="24th April,2019"};

                string serializedOrder = JsonConvert.SerializeObject(mixedOrder);
                var producerHelper = new ProducerWrapper(_producerConfig,"readytobake");
                await producerHelper.writeMessage(serializedOrder);
                Console.WriteLine($"Info: Mixer processed the order, request moved to bake process");

            }
        }
    }
}