using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public string displayName;
    public int value;

    [Serializable]
    public class Ingredient
    {
        public TileBase item;
        public int quantity;
    }
    public List<Ingredient> ingredients = new List<Ingredient>();
}
