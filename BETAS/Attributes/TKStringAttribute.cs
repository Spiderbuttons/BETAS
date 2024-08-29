using System;

namespace BETAS.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class TKStringAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
}