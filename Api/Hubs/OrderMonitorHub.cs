using Api.Models;
using Microsoft.AspNetCore.SignalR;

namespace SignalRDemo.Hubs{
    public class OrderMonitorHub: Hub<IOrder>
    {

    }
}