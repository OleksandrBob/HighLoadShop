using HighLoadShop.Domain.OrderContext.Entities;

namespace HighLoadShop.Application.OrderContext.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    void Add(Order order);
    void Update(Order order);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
