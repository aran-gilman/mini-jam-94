using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public Tilemap itemTilemap;
    public Tilemap indicatorTilemap;
    public RecipeList recipeList;

    public Text scoreDisplay;
    public Text hintDisplay;

    public TileBase selectionIndicator;

    public int score;

    private List<Vector3Int> selectedCells = new List<Vector3Int>();
    private List<Recipe> recipes = new List<Recipe>();

    private bool IsAdjacent(Vector3Int a, Vector3Int b)
    {
        int xDiff = Mathf.Abs(a.x - b.x);
        int yDiff = Mathf.Abs(a.y - b.y);
        return xDiff <= 1 && yDiff <= 1 && (xDiff == 0 || yDiff == 0);
    }

    private void Start()
    {
        recipes = new List<Recipe>(Resources.LoadAll<Recipe>("Recipes"));
    }

    public Recipe FindMatchingRecipe(List<Recipe.Ingredient> ingredients)
    {
        foreach (Recipe recipe in recipes)
        {
            if (recipe.ingredients.Count != ingredients.Count)
            {
                continue;
            }
            if (recipe.ingredients.All(a => ingredients.Exists(b => a.item == b.item && a.quantity == b.quantity)))
            {
                return recipe;
            }
        }
        return null;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Select"))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mouseCell = itemTilemap.WorldToCell(mousePos);

            if (itemTilemap.GetTile(mouseCell) != null && !selectedCells.Contains(mouseCell))
            {
                TileBase indicatorAtPosition = indicatorTilemap.GetTile(mouseCell);
                if (selectedCells.Count == 0 || selectedCells.Exists(cell => IsAdjacent(cell, mouseCell)))
                {
                    indicatorTilemap.SetTile(mouseCell, selectionIndicator);
                    selectedCells.Add(mouseCell);
                }
            }
        }

        if (Input.GetButtonDown("Bake"))
        {
            Dictionary<TileBase, Recipe.Ingredient> ingredients = new Dictionary<TileBase, Recipe.Ingredient>();
            foreach (Vector3Int cell in selectedCells)
            {
                Item tile = itemTilemap.GetTile(cell) as Item;
                if (!ingredients.ContainsKey(tile))
                {
                    ingredients.Add(tile, new Recipe.Ingredient
                    {
                        item = tile,
                        quantity = 0
                    });
                }
                ingredients[tile].quantity += 1;
                itemTilemap.SetTile(cell, null);
            }
            Recipe recipe = FindMatchingRecipe(ingredients.Values.ToList());
            if (recipe != null)
            {
                score += recipe.value;
                recipeList.GetEntryFor(recipe).SetActive(true);
                hintDisplay.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                hintDisplay.transform.parent.gameObject.SetActive(true);
                hintDisplay.text = GetHint(ingredients.Values.ToList());
            }
            indicatorTilemap.ClearAllTiles();
            selectedCells.Clear();
        }
    }

    private string GetHint(List<Recipe.Ingredient> ingredients)
    {
        Dictionary<int, List<Recipe>> diffs = new Dictionary<int, List<Recipe>>();
        foreach (Recipe recipe in recipes)
        {
            int diff = 0;
            foreach (Recipe.Ingredient recipeIngredient in recipe.ingredients)
            {
                if (!ingredients.Exists(ingredient => ingredient.item == recipeIngredient.item))
                {
                    diff += 1;
                }
            }
            if (!diffs.ContainsKey(diff))
            {
                diffs.Add(diff, new List<Recipe>());
            }
            diffs[diff].Add(recipe);
        }

        if (diffs.ContainsKey(0))
        {
            foreach (Recipe.Ingredient ingredient in ingredients)
            {
                Recipe.Ingredient recipeIngredient = diffs[0][0].ingredients.Find(r => r.item == ingredient.item);
                if (ingredient.quantity > recipeIngredient.quantity)
                {
                    return $"Too much {ingredient.item.displayName}";
                }
                else if (ingredient.quantity < recipeIngredient.quantity)
                {
                    return $"Not enough {ingredient.item.displayName}";
                }
            }
        }
        else
        {
            int smallestDiff = diffs.Keys.Min();
            Recipe nearestRecipe = diffs[smallestDiff][0];
            foreach (Recipe.Ingredient recipeIngredient in nearestRecipe.ingredients)
            {
                if (!ingredients.Exists(ingredient => ingredient.item == recipeIngredient.item))
                {
                    return $"Try adding {recipeIngredient.item.displayName}";
                }
            }
        }
        return "No available hints";
    }

    private void FixedUpdate()
    {
        scoreDisplay.text = $"${score}";
    }
}
