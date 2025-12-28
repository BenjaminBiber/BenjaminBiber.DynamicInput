using System;

namespace BenjaminBiber.DynamicInput;

/// <summary>
/// Controls how boolean properties should be rendered by <see cref="DynamicObjectEditor"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class DynamicInputBooleanAttribute : Attribute
{
    public DynamicInputBooleanAttribute(DynamicInputBooleanDisplay displayAs)
    {
        DisplayAs = displayAs;
    }

    public DynamicInputBooleanDisplay DisplayAs { get; }
}
