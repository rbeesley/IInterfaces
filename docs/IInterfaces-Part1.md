# IInterfaces Part 1 - Implementing `IEnumerable` and `IEnumerator`

> **Recreation note**  
> This article is a reconstruction of my original CodeProject article
> "IInterfaces Part 1 - Implementing IEnumerable and IEnumerator"
> (original URL: `https://www.codeproject.com/Articles/9768/IInterfaces-Part-1-Implementing-IEnumerable-and-IE`).
> The original site is no longer available, so this version is based on
> my original code, surviving indexed text, and memory of the article's
> structure.
>
> **Editor's note:** Code samples have been updated to C# 12 / .NET 8
> while preserving the original design and teaching goals.

## Introduction

C# and the .NET Framework make it very easy to iterate over collections
using the `foreach` statement. But if you build your own collection
types, how do you make them work with `foreach`?

In this article I'll walk through creating a simple custom collection
and show, step by step, what it takes to implement `IEnumerable` and
`IEnumerator`. We'll use a deliberately simple domain - a basket of
fruit - so we can focus on the mechanics of enumeration rather than the
business problem.

## The `Fruit` types

```csharp
namespace IInterfaces.Demo;

public abstract class Fruit
{
    public abstract string Name { get; }

    public override string ToString() => Name;
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

> **Editor's note:** In the original article, the hierarchy used
> virtual properties and concrete classes. Here we keep the same shape
> but use an abstract base class plus sealed derived classes, which is
> more idiomatic in modern C#.

## A simple `FruitBasket`

```csharp
using System.Collections;
using System.Collections.Generic;

namespace IInterfaces.Demo;

public sealed partial class FruitBasket : IEnumerable<Fruit>, IEnumerable
{
    private Fruit[] _items;
    private int _count;

    public FruitBasket(int initialCapacity = 4)
    {
        _items = new Fruit[initialCapacity];
        _count = 0;
    }

    public int Count => _count;

    public Fruit this[int index]
    {
        get
        {
            if ((uint)index >= (uint)_count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _items[index];
        }
    }

    public void Add(Fruit fruit)
    {
        if (fruit is null) throw new ArgumentNullException(nameof(fruit));
        EnsureCapacity(_count + 1);
        _items[_count++] = fruit;
    }

    private void EnsureCapacity(int capacity)
    {
        if (capacity <= _items.Length)
            return;

        var newCapacity = _items.Length == 0 ? 4 : _items.Length * 2;
        if (newCapacity < capacity)
            newCapacity = capacity;

        var newArray = new Fruit[newCapacity];
        Array.Copy(_items, 0, newArray, 0, _count);
        _items = newArray;
    }

    public IEnumerator<Fruit> GetEnumerator() => new FruitBasketEnumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

> **Editor's note:** The original code used a very similar
> `EnsureCapacity` implementation. At the time, `ArrayList` wasn't
> available in the language. I've kept the original pattern rather than
> using `List` so the role of the enumerator is clearer and to remain
> true to the original article, but this shouldn't be considered
> idiomatic by today's standards.

## Implementing `IEnumerator`

```csharp
using System.Collections;
using System.Collections.Generic;

namespace IInterfaces.Demo;

public sealed class FruitBasketEnumerator : IEnumerator<Fruit>
{
    private readonly FruitBasket _basket;
    private int _index;

    public FruitBasketEnumerator(FruitBasket basket)
    {
        _basket = basket ?? throw new ArgumentNullException(nameof(basket));
        _index = -1;
    }

    public Fruit Current
    {
        get
        {
            if (_index < 0 || _index >= _basket.Count)
                throw new InvalidOperationException("Enumerator is not positioned on a valid element.");
            return _basket[_index];
        }
    }

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (_index >= _basket.Count)
            return false;

        _index++;
        return _index < _basket.Count;
    }

    public void Reset() => _index = -1;

    public void Dispose()
    {
    }
}
```

> **Editor's note:** In the original article I first showed an approach where `FruitBasket`
implemented both `IEnumerable` and `IEnumerator` and then refactored to
this cleaner design. The key points still hold:

- `MoveNext` advances the index and indicates whether there's another
element.
- `Current` returns the fruit at the current index and throws if the
enumerator is in an invalid state.
- `Reset` moves back to the position "before the first element".

## Using `foreach`

```csharp
var basket = new FruitBasket();

basket.Add(new Apple());
basket.Add(new Banana());
basket.Add(new Orange());
basket.Add(new Pear());

Console.WriteLine("Contents of basket:");
foreach (var fruit in basket)
{
    Console.WriteLine($"- {fruit.Name}");
}
```

This is exactly the kind of `foreach` usage we were aiming for. The
basket now behaves just like the built‑in collections when it comes to
iteration.

---

At this point we can iterate over our custom collection, but we haven't
told .NET how to *order* our fruits. In [Part 2](IInterfaces-Part2.md) we'll add `IComparable`
and `IComparer` so we can sort and compare fruits in different ways.
