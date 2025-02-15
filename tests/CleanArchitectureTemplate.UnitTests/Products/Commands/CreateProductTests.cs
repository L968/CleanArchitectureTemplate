using CleanArchitectureTemplate.Application.Abstractions;
using CleanArchitectureTemplate.Application.Features.Products.Commands.CreateProduct;
using CleanArchitectureTemplate.Domain.Products;

namespace CleanArchitectureTemplate.UnitTests.Products.Commands;

public class CreateProductTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateProductHandler _handler;

    public CreateProductTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var loggerMock = new Mock<ILogger<CreateProductHandler>>();

        _handler = new CreateProductHandler(_repositoryMock.Object, _unitOfWorkMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task ShouldCreateNewProduct_WhenProductDoesNotExist()
    {
        // Arrange
        var command = new CreateProductCommand(
            Name: "New Product",
            Price: 150m
        );

        // Act
        CreateProductResponse result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Product", result.Name);
        Assert.Equal(150m, result.Price);
        _repositoryMock.Verify(x => x.Create(It.IsAny<Product>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
