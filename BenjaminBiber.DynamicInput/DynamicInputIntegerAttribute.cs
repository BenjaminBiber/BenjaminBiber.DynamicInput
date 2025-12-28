using System;

namespace BenjaminBiber.DynamicInput;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class DynamicInputIntegerAttribute : Attribute
{
    public DynamicInputIntegerAttribute(int minimum, int maximum, bool displayAsSlider = false)
    {
        if (maximum < minimum)
        {
            throw new ArgumentOutOfRangeException(nameof(maximum), "Maximum darf nicht kleiner sein als Minimum.");
        }

        Minimum = minimum;
        Maximum = maximum;
        DisplayAsSlider = displayAsSlider;
    }

    public int Minimum { get; }

    public int Maximum { get; }

    public bool DisplayAsSlider { get; }
}
