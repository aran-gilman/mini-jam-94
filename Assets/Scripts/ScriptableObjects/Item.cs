using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class Item : TileBase
{
    public string displayName;
    public int weight = 1;
    public Sprite sprite;
    public int tier;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
    }
}
