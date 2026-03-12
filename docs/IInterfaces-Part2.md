# IInterfaces Part 2 - Implementing `IComparable` and `IComparer`

> **Recreation note**  
> This article is a reconstruction of my original CodeProject article
> "IInterfaces Part 2 - Implementing IComparable and IComparer"
> (original URL: `https://www.codeproject.com/Articles/9774/IInterfaces-Part-2-Implementing-IComparable-and-IC`).
> The original site is no longer available, so this version is based on
> code, surviving references, and memory of the series.
>
> **Editor's note:** As with Part 1, the code has been updated to
> C# 12 / .NET 8 while preserving the teaching intent.

## Introduction

In [Part 1](IInterfaces-Part1.md) we made a custom `FruitBasket` collection work with `foreach`
by implementing `IEnumerable` and `IEnumerator`. We can now traverse our
collection, but we still haven't said what it means for one `Fruit` to
be "less than" another.

In this article we'll implement `IComparable` on `Fruit` to provide a
default ordering, and `IComparer` implementations to support alternate
sort orders.

## Implementing `IComparable` on `Fruit`

```csharp
namespace IInterfaces.Demo;

public enum FruitColor
{
    Red,
    Green,
    Yellow,
    Orange
}

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

        // Default ordering: by Name, then by Mass, then by Color.
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
```

> **Editor's note:** The original article used `IComparable` (non‑generic)
> with an `object` parameter. Here I've modernized this to
> `IComparable<Fruit>`, which avoids casting and is preferred in new code,
> but the semantics are the same.

## Sorting with the default ordering

```csharp
var fruits = new List<Fruit>
{
    new Banana { MassGrams = 120, Color = FruitColor.Yellow },
    new Apple  { MassGrams = 150, Color = FruitColor.Red },
    new Orange { MassGrams = 180, Color = FruitColor.Orange },
    new Pear   { MassGrams = 160, Color = FruitColor.Green }
};

Console.WriteLine("Original order:");
foreach (var fruit in fruits)
    Console.WriteLine($"- {fruit}");

fruits.Sort(); // uses Fruit.CompareTo

Console.WriteLine();
Console.WriteLine("Sorted by default ordering (Name, then Mass, then Color):");
foreach (var fruit in fruits)
    Console.WriteLine($"- {fruit}");
```

This is exactly the scenario the original Part 2 was written to
illustrate: once a type implements `IComparable`, the framework's sort
methods know how to order your objects.

## Implementing `IComparer` for alternate sort orders

```csharp
using System.Collections.Generic;

namespace IInterfaces.Demo;

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
```

> **Editor's note:** The original Part 2 code defined nested types
> (e.g., `SortByMassClass`) implementing `IComparer`. Here we keep that
> pattern but use generic `IComparer<Fruit>` and provide static
> properties for convenience.

## Using custom comparers

```csharp
var fruits = new List<Fruit>
{
    new Banana { MassGrams = 120, Color = FruitColor.Yellow },
    new Apple  { MassGrams = 150, Color = FruitColor.Red },
    new Orange { MassGrams = 180, Color = FruitColor.Orange },
    new Pear   { MassGrams = 160, Color = FruitColor.Green }
};

Console.WriteLine("Original order:");
foreach (var fruit in fruits)
    Console.WriteLine($"- {fruit}");

Console.WriteLine();
fruits.Sort(Fruit.ByMass);
Console.WriteLine("Sorted by mass:");
foreach (var fruit in fruits)
    Console.WriteLine($"- {fruit}");

Console.WriteLine();
fruits.Sort(Fruit.ByColor);
Console.WriteLine("Sorted by color, then name:");
foreach (var fruit in fruits)
    Console.WriteLine($"- {fruit}");
```

Now you can choose between:

- The default ordering defined by `IComparable`, and
- Arbitrary alternative orderings defined by `IComparer`.

## Using sorting with `FruitBasket`

```csharp
var basket = new FruitBasket();
basket.Add(new Banana { MassGrams = 110, Color = FruitColor.Yellow });
basket.Add(new Apple  { MassGrams = 140, Color = FruitColor.Red });
basket.Add(new Orange { MassGrams = 175, Color = FruitColor.Orange });
basket.Add(new Pear   { MassGrams = 165, Color = FruitColor.Green });

Console.WriteLine("Basket, insertion order:");
foreach (var fruit in basket)
    Console.WriteLine($"- {fruit}");

var asList = new List<Fruit>();
foreach (var fruit in basket)
    asList.Add(fruit);

asList.Sort(Fruit.ByMass);

Console.WriteLine();
Console.WriteLine("Basket contents sorted by mass (via list copy):");
foreach (var fruit in asList)
    Console.WriteLine($"- {fruit}");
```

The important point is that enumeration (`IEnumerable` / `IEnumerator`)
and ordering (`IComparable` / `IComparer`) are orthogonal: you can use
them together to build collections that are easy to traverse *and*
behave correctly when sorted or compared.
