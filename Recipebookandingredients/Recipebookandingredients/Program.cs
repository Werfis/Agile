using System;
using System.Collections;
using System.Collections.Generic;

public enum CraftTier
{
    Basic,
    Advanced,
    Expert,
    Master,
    Mythic
}

public class Ingredient
{
    public string Code { get; }
    public int Quantity { get; }

    public Ingredient(string code, int quantity)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Ingredient code cannot be empty", nameof(code));
        
        if (quantity < 1)
            throw new ArgumentException("Quantity must be at least 1", nameof(quantity));

        Code = code;
        Quantity = quantity;
    }

    public override string ToString() => $"{Code} х{Quantity}";
}

public class Recipe
{
    private readonly List<Ingredient> _ingredients = new List<Ingredient>();

    public string Id { get; }
    public string Title { get; }
    public CraftTier Tier { get; }
    public IReadOnlyList<Ingredient> Ingredients => _ingredients.AsReadOnly();

    public Recipe(string id, string title, CraftTier tier)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Recipe ID cannot be empty", nameof(id));
        
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Recipe title cannot be empty", nameof(title));

        Id = id;
        Title = title;
        Tier = tier;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        if (ingredient == null)
            throw new ArgumentNullException(nameof(ingredient));
        
        _ingredients.Add(ingredient);
    }

    public Recipe(string id, string title, CraftTier tier, IEnumerable<Ingredient> ingredients)
        : this(id, title, tier)
    {
        if (ingredients == null)
            throw new ArgumentNullException(nameof(ingredients));
        
        foreach (var ingredient in ingredients)
        {
            AddIngredient(ingredient);
        }
    }

    public override string ToString() => Title;
}

public class RecipeBook : IEnumerable<Recipe>
{
    private readonly List<Recipe> _recipes = new List<Recipe>();
    private readonly Dictionary<string, Recipe> _byId = new Dictionary<string, Recipe>();

    public int Count => _recipes.Count;

    public Recipe this[int index]
    {
        get
        {
            if (index < 0 || index >= _recipes.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            
            return _recipes[index];
        }
    }

    public Recipe this[string id]
    {
        get
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            
            if (!_byId.TryGetValue(id, out var recipe))
                throw new KeyNotFoundException($"Recipe with ID '{id}' not found");
            
            return recipe;
        }
    }

    public void Add(Recipe recipe)
    {
        if (recipe == null)
            throw new ArgumentNullException(nameof(recipe));
        
        if (_byId.ContainsKey(recipe.Id))
            throw new ArgumentException($"Recipe with ID '{recipe.Id}' already exists");

        _recipes.Add(recipe);
        _byId.Add(recipe.Id, recipe);
    }

    public bool RemoveAt(int index)
    {
        if (index < 0 || index >= _recipes.Count)
            return false;

        var recipe = _recipes[index];
        _recipes.RemoveAt(index);
        _byId.Remove(recipe.Id);
        
        return true;
    }

    public bool RemoveById(string id)
    {
        if (id == null)
            return false;

        if (!_byId.TryGetValue(id, out var recipe))
            return false;

        _recipes.Remove(recipe);
        _byId.Remove(id);
        
        return true;
    }

    public IEnumerable<Recipe> EnumerateByTier(CraftTier minTier)
    {
        foreach (var recipe in _recipes)
        {
            if (recipe.Tier >= minTier)
                yield return recipe;
        }
    }

    public IEnumerator<Recipe> GetEnumerator() => _recipes.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class Program
{
    public static void Main()
    {
        var book = new RecipeBook();

        // Рецепты обычной еды
        var breadRecipe = new Recipe("bread_001", "Bread", CraftTier.Basic);
        breadRecipe.AddIngredient(new Ingredient("Wheat", 3));
        breadRecipe.AddIngredient(new Ingredient("Water", 1));
        breadRecipe.AddIngredient(new Ingredient("Yeast", 1));
        book.Add(breadRecipe);

        var cakeRecipe = new Recipe("cake_001", "Cake", CraftTier.Advanced);
        cakeRecipe.AddIngredient(new Ingredient("Flour", 2));
        cakeRecipe.AddIngredient(new Ingredient("Eggs", 3));
        cakeRecipe.AddIngredient(new Ingredient("Sugar", 2));
        cakeRecipe.AddIngredient(new Ingredient("Butter", 1));
        cakeRecipe.AddIngredient(new Ingredient("Milk", 1));
        book.Add(cakeRecipe);

        var pieRecipe = new Recipe("pie_001", "Apple Pie", CraftTier.Basic);
        pieRecipe.AddIngredient(new Ingredient("Flour", 2));
        pieRecipe.AddIngredient(new Ingredient("Apples", 4));
        pieRecipe.AddIngredient(new Ingredient("Sugar", 1));
        pieRecipe.AddIngredient(new Ingredient("Butter", 1));
        book.Add(pieRecipe);

        var stewRecipe = new Recipe("stew_001", "Mushroom Stew", CraftTier.Basic);
        stewRecipe.AddIngredient(new Ingredient("Mushrooms", 5));
        stewRecipe.AddIngredient(new Ingredient("Potatoes", 2));
        stewRecipe.AddIngredient(new Ingredient("Carrots", 2));
        stewRecipe.AddIngredient(new Ingredient("Onion", 1));
        stewRecipe.AddIngredient(new Ingredient("Water", 2));
        book.Add(stewRecipe);

        Console.WriteLine("All recipes:");
        foreach (var recipe in book)
        {
            Console.WriteLine(recipe);
            Console.WriteLine("  Ingredients: " + string.Join(", ", recipe.Ingredients));
        }

        Console.WriteLine("\nRecipes Advanced and above:");
        foreach (var recipe in book.EnumerateByTier(CraftTier.Advanced))
        {
            Console.WriteLine(recipe);
        }

        Console.WriteLine($"\nAccess by ID 'cake_001': {book["cake_001"]}");
        Console.WriteLine($"Access by index 2: {book[2]}");

        book.RemoveById("stew_001");
        Console.WriteLine($"\nAfter removal, recipe count: {book.Count}");
        
        Console.WriteLine("\nRemaining recipes:");
        foreach (var recipe in book)
        {
            Console.WriteLine($"- {recipe}");
        }
    }
}