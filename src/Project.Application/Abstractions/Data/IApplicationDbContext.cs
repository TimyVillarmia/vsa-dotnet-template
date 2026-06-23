using Microsoft.EntityFrameworkCore;
using Project.Domain.Todos;

namespace Project.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Todo> Todos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}