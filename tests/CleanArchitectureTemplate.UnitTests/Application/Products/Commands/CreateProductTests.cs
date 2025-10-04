using CleanArchitectureTemplate.Application.Features.Products.Commands.CreateProduct;
using CleanArchitectureTemplate.Domain.Results;

namespace CleanArchitectureTemplate.UnitTests.Application.Products.Commands;

public class CreateProductTests : IClassFixture<AppDbContextFixture>
{
    private readonly CreateProductHandler _handler;

    public CreateProductTests(AppDbContextFixture fixture)
    {
        AppDbContext dbContext = fixture.DbContext;
        var loggerMock = new Mock<ILogger<CreateProductHandler>>();

        _handler = new CreateProductHandler(dbContext, loggerMock.Object);
    }

    [Fact]
    public async Task WhenProductDoesNotExist_ShouldCreateNewProduct()
    {
        // Arrange
        var command = new CreateProductCommand(
            Name: "New Product",
            Price: 150m
        );

        // Act
        Result<CreateProductResponse> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess, "Expected result to be successful");
        Assert.NotNull(result.Value);
        Assert.Equal("New Product", result.Value!.Name);
        Assert.Equal(150m, result.Value.Price);
    }
}
