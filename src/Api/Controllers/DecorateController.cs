using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.KafkaUtil;
using Newtonsoft.Json;
using Confluent.Kafka;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecorateController : ControllerBase
    {

        private readonly ProducerConfig config;
        public DecorateController(ProducerConfig config)
        {
            this.config = config;

        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody] DecoratedOrder decoratedOrder)
        {
            if(!ModelState.IsValid){
                BadRequest();
            }

            Console.WriteLine("===============Decorator====================");
            Console.WriteLine($"POST => order#{decoratedOrder.Id} decoration is completed, moving this to readytobox queue");
            Console.WriteLine("----");
            Console.WriteLine($"Id:{decoratedOrder.Id},Flavour: {decoratedOrder.Flavour},Quantity:{decoratedOrder.Quantity}");
            Console.WriteLine($"PackagedBy:{decoratedOrder.DecoratedBy}, PackagedOn:{decoratedOrder.DecoratedOn}");
            Console.WriteLine("===================================");
            

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(decoratedOrder);
            var producer = new ProducerWrapper(this.config,"readytobox");
            await producer.writeMessage(serializedOrder);

             Created("TransactionId", "Your order is in progress");
        }
    }
}
