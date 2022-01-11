using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    [SerializeField] private int _rows;
    [SerializeField] private int _cols;
    [SerializeField] private float _gridSize;

    [SerializeField] private GameObject _emptyTile;

    public GameObject[,] tiles;

    private void Start()
    {
        tiles = new GameObject[_rows, _cols];

        for (int x = 0; transform.position.x + x < transform.position.x + _rows; x++)
        {
            for (int y = 0; transform.position.y + y < transform.position.y + _cols; y++)
            {
                tiles[x, y] = Instantiate(_emptyTile, new Vector2(transform.position.x + x * _gridSize, transform.position.y + y * _gridSize), Quaternion.identity, transform);
            }
        }

    }

    public Vector2Int GetTileCoordinate(GameObject gridObject)
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
            for (int y = 0; y < tiles.GetLength(1); y++)
                if (gridObject.Equals(tiles[x, y]))
                    return new Vector2Int(x, y);

        return Vector2Int.zero;
    }

    public void DestroGrid(Vector2Int gridPos)
    {
        tiles[gridPos.x, gridPos.y].SetActive(false);
    }
  
    

    public bool ControlShapeFit(List<Vector2Int> gridPos)
    {
        for (int i = 0; i < gridPos.Count; i++)
            if (gridPos[i].x > tiles.GetLength(0) - 1 || gridPos[i].x < 0 || gridPos[i].y > tiles.GetLength(1) - 1 || gridPos[i].y < 0 || !tiles[gridPos[i].x, gridPos[i].y].activeSelf)
                return false;

        return true;
        
    }

}
