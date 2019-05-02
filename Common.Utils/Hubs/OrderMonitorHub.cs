using System.Threading.Tasks;
using Api.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;

namespace SignalRDemo.Hubs{
    public class OrderMonitorHub : Hub<IOrderRequest>
    {
        private SignalRKafkaProxy _proxy;
        protected OrderMonitorHub()
        {
            _proxy = new SignalRKafkaProxy();
        }

        public override async Task OnConnectedAsync(){
            //manage and track connections here

            //
            SignalRClient client = extractInformation(Context);
            _proxy.AddClient(client);

            Consumer<string,string> consumerObj = new Consumer<string,string>(client.ConsumerConfig);
            _proxy.AddConsumer(consumerObj);

            await base.OnConnectedAsync();
        }
        private SignalRClient extractInformation(HubCallerContext context){
            //TODO
            ConsumerConfig config = new ConsumerConfig(){
                GroupId = "G1",
                BootstrapServers="localhost:9092",
                ClientId=context.ConnectionId
            };
            return new SignalRClient(){ ConsumerConfig = config, Url="http://localhost:3000/",Topic="orderrequests",ConsumerGroup="G1",ConnectionId=context.ConnectionId};
        }
    }
}