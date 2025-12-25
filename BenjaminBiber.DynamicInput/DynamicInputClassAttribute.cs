using System;
using System.Collections.Generic;
using System.Linq;

namespace BenjaminBiber.DynamicInput;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public sealed class DynamicInputClassAttribute : Attribute
{
    private static readonly StringSplitOptions SplitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    public DynamicInputClassAttribute(params string[] cssClasses)
    {
        CssClasses = cssClasses?
            .SelectMany(static value => value?.Split(' ', SplitOptions) ?? Array.Empty<string>())
            .Where(static value => !string.IsNullOrWhiteSpace(value))
            .ToArray() ?? Array.Empty<string>();
    }

    public IReadOnlyList<string> CssClasses { get; }
}
