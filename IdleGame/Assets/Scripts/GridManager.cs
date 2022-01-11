using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    [SerializeField] private int _rows;
    [SerializeField] private int _cols;
    [SerializeField] private float _gridSize;

    [SerializeField] private GameObject _emptyTile;

    public GameObject[,] _tiles;

    private Vector2Int lastCoordinate = Vector2Int.zero;

    private void Start()
    {
        _tiles = new GameObject[_rows, _cols];

        for (int x = 0; transform.position.x + x < transform.position.x + _rows; x++)
        {
            for (int y = 0; transform.position.y + y < transform.position.y + _cols; y++)
            {
                _tiles[x, y] = Instantiate(_emptyTile, new Vector2(transform.position.x + x * _gridSize, transform.position.y + y * _gridSize), Quaternion.identity, transform);
            }
        }

    }

    public Vector2Int GetTileCoordinate(GameObject gridObject)
    {
        for (int x = 0; x < _tiles.GetLength(0); x++)
            for (int y = 0; y < _tiles.GetLength(1); y++)
                if (gridObject.Equals(_tiles[x, y]))
                    return new Vector2Int(x, y);


        return Vector2Int.zero;
    }

}
