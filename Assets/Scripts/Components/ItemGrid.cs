using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class ItemGrid : MonoBehaviour
{
    public Vector2Int size;

    private List<Item> items = new List<Item>();
    private Tilemap tilemap;

    public void PopulateItems(int tier)
    {
        IEnumerable<Item> unlockedItems = items.Where(item => item.tier <= tier);
        List<Item> weightedItems = new List<Item>();
        foreach (Item item in unlockedItems)
        {
            for (int i = 0; i < item.weight; i++)
            {
                weightedItems.Add(item);
            }
        }

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                int tileIndex = Random.Range(0, weightedItems.Count);
                tilemap.SetTile(new Vector3Int(x, y, 0), weightedItems[tileIndex]);
            }
        }
    }

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        items = new List<Item>(Resources.LoadAll<Item>("Items"));
        PopulateItems(0);
    }
}
