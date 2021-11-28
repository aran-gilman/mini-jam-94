using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplay : MonoBehaviour
{
    public Recipe recipe;
    public GameObject ingredientPrefab;

    public Text nameElement;
    public GameObject ingredientContainer;

    private void Start()
    {
        nameElement.text = recipe.displayName;
        foreach (Recipe.Ingredient ingredient in recipe.ingredients)
        {
            GameObject go = Instantiate(ingredientPrefab, ingredientContainer.transform);
            go.GetComponentInChildren<Text>().text = $"{ingredient.quantity}x";
            go.GetComponentInChildren<Image>().sprite = ingredient.item.sprite;
        }
    }
}
