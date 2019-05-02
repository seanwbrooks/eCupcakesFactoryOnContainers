using System.Collections.Generic;
using Confluent.Kafka;

namespace SignalRDemo.Hubs
{
    public class SignalRKafkaProxy{

        private List<SignalRClient> _clients;
        private List<Consumer<string,string>> _consumers;


        public SignalRKafkaProxy()
        {
            
        }

        public void AddClient(SignalRClient client){
            _clients.Add(client);
        }

        public void AddConsumer(Consumer<string,string> consumerConnection){
            _consumers.Add(consumerConnection);
        }

    }

    public class SignalRClient{
        public string ConnectionId { get; set; }
        public string Url { get; set; }
        public string Topic { get; set; }
        public string ConsumerGroup {get;set;}

        public ConsumerConfig ConsumerConfig{get;set;}
    }
}