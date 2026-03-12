// src/IInterfaces.Demo/Program.cs
using IInterfaces.Demo;

// Part 1 demo: foreach over a custom collection
Console.WriteLine("=== Part 1: IEnumerable / IEnumerator ===");
var basket = new FruitBasket();
basket.Add(new Apple());
basket.Add(new Banana());
basket.Add(new Orange());
basket.Add(new Pear());

Console.WriteLine("Contents of basket:");
foreach (var fruit in basket)
    Console.WriteLine($"- {fruit.Name}");

Console.WriteLine();

// Part 2 demo: IComparable and IComparer
Console.WriteLine("=== Part 2: IComparable / IComparer ===");
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
fruits.Sort(); // uses Fruit.CompareTo
Console.WriteLine("Sorted by default ordering (Name, then Mass, then Color):");
foreach (var fruit in fruits)
    Console.WriteLine($"- {fruit}");

Console.WriteLine();
fruits.Sort(Fruit.ByMass);
Console.WriteLine("Sorted by mass:");
foreach (var fruit in fruits)
    Console.WriteLine($"- {fruit}");

Console.WriteLine();
fruits.Sort(Fruit.ByColor);
Console.WriteLine("Sorted by color then name:");
foreach (var fruit in fruits)
    Console.WriteLine($"- {fruit}");
