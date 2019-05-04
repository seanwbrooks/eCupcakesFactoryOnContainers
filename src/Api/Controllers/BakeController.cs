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
    public class BakeController : ControllerBase
    {

        private readonly ProducerConfig config;
        public BakeController(ProducerConfig config)
        {
            this.config = config;

        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody] BakedOrder bakedOrder)
        {
            if(!ModelState.IsValid){
                BadRequest();
            }

            Console.WriteLine("===============Bake====================");
            Console.WriteLine($"POST => order#{bakedOrder.Id} is baked, moving this to decorate queue");
            Console.WriteLine("----");
            Console.WriteLine($"Id:{bakedOrder.Id},Flavour: {bakedOrder.Flavour},Quantity:{bakedOrder.Quantity}");
            Console.WriteLine($"MixedBy:{bakedOrder.BakedBy}, MixedOn:{bakedOrder.BakedOn}");
            Console.WriteLine("===================================");
            

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(bakedOrder);
            var producer = new ProducerWrapper(this.config,"readytodecorate");
            await producer.writeMessage(serializedOrder);

             Created("TransactionId", "Your order is in progress");
        }
    }
}
