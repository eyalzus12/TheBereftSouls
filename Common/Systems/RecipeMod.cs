using System;
using Terraria;
using Terraria.ModLoader;

namespace TheBereftSouls.Common.Systems;

public readonly struct RecipeMod
{
    private readonly Action<Recipe> _mod;

    internal RecipeMod(Action<Recipe> mod) => _mod = mod;

    internal void Modify(Recipe recipe) => _mod(recipe);

    public static ChainableRecipeMod AddItem(int itemId, int stack = 1) =>
        new((Recipe recipe) => recipe.AddIngredient(itemId, stack));

    public static ChainableRecipeMod AddItem(ModItem item, int stack = 1) =>
        new((Recipe recipe) => recipe.AddIngredient(item, stack));

    public static ChainableRecipeMod AddItem<T>(int stack = 1)
        where T : ModItem => new((Recipe recipe) => recipe.AddIngredient<T>(stack));

    public static ChainableRecipeMod RemoveItem(int itemId) =>
        new((Recipe recipe) => recipe.RemoveIngredient(itemId));

    public static ChainableRecipeMod ReplaceItem(int origId, int newId) =>
        RemoveItem(origId).AddItem(newId);

    public static ChainableRecipeMod AddDecraftCondition(Condition condition) =>
        new((Recipe recipe) => recipe.AddDecraftCondition(condition));

    // Clone the recipe and modifies each resulting recipe using a different option.
    public static RecipeMod Branch(RecipeMod option1, RecipeMod option2) =>
        new(
            (Recipe recipe) =>
            {
                Recipe newRecipe = recipe.Clone();
                newRecipe.Register();
                newRecipe.SortAfter(recipe);
                option1.Modify(recipe);
                option2.Modify(newRecipe);
            }
        );
}

/*
A wrapper class with chaining operations.
The distinction exists to avoid ambiguities when chaining after operations like Branch.
We want to prevent RecipeMod.Branch(..., ...).AddItem(...).
Since it can be unintuitive which of the recipes the AddItem applies to.
*/
public readonly struct ChainableRecipeMod
{
    private readonly RecipeMod _mod;

    internal ChainableRecipeMod(Action<Recipe> mod) => _mod = new(mod);

    internal void Modify(Recipe recipe) => _mod.Modify(recipe);

    public ChainableRecipeMod AddItem(int itemId, int stack = 1) =>
        Chain(this, RecipeMod.AddItem(itemId, stack));

    public ChainableRecipeMod AddItem(ModItem item, int stack = 1) =>
        Chain(this, RecipeMod.AddItem(item, stack));

    public ChainableRecipeMod AddItem<T>(int stack = 1)
        where T : ModItem => Chain(this, RecipeMod.AddItem<T>(stack));

    public RecipeMod Branch(RecipeMod option1, RecipeMod option2) =>
        Chain(this, RecipeMod.Branch(option1, option2));

    private static ChainableRecipeMod Chain(ChainableRecipeMod first, ChainableRecipeMod second) =>
        new(
            (Recipe recipe) =>
            {
                first.Modify(recipe);
                second.Modify(recipe);
            }
        );

    // unchainable variant
    private static RecipeMod Chain(ChainableRecipeMod first, RecipeMod second) =>
        new(
            (Recipe recipe) =>
            {
                first.Modify(recipe);
                second.Modify(recipe);
            }
        );

    public static implicit operator RecipeMod(ChainableRecipeMod r) => r._mod;
}