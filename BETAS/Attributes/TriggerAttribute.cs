using System;

namespace BETAS.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class TriggerAttribute : Attribute
{
    public TriggerAttribute()
    {
    }
}