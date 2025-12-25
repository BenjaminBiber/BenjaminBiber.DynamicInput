using System;

namespace BenjaminBiber.DynamicInput;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class DynamicInputIgnoreAttribute : Attribute
{
}
