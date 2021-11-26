using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInput : MonoBehaviour
{
    public Tilemap itemTilemap;
    public Tilemap indicatorTilemap;

    public TileBase selectionIndicator;

    private List<Vector3Int> selectedCells = new List<Vector3Int>();

    private void Update()
    {
        if (Input.GetButtonDown("Select"))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mouseCell = itemTilemap.WorldToCell(mousePos);

            TileBase indicatorAtPosition = indicatorTilemap.GetTile(mouseCell);
            if (indicatorAtPosition == selectionIndicator)
            {
                indicatorTilemap.SetTile(mouseCell, null);
                selectedCells.Remove(mouseCell);
            }
            else
            {
                if (selectedCells.Count == 0)
                {
                    indicatorTilemap.SetTile(mouseCell, selectionIndicator);
                    selectedCells.Add(mouseCell);
                }
                else
                {
                    bool isValid = false;
                    foreach (Vector3Int cell in selectedCells)
                    {
                        int xDiff = Mathf.Abs(cell.x - mouseCell.x);
                        int yDiff = Mathf.Abs(cell.y - mouseCell.y);

                        if (xDiff <= 1 && yDiff <= 1 && (xDiff == 0 ^ yDiff == 0))
                        {
                            isValid = true;
                            break;
                        }

                    }

                    if (isValid)
                    {
                        indicatorTilemap.SetTile(mouseCell, selectionIndicator);
                        selectedCells.Add(mouseCell);
                    }
                }
            }
        }
    }
}
