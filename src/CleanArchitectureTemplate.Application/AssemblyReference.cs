﻿using System.Reflection;

namespace CleanArchitectureTemplate.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
