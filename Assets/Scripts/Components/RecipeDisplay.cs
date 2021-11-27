using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplay : MonoBehaviour
{
    public Recipe recipe;
    public Text nameElement;
    public Text ingredientElement;

    private void Start()
    {
        nameElement.text = recipe.displayName;
        ingredientElement.text = string.Join(", ", recipe.ingredients.Select(i => $"{i.quantity}x {i.item.displayName}"));
    }
}
