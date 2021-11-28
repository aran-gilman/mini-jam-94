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
    public TierDisplay tierDisplay;

    public GameObject lastRecipeDisplay;
    public GameObject ingredientImagePrefab;

    public Text scoreDisplay;
    public Text hintDisplay;

    public TileBase selectionIndicator;

    public AudioClip recipeSuccessSfx;
    public AudioClip recipeFailSfx;

    public int currentTier;
    public int score;

    public int refreshAfterXRecipes = 5;

    public List<Tier> tiers = new List<Tier>();

    private List<Vector3Int> selectedCells = new List<Vector3Int>();
    private List<Recipe> recipes = new List<Recipe>();

    private RandomizeSfx selectSfx;
    private AudioSource audioSource;

    private int recipesUntilRefresh;

    private bool IsAdjacent(Vector3Int a, Vector3Int b)
    {
        int xDiff = Mathf.Abs(a.x - b.x);
        int yDiff = Mathf.Abs(a.y - b.y);
        return xDiff <= 1 && yDiff <= 1 && (xDiff == 0 || yDiff == 0);
    }

    private void Start()
    {
        recipes = new List<Recipe>(Resources.LoadAll<Recipe>("Recipes"));
        selectSfx = GetComponent<RandomizeSfx>();
        audioSource = GetComponent<AudioSource>();

        recipesUntilRefresh = refreshAfterXRecipes;
        tierDisplay.SetTier(0);
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
                    selectSfx.Play();
                }
            }
        }

        if (Input.GetButtonDown("Bake"))
        {
            for (int i = 0; i < lastRecipeDisplay.transform.childCount; i++)
            {
                Destroy(lastRecipeDisplay.transform.GetChild(i).gameObject);
            }

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

                GameObject go = Instantiate(ingredientImagePrefab, lastRecipeDisplay.transform);
                go.GetComponent<Image>().sprite = tile.sprite;
            }

            Recipe recipe = FindMatchingRecipe(ingredients.Values.ToList());
            if (recipe != null)
            {
                score += recipe.value;
                recipeList.GetEntryFor(recipe).SetActive(true);
                hintDisplay.transform.parent.gameObject.SetActive(false);
                audioSource.PlayOneShot(recipeSuccessSfx);
            }
            else
            {
                hintDisplay.text = GetHint(ingredients.Values.ToList());
                hintDisplay.transform.parent.gameObject.SetActive(hintDisplay.text.Length > 0);
                audioSource.PlayOneShot(recipeFailSfx);
            }
            indicatorTilemap.ClearAllTiles();
            selectedCells.Clear();

            int nextTier = currentTier + 1;
            if (nextTier < tiers.Count && tiers[nextTier].IsUnlocked(score, recipes.Where(recipeList.IsRecipeDiscovered).ToList()))
            {
                currentTier = nextTier;
                itemTilemap.GetComponent<ItemGrid>().PopulateItems(currentTier);
                recipesUntilRefresh = refreshAfterXRecipes;
                tierDisplay.SetTier(currentTier);
            }
            else
            {
                recipesUntilRefresh -= 1;
                if (recipesUntilRefresh < 1)
                {
                    itemTilemap.GetComponent<ItemGrid>().PopulateItems(currentTier);
                    recipesUntilRefresh = refreshAfterXRecipes;
                }
            }
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

        int maxDiff = diffs.Keys.Max();
        for (int i = 0; i <= maxDiff; i++)
        {
            if (!diffs.ContainsKey(i))
            {
                continue;
            }
            List<Recipe> recipes = diffs[i];

            Recipe target = recipes.Find(recipe => !recipeList.IsRecipeDiscovered(recipe) && recipe.tier <= currentTier);
            if (target == null)
            {
                continue;
            }

            foreach (Recipe.Ingredient ingredient in ingredients)
            {
                if (!target.ingredients.Exists(recipeIngredient => recipeIngredient.item == ingredient.item))
                {
                    return $"Try removing {ingredient.item.displayName}";
                }
            }

            foreach (Recipe.Ingredient recipeIngredient in target.ingredients)
            {
                if (!ingredients.Exists(ingredient => ingredient.item == recipeIngredient.item))
                {
                    return $"Try adding {recipeIngredient.item.displayName}";
                }
            }

            foreach (Recipe.Ingredient ingredient in ingredients)
            {
                Recipe.Ingredient recipeIngredient = target.ingredients.Find(r => r.item == ingredient.item);
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
        return "";
    }

    private void FixedUpdate()
    {
        scoreDisplay.text = $"Score: {score}";
    }
}
