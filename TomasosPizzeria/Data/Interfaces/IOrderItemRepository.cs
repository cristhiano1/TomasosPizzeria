using TomasosPizzeria.Data.Entities;

namespace TomasosPizzeria.Data.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync();
        Task<OrderItem?> GetOrderItemByIdAsync(int id);
        Task AddOrderItemAsync(OrderItem orderItem);
        void UpdateOrderItem(OrderItem orderItem);
        Task DeleteOrderItemAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
