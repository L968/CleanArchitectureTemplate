using System.Net.Http.Json;
using Bogus;

namespace CleanArchitectureTemplate.IntegrationTests;

public class ProductTests : IClassFixture<CleanArchitectureTemplateApiFixture>
{
    private readonly HttpClient _httpClient;
    private readonly Faker _faker;

    public ProductTests(CleanArchitectureTemplateApiFixture fixture)
    {
        _httpClient = fixture.HttpClient!;
        _faker = new Faker();
    }

    [Fact]
    public async Task GetProducts_WhenCalled_ShouldReturnOk()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/products");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProductById_WhenProductExists_ShouldReturnOk()
    {
        var createProductCommand = new
        {
            name = _faker.Commerce.ProductName(),
            price = _faker.Random.Decimal(1, 100)
        };

        HttpResponseMessage createResponse = await _httpClient.PostAsJsonAsync("/product", createProductCommand);
        CreatedProductResponse? createdProduct = await createResponse.Content.ReadFromJsonAsync<CreatedProductResponse>();

        HttpResponseMessage response = await _httpClient.GetAsync($"/product/{createdProduct!.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProductById_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        string nonExistentProductId = Guid.NewGuid().ToString();
        HttpResponseMessage response = await _httpClient.GetAsync($"/product/{nonExistentProductId}");
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

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/product", createProductCommand);
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

        HttpResponseMessage createResponse = await _httpClient.PostAsJsonAsync("/product", createProductCommand);
        CreatedProductResponse? createdProduct = await createResponse.Content.ReadFromJsonAsync<CreatedProductResponse>();

        var updateProductCommand = new
        {
            name = _faker.Commerce.ProductName(),
            price = _faker.Random.Decimal(1, 100)
        };

        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/product/{createdProduct!.Id}", updateProductCommand);
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

        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/product/{nonExistentProductId}", updateProductCommand);
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

        HttpResponseMessage createResponse = await _httpClient.PostAsJsonAsync("/product", createProductCommand);
        CreatedProductResponse? createdProduct = await createResponse.Content.ReadFromJsonAsync<CreatedProductResponse>();

        HttpResponseMessage response = await _httpClient.DeleteAsync($"/product/{createdProduct!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        string nonExistentProductId = Guid.NewGuid().ToString();
        HttpResponseMessage response = await _httpClient.DeleteAsync($"/product/{nonExistentProductId}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

internal sealed class CreatedProductResponse
{
    public string Id { get; set; }
}
