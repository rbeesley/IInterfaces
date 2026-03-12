// src/IInterfaces.Demo/FruitBasket.cs
using System.Collections;
using System.Collections.Generic;

namespace IInterfaces.Demo;

// NOTE (Editor): The original FruitBasket used a raw array and manual
// resizing; we preserve that here rather than using List<Fruit> so the
// mechanics of EnsureCapacity remain visible.

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
