﻿using System;

namespace BETAS.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ActionAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
}