using Microsoft.EntityFrameworkCore;
using VsaTemplate.Domain.Todos;

namespace VsaTemplate.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Todo> Todos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}