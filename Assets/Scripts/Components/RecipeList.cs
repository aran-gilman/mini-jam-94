using System.Collections.Generic;
using UnityEngine;

public class RecipeList : MonoBehaviour
{
    public GameObject prefab;
    public int spacing;

    public bool IsRecipeDiscovered(Recipe recipe)
    {
        if (!recipeListEntries.ContainsKey(recipe))
        {
            return false;
        }
        return recipeListEntries[recipe].activeSelf;
    }

    public GameObject GetEntryFor(Recipe recipe)
    {
        if (recipeListEntries.ContainsKey(recipe))
        {
            return recipeListEntries[recipe];
        }
        return null;
    }

    private Dictionary<Recipe, GameObject> recipeListEntries = new Dictionary<Recipe, GameObject>();

    private void Start()
    {
        Recipe[] recipes = Resources.LoadAll<Recipe>("Recipes");
        int y = 0;
        foreach (Recipe recipe in recipes)
        {
            GameObject go = Instantiate(prefab, transform);
            go.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, y, spacing);
            go.GetComponent<RecipeDisplay>().recipe = recipe;
            recipeListEntries.Add(recipe, go);
            go.SetActive(false);
            y += spacing;
        }
    }
}
