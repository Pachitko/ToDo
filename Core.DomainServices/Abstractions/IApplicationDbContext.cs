using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;

namespace Core.DomainServices.Abstractions
{
    public interface IApplicationDbContext
    {
        DbSet<ToDoItem> ToDoItems { get; set; }
        DbSet<ToDoList> ToDoLists { get; set; }
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}