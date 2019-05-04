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
    public class MixController : ControllerBase
    {

        private readonly ProducerConfig config;
        public MixController(ProducerConfig config)
        {
            this.config = config;

        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody] MixedOrder mixedOrder)
        {
            if(!ModelState.IsValid){
                BadRequest();
            }

            Console.WriteLine("===============MIX====================");
            Console.WriteLine($"POST => order#{mixedOrder.Id} is mixed, moving this to bake queue");
            Console.WriteLine("----");
            Console.WriteLine($"Id:{mixedOrder.Id},Flavour: {mixedOrder.Flavour},Quantity:{mixedOrder.Quantity}");
            Console.WriteLine($"MixedBy:{mixedOrder.MixedBy}, MixedOn:{mixedOrder.MixedOn}");
            Console.WriteLine("===================================");
            

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(mixedOrder);
            var producer = new ProducerWrapper(this.config,"readytobake");
            await producer.writeMessage(serializedOrder);

             Created("TransactionId", "Your order is in progress");
        }
    }
}
