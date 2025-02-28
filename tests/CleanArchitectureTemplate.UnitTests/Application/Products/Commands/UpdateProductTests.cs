using CleanArchitectureTemplate.Application.Features.Products.Commands.UpdateProduct;
using CleanArchitectureTemplate.Domain.Products;

namespace CleanArchitectureTemplate.UnitTests.Application.Products.Commands;

public class UpdateProductTests : IClassFixture<AppDbContextFixture>
{
    private readonly AppDbContext _dbContext;
    private readonly UpdateProductHandler _handler;

    public UpdateProductTests(AppDbContextFixture fixture)
    {
        _dbContext = fixture.DbContext;
        var loggerMock = new Mock<ILogger<UpdateProductHandler>>();

        _handler = new UpdateProductHandler(_dbContext, loggerMock.Object);
    }

    [Fact]
    public async Task WhenProductExists_ShouldUpdateProduct()
    {
        // Arrange
        var existingProduct = new Product(
            name: "Old Product",
            price: 100m
        );

        await _dbContext.Products.AddAsync(existingProduct);
        await _dbContext.SaveChangesAsync();

        var command = new UpdateProductCommand
        {
            Id = existingProduct.Id,
            Name = "Updated Product",
            Price = 150m
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Product? updatedProduct = await _dbContext.Products.FindAsync(existingProduct.Id);
        Assert.NotNull(updatedProduct);
        Assert.Equal("Updated Product", updatedProduct.Name);
        Assert.Equal(150m, updatedProduct.Price);
    }

    [Fact]
    public async Task WhenProductDoesNotExist_ShouldThrowAppException()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "Nonexistent Product",
            Price = 100m
        };

        // Act & Assert
        AppException exception = await Assert.ThrowsAsync<AppException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal(ProductErrors.ProductNotFound(command.Id).Message, exception.Message);
        Assert.Equal(ErrorType.NotFound, exception.ErrorType);
    }
}
