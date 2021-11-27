using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public string displayName;
    public int value;

    [Serializable]
    public class Ingredient
    {
        public Item item;
        public int quantity;
    }
    public List<Ingredient> ingredients = new List<Ingredient>();
}
