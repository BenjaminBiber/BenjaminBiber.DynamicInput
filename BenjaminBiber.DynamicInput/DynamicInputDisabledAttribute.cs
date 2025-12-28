using System;

namespace BenjaminBiber.DynamicInput;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class DynamicInputDisabledAttribute : Attribute
{
    public DynamicInputDisabledAttribute(string predicateMethodName)
    {
        if (string.IsNullOrWhiteSpace(predicateMethodName))
        {
            throw new ArgumentException("Der Methodenname darf nicht leer sein.", nameof(predicateMethodName));
        }

        PredicateMethodName = predicateMethodName;
    }

    public string PredicateMethodName { get; }
}
