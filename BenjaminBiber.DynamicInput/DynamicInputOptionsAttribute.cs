using System;

namespace BenjaminBiber.DynamicInput;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class DynamicInputOptionsAttribute : Attribute
{
    public DynamicInputOptionsAttribute(string providerMethodName)
    {
        ProviderMethodName = providerMethodName;
    }

    public string ProviderMethodName { get; }
}
