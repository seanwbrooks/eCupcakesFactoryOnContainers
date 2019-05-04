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
    public class BoxController : ControllerBase
    {

        private readonly ProducerConfig config;
        public BoxController(ProducerConfig config)
        {
            this.config = config;

        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody] BoxedOrder boxedOrder)
        {
            if(!ModelState.IsValid){
                BadRequest();
            }

            Console.WriteLine("===============Packaging====================");
            Console.WriteLine($"POST => order#{boxedOrder.Id} packaging is completed, moving this to toship queue");
            Console.WriteLine("----");
            Console.WriteLine($"Id:{boxedOrder.Id},Flavour: {boxedOrder.Flavour},Quantity:{boxedOrder.Quantity}");
            Console.WriteLine($"PackagedBy:{boxedOrder.PackagedBy}, PackagedOn:{boxedOrder.PackagedOn}");
            Console.WriteLine("===================================");
            

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(boxedOrder);
            var producer = new ProducerWrapper(this.config,"readytoship");
            await producer.writeMessage(serializedOrder);

             Created("TransactionId", "Your order is in progress");
        }
    }
}
