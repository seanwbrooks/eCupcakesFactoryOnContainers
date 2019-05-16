using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System;
namespace SignalRDemo.Hubs{
    public class OrderMonitorHub : Hub<IOrderRequest>
    {
        public override async Task OnConnectedAsync(){
            //manage and track connections here
            if(!SignalRKafkaProxy.AllConsumers.Keys.Contains(Context.ConnectionId)){
                
                Console.WriteLine("Recieved new connection");
                //
                SignalRClient client = extractInformation(Context);
                SignalRKafkaProxy.AddClient(client);

                //Create a connection object 
                IConsumer<string,string> consumerObj = new ConsumerBuilder<string,string>(client.ConsumerConfig).Build();
                consumerObj.Subscribe(client.Topic);
                Console.WriteLine($"Adding {consumerObj}");
                SignalRKafkaProxy.AddConsumer(client.ConnectionId,consumerObj);
            }

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"Connection Disconnected {Context.ConnectionId}");
            //Remove consumer connection
            if(SignalRKafkaProxy.AllConsumers.Keys.Contains(Context.ConnectionId)){
                SignalRKafkaProxy.AllConsumers.Remove(Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }
        private SignalRClient extractInformation(HubCallerContext context){
            //TODO
            var httpContext = Context.GetHttpContext();
            string consumerGroupName = httpContext.Request.Query["consumergroup"];
            string topicName = httpContext.Request.Query["topic"];
            ConsumerConfig config = new ConsumerConfig(){
                GroupId = consumerGroupName,
                BootstrapServers="broker:9092",
                ClientId=context.ConnectionId
            };
            return new SignalRClient(){ ConsumerConfig = config,Topic=topicName,ConsumerGroup=consumerGroupName,ConnectionId=context.ConnectionId};
        }

        public Dictionary<string,IConsumer<string,string>> AllActiveConnections{
            get{
                return  SignalRKafkaProxy.AllConsumers;
            }
        }
    }
}