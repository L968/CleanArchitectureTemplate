using CleanArchitectureTemplate.Application.Features.Products.Commands.DeleteProduct;
using CleanArchitectureTemplate.Domain.Products;
using CleanArchitectureTemplate.Domain.Results;

namespace CleanArchitectureTemplate.UnitTests.Application.Products.Commands;

public class DeleteProductTests : IClassFixture<AppDbContextFixture>
{
    private readonly AppDbContext _dbContext;
    private readonly DeleteProductHandler _handler;

    public DeleteProductTests(AppDbContextFixture fixture)
    {
        _dbContext = fixture.DbContext;
        var loggerMock = new Mock<ILogger<DeleteProductHandler>>();

        _handler = new DeleteProductHandler(_dbContext, loggerMock.Object);
    }

    [Fact]
    public async Task WhenProductExists_ShouldDeleteProduct()
    {
        // Arrange
        var existingProduct = new Product(
            name: "Product to Delete",
            price: 100m
        );

        await _dbContext.Products.AddAsync(existingProduct);
        await _dbContext.SaveChangesAsync();

        var command = new DeleteProductCommand(existingProduct.Id);

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess, "Expected deletion to succeed");

        Product? deletedProduct = await _dbContext.Products.FindAsync([existingProduct.Id], CancellationToken.None);
        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task WhenProductDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var command = new DeleteProductCommand(Guid.NewGuid());

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess, "Expected result to be failure");
        Assert.Equal("Product.NotFound", result.Error.Code);
    }
}
