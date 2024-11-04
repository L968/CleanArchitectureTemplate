using CleanArchitectureTemplate.Application.Abstractions;
using CleanArchitectureTemplate.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Database;

public sealed class ProductsDbContext(DbContextOptions<ProductsDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<Product> Products { get; set; }
}
