using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.SignalR;
namespace SignalRDemo.Hubs{
    public class OrderMonitorHub: Hub<IOrderRequest>
    {
    public async  Task SendOrderRecievedNotificationToClients(OrderRequest orderRequest)
        {
            await Clients.All.InformNewOrderToMix(orderRequest);
        }
    }
}