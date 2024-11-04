using CleanArchitectureTemplate.Application.Features.Products.Queries.GetProducts;
using CleanArchitectureTemplate.Domain.Products;

namespace CleanArchitectureTemplate.UnitTests.Products.Queries;

public class GetProductsTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<ILogger<GetProductsHandler>> _loggerMock;
    private readonly GetProductsHandler _handler;

    public GetProductsTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<GetProductsHandler>>();

        _handler = new GetProductsHandler(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ShouldReturnListOfProducts_WhenProductsExist()
    {
        // Arrange
        var investmentProducts = new List<Product>
        {
            new(name: "Product A", price: 100m),
            new(name: "Product B", price: 200m),
        };

        _repositoryMock.Setup(x => x.GetAsync(It.IsAny<CancellationToken>())).ReturnsAsync(investmentProducts);
        var query = new GetProductsQuery();

        // Act
        IEnumerable<GetProductsResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("Product A", result.ElementAt(0).Name);
        Assert.Equal("Product B", result.ElementAt(1).Name);
    }

    [Fact]
    public async Task ShouldReturnEmptyList_WhenNoProductsExist()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);
        var query = new GetProductsQuery();

        // Act
        IEnumerable<GetProductsResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
