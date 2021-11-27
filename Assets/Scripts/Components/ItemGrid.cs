using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class ItemGrid : MonoBehaviour
{
    public Vector2Int size;

    private List<Item> items = new List<Item>();
    private List<Item> weightedItems = new List<Item>();
    private Tilemap tilemap;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        items = new List<Item>(Resources.LoadAll<Item>("Items"));
        foreach (Item item in items)
        {
            for (int i = 0; i < item.weight; i++)
            {
                weightedItems.Add(item);
            }
        }
        PopulateItems();
    }

    private void PopulateItems()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                int tileIndex = Random.Range(0, weightedItems.Count);
                tilemap.SetTile(new Vector3Int(x, y, 0), weightedItems[tileIndex]);
            }
        }
    }
}
