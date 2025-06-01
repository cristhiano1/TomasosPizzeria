using Microsoft.EntityFrameworkCore;
using TomasosPizzeria.Context;
using TomasosPizzeria.Data.Entities;
using TomasosPizzeria.Data.Interfaces;

namespace TomasosPizzeria.Data.Repos
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;

        public OrderItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
        {
            return await _context.OrderItems
                .Include(oi => oi.Dish)
                .Include(oi => oi.Order)
                .ToListAsync();
        }

        public async Task<OrderItem?> GetOrderItemByIdAsync(int id)
        {
            return await _context.OrderItems
                .Include(oi => oi.Dish)
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.Id == id);
        }

        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
        }

        public async Task DeleteOrderItemAsync(int id)
        {
            var orderItem = await GetOrderItemByIdAsync(id);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
