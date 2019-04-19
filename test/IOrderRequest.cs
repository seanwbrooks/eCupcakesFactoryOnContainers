using System.Threading.Tasks;

namespace Api.Models{
    public interface IOrder
    {
        Task InformNewOrder(Order o);
    }
}