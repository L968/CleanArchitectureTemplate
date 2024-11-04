using System.Reflection;

namespace CleanArchitectureTemplate.Presentation;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
