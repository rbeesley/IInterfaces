// src/IInterfaces.Demo/FruitBasketEnumerator.cs
using System.Collections;
using System.Collections.Generic;

namespace IInterfaces.Demo;

// NOTE (Editor): The original article first showed FruitBasket implementing
// both IEnumerable and IEnumerator, then refactored to a separate
// enumerator class. We present only the final, refactored version here.

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
        // No resources to release.
    }
}
