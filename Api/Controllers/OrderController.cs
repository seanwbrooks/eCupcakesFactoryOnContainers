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
    public class OrderController : ControllerBase
    {

        private readonly ProducerConfig config;
        public OrderController(ProducerConfig config)
        {
            this.config = config;

        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody] OrderRequest orderRequest)
        {
            if(!ModelState.IsValid){
                BadRequest();
            }

            Console.WriteLine("===================================");
            Console.WriteLine("POST=> Recieved a new order request");
            Console.WriteLine("----");
            Console.WriteLine($"Id:{orderRequest.Id},Flavour: {orderRequest.Flavour},Quantity:{orderRequest.Quantity}");
            Console.WriteLine("===================================");
            

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(orderRequest);
            var producer = new ProducerWrapper(this.config,"orderrequests");
            await producer.writeMessage(serializedOrder);

             Created("TransactionId", "Your order is in progress");
        }
    }
}
