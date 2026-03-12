// src/IInterfaces.Demo/Fruit.cs
namespace IInterfaces.Demo;

public enum FruitColor
{
    Red,
    Green,
    Yellow,
    Orange
}

// NOTE (Editor): In the original article, Fruit was simpler and may not
// have implemented IComparable; here we combine the Part 1/2 ideas for
// the demo code while keeping the narrative clear in the docs.

public abstract partial class Fruit : IComparable<Fruit>
{
    public abstract string Name { get; }

    public double MassGrams { get; init; }

    public FruitColor Color { get; init; }

    public override string ToString() => $"{Name} ({MassGrams:0}g, {Color})";

    public int CompareTo(Fruit? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;

        // Default ordering: Name, then Mass, then Color.
        var nameCompare = string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        if (nameCompare != 0) return nameCompare;

        var massCompare = MassGrams.CompareTo(other.MassGrams);
        if (massCompare != 0) return massCompare;

        return Color.CompareTo(other.Color);
    }
}

public sealed class Apple : Fruit
{
    public override string Name => "Apple";
}

public sealed class Banana : Fruit
{
    public override string Name => "Banana";
}

public sealed class Orange : Fruit
{
    public override string Name => "Orange";
}

public sealed class Pear : Fruit
{
    public override string Name => "Pear";
}
