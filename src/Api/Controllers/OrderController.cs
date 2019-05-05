using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Api.Models;
using Api.KafkaUtil;
using Newtonsoft.Json;
using Confluent.Kafka;
using Common.Utils;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly ProducerConfig _config;

        public OrderController(ProducerConfig config)
        {
            this._config = config ?? throw new ArgumentNullException(nameof(config));
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderAcknowledgment), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequest orderRequest)
        {
            if(!ModelState.IsValid){
                BadRequest();
            }

            Console.WriteLine("===================================");
            Console.WriteLine("POST=> Recieved a new order request");
            Console.WriteLine("----");
            Console.WriteLine($"Flavour: {orderRequest.Flavour},Quantity:{orderRequest.Quantity}, Size:{orderRequest.Size}");
            Console.WriteLine("===================================");
            
            //Generate OrderId number
            orderRequest.Id = SequenceNumberGenerator.Next;

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(orderRequest);
            var producer = new ProducerWrapper(this._config,"orderrequests");
            await producer.writeMessage(serializedOrder);

            var acknowledgementInfo = new OrderAcknowledgment(){
                                            CorrelationId=Guid.NewGuid(),
                                            Acknowledgment="Recieved your order!! Estimated time to process is 10mins",
                                            CreatedOn=DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                                            Order = orderRequest
                                        };
            return CreatedAtAction(nameof(GetById),new { id = orderRequest.Id }, acknowledgementInfo );
        }

        [Route("mix")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderStageAsMixedAsync([FromBody]MixedOrder mixedOrder)
        {
            if(!ModelState.IsValid){
                BadRequest();
            }

            Console.WriteLine("===============MIX====================");
            Console.WriteLine($"POST => updating order#{mixedOrder.Id} as  mixed, moving this to bake queue");
            Console.WriteLine("----");
            Console.WriteLine($"Id:{mixedOrder.Id},Flavour: {mixedOrder.Flavour},Quantity:{mixedOrder.Quantity}");
            Console.WriteLine($"MixedBy:{mixedOrder.MixedBy}, MixedOn:{mixedOrder.MixedOn}");
            Console.WriteLine("===================================");
            
            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(mixedOrder);
            var producer = new ProducerWrapper(this._config,"readytobake");
            await producer.writeMessage(serializedOrder);

            //"your order has been updated as mixed , moved it to bake queue."
            return Ok();
        }

        [Route("bake")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderStageAsBakedAsync([FromBody]BakedOrder bakedOrder)
        {
            if(!ModelState.IsValid){
                BadRequest();
            }

            Console.WriteLine("===============Bake====================");
            Console.WriteLine($"POST => updating order#{bakedOrder.Id} as baked, moving this to decorate queue");
            Console.WriteLine("----");
            Console.WriteLine($"Id:{bakedOrder.Id},Flavour: {bakedOrder.Flavour},Quantity:{bakedOrder.Quantity}");
            Console.WriteLine($"BakedBy:{bakedOrder.BakedBy}, BakedOn:{bakedOrder.BakedOn}");
            Console.WriteLine("===================================");
            

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(bakedOrder);
            var producer = new ProducerWrapper(this._config,"readytodecorate");
            await producer.writeMessage(serializedOrder);

            //"your order has been updated as baked , moved it to decorate queue."
            return Ok();
        }

        [Route("decorate")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderStageAsDecoratedAsync([FromBody]DecoratedOrder decoratedOrder)
        {
            if(!ModelState.IsValid){
                BadRequest();
            }

            Console.WriteLine("===============Decorator====================");
            Console.WriteLine($"POST => updating order#{decoratedOrder.Id} as decorated, moving this to readytobox queue");
            Console.WriteLine("----");
            Console.WriteLine($"Id:{decoratedOrder.Id},Flavour: {decoratedOrder.Flavour},Quantity:{decoratedOrder.Quantity}");
            Console.WriteLine($"DecoratedBy:{decoratedOrder.DecoratedBy}, DecoratedOn:{decoratedOrder.DecoratedOn}");
            Console.WriteLine("===================================");
            

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(decoratedOrder);
            var producer = new ProducerWrapper(this._config,"readytobox");
            await producer.writeMessage(serializedOrder);


            //"your order has been updated as decorated , moved it to box queue."
            return Ok();
        }

        [Route("box")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderStageAsPackagedAsync([FromBody]BoxedOrder boxedOrder)
        {
            if(!ModelState.IsValid){
                BadRequest();
            }

            Console.WriteLine("===============Packaging====================");
            Console.WriteLine($"POST => updating order#{boxedOrder.Id} as packaged , moving this to readytoship queue");
            Console.WriteLine("----");
            Console.WriteLine($"Id:{boxedOrder.Id},Flavour: {boxedOrder.Flavour},Quantity:{boxedOrder.Quantity}");
            Console.WriteLine($"PackagedBy:{boxedOrder.PackagedBy}, PackagedOn:{boxedOrder.PackagedOn}");
            Console.WriteLine("===================================");
            

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(boxedOrder);
            var producer = new ProducerWrapper(this._config,"readytoship");
            await producer.writeMessage(serializedOrder);


            //"your order has been updated as packaged , moved it to readytoship queue."
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult<OrderRequest> GetById(int id)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
