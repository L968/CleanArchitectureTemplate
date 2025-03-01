using CleanArchitectureTemplate.Application;
using CleanArchitectureTemplate.Application.Features.Products.Queries.GetProducts;
using CleanArchitectureTemplate.Domain.Products;

namespace CleanArchitectureTemplate.UnitTests.Application.Products.Queries;

public class GetProductsTests : IClassFixture<AppDbContextFixture>
{
    private readonly AppDbContext _dbContext;
    private readonly GetProductsHandler _handler;

    public GetProductsTests(AppDbContextFixture fixture)
    {
        _dbContext = fixture.DbContext;
        var loggerMock = new Mock<ILogger<GetProductsHandler>>();

        _handler = new GetProductsHandler(_dbContext, loggerMock.Object);
    }

    [Fact]
    public async Task WhenProductsExist_ShouldReturnPaginatedListOfProducts()
    {
        // Arrange
        _dbContext.Products.RemoveRange(_dbContext.Products);
        await _dbContext.SaveChangesAsync();

        var products = new List<Product>
        {
            new(name: "Product A", price: 100m),
            new(name: "Product B", price: 200m),
            new(name: "Product C", price: 300m)
        };

        await _dbContext.Products.AddRangeAsync(products);
        await _dbContext.SaveChangesAsync();

        var query = new GetProductsQuery(Page: 1, PageSize: 2);

        // Act
        PaginatedList<GetProductsResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count());
        Assert.Equal(3, result.TotalItems);
        Assert.Equal(2, result.TotalPages);
        Assert.Contains(result.Items, p => p.Name == "Product A" && p.Price == 100m);
        Assert.Contains(result.Items, p => p.Name == "Product B" && p.Price == 200m);
    }

    [Fact]
    public async Task WhenNoProductsExist_ShouldReturnEmptyPaginatedList()
    {
        // Arrange
        _dbContext.Products.RemoveRange(_dbContext.Products);
        await _dbContext.SaveChangesAsync();

        var query = new GetProductsQuery(Page: 1, PageSize: 10);

        // Act
        PaginatedList<GetProductsResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalItems);
        Assert.Equal(0, result.TotalPages);
    }
}
