using System.Reflection;

namespace CleanArchitectureTemplate.ArchitectureTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Domain.Products.Product).Assembly;

    protected static readonly Assembly ApplicationAssembly = typeof(Application.AssemblyReference).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.DependencyInjection).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Presentation.AssemblyReference).Assembly;
}
