using System.Threading.Tasks;

namespace Api.Models{
    public interface IOrder
    {
        Task InformApps(Order o);
    }
}