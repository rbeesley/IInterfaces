# IInterfaces - Recreated CodeProject series

This repository contains a modern reconstruction of my two‑part article
series originally published on [CodeProject](https://www.codeproject.com):

- **IInterfaces Part 1 - Implementing IEnumerable and IEnumerator**  
  Original URL:  
  `https://www.codeproject.com/Articles/9768/IInterfaces-Part-1-Implementing-IEnumerable-and-IE`
- **IInterfaces Part 2 - Implementing IComparable and IComparer**  
  Original URL:  
  `https://www.codeproject.com/Articles/9774/IInterfaces-Part-2-Implementing-IComparable-and-IC`

CodeProject has since gone offline / become unreachable, and the
original article pages and download archives are no longer available.

## What's in this repo

- `src/IInterfaces.Demo/` - A .NET 8 / C# 12 console project containing
  the sample code used in the articles:
  - `Fruit` hierarchy (`Apple`, `Banana`, `Orange`, `Pear`)
  - `FruitBasket` custom collection
  - `FruitBasketEnumerator`
  - `IComparable<Fruit>` implementation on `Fruit`
  - `IComparer<Fruit>` implementations for alternative sort orders
- `docs/` - Markdown reconstructions of the original articles:
  - `IInterfaces-Part1.md` ([link - Part 1](docs\IInterfaces-Part1.md))
  - `IInterfaces-Part2.md` ([link - Part 2](docs\IInterfaces-Part2.md))

Each Markdown article starts with a **Recreation note** that explains
the origin of the content and provides the original CodeProject URL for
provenance. Where the code or wording has been modernized, the text
includes explicit **Editor's notes** so readers can distinguish between
original behavior and updated implementation details.

## Goals of the reconstruction

The aim of this project is to:

- Preserve the **teaching intent** of the original series: carefully
  crafted examples that demonstrate the usefulness of implementing
  various interfaces in .NET.
- Keep the **structure and narrative** as close as possible to the 2005
  articles, based on surviving indexed text and the original sample
  code.
- Provide **working, idiomatic .NET 8 code** so readers can clone the
  repo, build, and run the samples with modern tooling.
- While I've validated that this recreation builds, it hasn't been
  closely examined to see that it functions properly. There may be bugs.

If you remember reading the original articles on CodeProject, this
repository is intended to feel very familiar, just with updated
language features and project layout.

## How to build and run

```bash
git clone <this-repo-url>
cd IInterfaces
dotnet build
dotnet run --project src/IInterfaces.Demo/IInterfaces.Demo.csproj
```
