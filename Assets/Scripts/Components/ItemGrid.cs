using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class ItemGrid : MonoBehaviour
{
    public Vector2Int size;
    public List<TileBase> itemTiles = new List<TileBase>();

    private Tilemap tilemap;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        PopulateItems();
    }

    private void PopulateItems()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                int tileIndex = Random.Range(0, itemTiles.Count);
                tilemap.SetTile(new Vector3Int(x, y, 0), itemTiles[tileIndex]);
            }
        }
    }
}
