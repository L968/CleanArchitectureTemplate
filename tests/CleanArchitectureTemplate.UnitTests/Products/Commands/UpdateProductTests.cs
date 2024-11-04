using CleanArchitectureTemplate.Application.Abstractions;
using CleanArchitectureTemplate.Application.Features.Products.Commands.UpdateProduct;
using CleanArchitectureTemplate.Domain.Products;

namespace CleanArchitectureTemplate.UnitTests.Products.Commands;

public class UpdateProductTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<UpdateProductHandler>> _loggerMock;
    private readonly UpdateProductHandler _handler;

    public UpdateProductTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<UpdateProductHandler>>();

        _handler = new UpdateProductHandler(_repositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ShouldUpdateProduct_WhenProductExists()
    {
        // Arrange
        var existingProduct = new Product(
            name: "Old Product",
            price: 100m
        );

        var command = new UpdateProductCommand
        {
            Id = existingProduct.Id,
            Name = "Updated Product",
            Price = 150m
        };

        _repositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(existingProduct);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.Update(existingProduct), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.Equal("Updated Product", existingProduct.Name);
        Assert.Equal(150m, existingProduct.Price);
    }

    [Fact]
    public async Task ShouldThrowAppException_WhenProductDoesNotExist()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Id = Guid.Empty,
            Name = "Nonexistent Product",
            Price = 100m
        };

        _repositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Product?)null);

        // Act & Assert
        AppException exception = await Assert.ThrowsAsync<AppException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal($"No Product found with Id {command.Id}", exception.Message);

        _repositoryMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
