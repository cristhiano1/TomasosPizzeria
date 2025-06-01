using TomasosPizzeria.Data.Entities;

namespace TomasosPizzeria.Data.Interfaces
{
    
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task AddOrderAsync(Order order);
        void UpdateOrder(Order order);
        Task DeleteOrderAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
