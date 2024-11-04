using CleanArchitectureTemplate.Application.Features.Products.Queries.GetProductById;
using CleanArchitectureTemplate.Domain.Products;

namespace CleanArchitectureTemplate.UnitTests.Products.Queries;

public class GetProductByIdTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<ILogger<GetProductByIdHandler>> _loggerMock;
    private readonly GetProductByIdHandler _handler;

    public GetProductByIdTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<GetProductByIdHandler>>();

        _handler = new GetProductByIdHandler(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var investmentProduct = new Product(
            name: "Test Product",
            price: 100m
        );

        _repositoryMock.Setup(x => x.GetByIdAsync(investmentProduct.Id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(investmentProduct);

        var query = new GetProductByIdQuery(Id: investmentProduct.Id);

        // Act
        GetProductByIdResponse result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(investmentProduct.Id, result.Id);
        Assert.Equal("Test Product", result.Name);
        Assert.Equal(100m, result.Price);
    }

    [Fact]
    public async Task ShouldThrowAppException_WhenProductDoesNotExist()
    {
        // Arrange
        var query = new GetProductByIdQuery(Id: Guid.Empty);

        _repositoryMock.Setup(x => x.GetByIdAsync(Guid.Empty, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Product?)null);

        // Act & Assert
        AppException exception = await Assert.ThrowsAsync<AppException>(() => _handler.Handle(query, CancellationToken.None));
        Assert.Equal($"Product with Id {query.Id} not found", exception.Message);
    }
}
