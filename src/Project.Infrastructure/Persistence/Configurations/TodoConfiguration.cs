using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Todos;

namespace Project.Infrastructure.Persistence.Configurations;

public sealed class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("todos");

        builder.HasKey(todo => todo.Id);

        builder.Property(todo => todo.Id)
            .ValueGeneratedNever();

        builder.Property(todo => todo.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(todo => todo.IsCompleted)
            .IsRequired();
    }
}