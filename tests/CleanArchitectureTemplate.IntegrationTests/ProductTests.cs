using System.Net.Http.Json;
using Bogus;
using CleanArchitectureTemplate.Application;
using CleanArchitectureTemplate.Application.Features.Products.Queries.GetProducts;

namespace CleanArchitectureTemplate.IntegrationTests;

public class ProductTests : IClassFixture<CleanArchitectureTemplateApiFixture>
{
    private readonly HttpClient _httpClient;
    private readonly Faker _faker;
    private const string ApiVersion = "v1";

    public ProductTests(CleanArchitectureTemplateApiFixture fixture)
    {
        _httpClient = fixture.HttpClient!;
        _faker = new Faker();
    }

    [Fact]
    public async Task GetProducts_WhenCalled_ShouldReturnOkWithPaginatedList()
    {
        // Arrange
        const int expectedPage = 1;
        const int expectedPageSize = 10;

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync($"/{ApiVersion}/products?page={expectedPage}&pageSize={expectedPageSize}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PaginatedList<GetProductsResponse>? paginatedResponse = await response.Content.ReadFromJsonAsync<PaginatedList<GetProductsResponse>>();

        Assert.NotNull(paginatedResponse);
        Assert.Equal(expectedPage, paginatedResponse.Page);
        Assert.Equal(expectedPageSize, paginatedResponse.PageSize);
        Assert.True(paginatedResponse.TotalItems >= 0);
        Assert.True(paginatedResponse.TotalPages >= 0);
        Assert.NotNull(paginatedResponse.Items);
        Assert.All(paginatedResponse.Items, Assert.NotNull);
    }

    [Fact]
    public async Task GetProductById_WhenProductExists_ShouldReturnOk()
    {
        var createProductCommand = new
        {
            name = _faker.Commerce.ProductName(),
            price = _faker.Random.Decimal(1, 100)
        };

        HttpResponseMessage createResponse = await _httpClient.PostAsJsonAsync($"/{ApiVersion}/product", createProductCommand);
        CreatedProductResponse? createdProduct = await createResponse.Content.ReadFromJsonAsync<CreatedProductResponse>();

        HttpResponseMessage response = await _httpClient.GetAsync($"/{ApiVersion}/product/{createdProduct!.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProductById_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        string nonExistentProductId = Guid.NewGuid().ToString();
        HttpResponseMessage response = await _httpClient.GetAsync($"/{ApiVersion}/product/{nonExistentProductId}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_WhenValid_ShouldReturnCreated()
    {
        var createProductCommand = new
        {
            name = _faker.Commerce.ProductName(),
            price = _faker.Random.Decimal(1, 100)
        };

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"/{ApiVersion}/product", createProductCommand);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_WhenProductExists_ShouldReturnNoContent()
    {
        var createProductCommand = new
        {
            name = _faker.Commerce.ProductName(),
            price = _faker.Random.Decimal(1, 100)
        };

        HttpResponseMessage createResponse = await _httpClient.PostAsJsonAsync($"/{ApiVersion}/product", createProductCommand);
        CreatedProductResponse? createdProduct = await createResponse.Content.ReadFromJsonAsync<CreatedProductResponse>();

        var updateProductCommand = new
        {
            name = _faker.Commerce.ProductName(),
            price = _faker.Random.Decimal(1, 100)
        };

        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/{ApiVersion}/product/{createdProduct!.Id}", updateProductCommand);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        string nonExistentProductId = Guid.NewGuid().ToString();
        var updateProductCommand = new
        {
            name = _faker.Commerce.ProductName(),
            price = _faker.Random.Decimal(1, 100)
        };

        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/{ApiVersion}/product/{nonExistentProductId}", updateProductCommand);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_WhenProductExists_ShouldReturnNoContent()
    {
        var createProductCommand = new
        {
            name = _faker.Commerce.ProductName(),
            price = _faker.Random.Decimal(1, 100)
        };

        HttpResponseMessage createResponse = await _httpClient.PostAsJsonAsync($"/{ApiVersion}/product", createProductCommand);
        CreatedProductResponse createdProduct = await createResponse.Content.ReadFromJsonAsync<CreatedProductResponse>();

        HttpResponseMessage response = await _httpClient.DeleteAsync($"/{ApiVersion}/product/{createdProduct!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        string nonExistentProductId = Guid.NewGuid().ToString();
        HttpResponseMessage response = await _httpClient.DeleteAsync($"/{ApiVersion}/product/{nonExistentProductId}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

internal sealed class CreatedProductResponse
{
    public string Id { get; set; }
}
