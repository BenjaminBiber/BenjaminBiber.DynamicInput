using System;

namespace BenjaminBiber.DynamicInput;

/// <summary>
/// Marks a property to be rendered under the specified heading when using <see cref="DynamicObjectEditor"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class DynamicInputHeadingAttribute : Attribute
{
    public DynamicInputHeadingAttribute(string heading)
    {
        Heading = heading ?? throw new ArgumentNullException(nameof(heading));
    }

    public string Heading { get; }
}
