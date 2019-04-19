using System.Threading.Tasks;

namespace Api.Models{
    public interface IOrderRequest
    {
        Task InformNewOrder(OrderRequest o);
    }
}