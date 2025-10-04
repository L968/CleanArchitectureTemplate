using CleanArchitectureTemplate.Application.Features.Products.Queries.GetProductById;
using CleanArchitectureTemplate.Domain.Products;
using CleanArchitectureTemplate.Domain.Results;

namespace CleanArchitectureTemplate.UnitTests.Application.Products.Queries;

public class GetProductByIdTests : IClassFixture<AppDbContextFixture>
{
    private readonly AppDbContext _dbContext;
    private readonly GetProductByIdHandler _handler;

    public GetProductByIdTests(AppDbContextFixture fixture)
    {
        _dbContext = fixture.DbContext;
        var loggerMock = new Mock<ILogger<GetProductByIdHandler>>();

        _handler = new GetProductByIdHandler(_dbContext, loggerMock.Object);
    }

    [Fact]
    public async Task WhenProductExists_ShouldReturnProduct()
    {
        // Arrange
        var existingProduct = new Product(
            name: "Test Product",
            price: 100m
        );

        await _dbContext.Products.AddAsync(existingProduct);
        await _dbContext.SaveChangesAsync();

        var query = new GetProductByIdQuery(existingProduct.Id);

        // Act
        Result<GetProductByIdResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess, "Expected result to be successful");
        Assert.NotNull(result.Value);
        Assert.Equal(existingProduct.Id, result.Value!.Id);
        Assert.Equal("Test Product", result.Value.Name);
        Assert.Equal(100m, result.Value.Price);
    }

    [Fact]
    public async Task WhenProductDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var query = new GetProductByIdQuery(Guid.NewGuid());

        // Act
        Result<GetProductByIdResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess, "Expected result to be failure");
        Assert.Equal("Product.NotFound", result.Error.Code);
    }
}
