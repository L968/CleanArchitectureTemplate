using CleanArchitectureTemplate.Application.Features.Products.Commands.UpdateProduct;
using CleanArchitectureTemplate.Domain.Products;
using CleanArchitectureTemplate.Domain.Results;

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

        var command = new UpdateProductCommand(
            Id: existingProduct.Id,
            Name: "Updated Product",
            Price: 150m
        );

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess, "Expected update to succeed");

        Product? updatedProduct = await _dbContext.Products.FindAsync(new object?[] { existingProduct.Id }, CancellationToken.None);
        Assert.NotNull(updatedProduct);
        Assert.Equal("Updated Product", updatedProduct!.Name);
        Assert.Equal(150m, updatedProduct.Price);
    }

    [Fact]
    public async Task WhenProductDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var command = new UpdateProductCommand(
            Id: Guid.NewGuid(),
            Name: "Nonexistent Product",
            Price: 100m
        );

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess, "Expected result to be failure");
        Assert.Equal("Product.NotFound", result.Error.Code);
    }
}
