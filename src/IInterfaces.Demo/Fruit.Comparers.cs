// src/IInterfaces.Demo/Fruit.Comparers.cs
using System.Collections.Generic;

namespace IInterfaces.Demo;

// NOTE (Editor): The original Part 2 download used nested classes
// SortByMassClass and SortByColorClass implementing IComparer.
// Here we modernize the naming but preserve behavior.

public abstract partial class Fruit
{
    public sealed class SortByMass : IComparer<Fruit>
    {
        public int Compare(Fruit? x, Fruit? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;

            return x.MassGrams.CompareTo(y.MassGrams);
        }
    }

    public sealed class SortByColor : IComparer<Fruit>
    {
        public int Compare(Fruit? x, Fruit? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;

            var colorCompare = x.Color.CompareTo(y.Color);
            if (colorCompare != 0) return colorCompare;

            return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
        }
    }

    public static IComparer<Fruit> ByMass { get; } = new SortByMass();
    public static IComparer<Fruit> ByColor { get; } = new SortByColor();
}
