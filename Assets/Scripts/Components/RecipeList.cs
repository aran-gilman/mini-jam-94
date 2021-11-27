using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeList : MonoBehaviour
{
    public GameObject prefab;
    public int spacing;

    private void Start()
    {
        Recipe[] recipes = Resources.LoadAll<Recipe>("Recipes");
        int y = 0;
        foreach (Recipe recipe in recipes)
        {
            GameObject go = Instantiate(prefab, transform);
            go.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, y, spacing);
            go.GetComponent<RecipeDisplay>().recipe = recipe;
            y += spacing;
        }
    }
}
