using System.Threading.Tasks;

namespace Api.Models{
    public interface IOrder
    {
        Task InformNewOrderToMix(OrderRequest o);
    }
}