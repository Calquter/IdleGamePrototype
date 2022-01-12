using UnityEngine;
using System.Collections.Generic;

public class ConstructionManager : MonoBehaviour
{

    public Building currentBuild;
    [SerializeField] private GameObject _placeholderBuildingObject;

    [SerializeField] private List<GameObject> _objectsShapes;
    private GameObject _selectedGrid;
    private bool _canBuild, _isOutOfTile;

    public List<Vector2Int> constructableTiles;
    private void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            _objectsShapes.Add(Instantiate(_placeholderBuildingObject, new Vector2(500, 500), Quaternion.identity));
            _objectsShapes[i].SetActive(false);
        }
    }

    private void Update()
    {
        RaycastEvent();
    }

    private void RaycastEvent()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            if (currentBuild != null)
            {
                Vector2Int coordinate = GameManager.instance.gridManager.GetTileCoordinate(hit.collider.gameObject);
                

                if (_selectedGrid != null && !_selectedGrid.Equals(hit.collider.gameObject))
                    HidePlaceHolders();

                _selectedGrid = hit.collider.gameObject;

                VisualizeBuildingShape(coordinate);

                if (Input.GetMouseButtonDown(0) && _canBuild && !_isOutOfTile)
                {
                    if (hit.collider.tag == "Grid")
                    {
                        List<Vector2Int> tiles = FindConstructingPlaces(coordinate);


                        for (int i = 0; i < tiles.Count; i++)
                        {
                            ConstructBuilding(tiles[i]);
                        }

                        HidePlaceHolders();
                    }
                }
            }
        }
    }

    private void VisualizeBuildingShape(Vector2Int coordinates)
    {
        PlaceShapes(FindConstructingPlaces(coordinates));
    }
    
    private List<Vector2Int> FindConstructingPlaces(Vector2Int coordinates)
    {
        constructableTiles = new List<Vector2Int>();

        for (int row = 0; row < currentBuild.type.rows.Length; row++)
        {
            int horizontalLeftMove = Mathf.Abs(currentBuild.type.rows[row].x);
            int horizontalRightMove = Mathf.Abs(currentBuild.type.rows[row].y);
            _isOutOfTile = false;


            if (horizontalLeftMove != 0)
                for (int x = 0; x < horizontalLeftMove; x++)
                {
                    if (coordinates.x + x < 10 && coordinates.x + x >= 0 && coordinates.y - row < 10 && coordinates.y - row >= 0)
                        constructableTiles.Add(new Vector2Int(coordinates.x - x, coordinates.y - row));
                    else
                        _isOutOfTile = true;
                }
                    

            if (horizontalRightMove != 0)
                for (int y = 0; y < horizontalRightMove; y++)
                {
                    if (coordinates.x + y < 10 && coordinates.x + y >= 0 && coordinates.y - row < 10 && coordinates.y - row >= 0)
                        constructableTiles.Add(new Vector2Int(coordinates.x + y, coordinates.y - row));
                    else
                        _isOutOfTile = true;
                }


            if (coordinates.x < 10 && coordinates.x >= 0 && coordinates.y - row < 10 && coordinates.y - row >= 0)
                constructableTiles.Add(new Vector2Int(coordinates.x, coordinates.y - row));
            else
                _isOutOfTile = true;


        }
        
        return constructableTiles;
    }

    private void ConstructBuilding(Vector2Int gridPos)
    {
        Instantiate(currentBuild.type.constructionObject, GameManager.instance.gridManager.tiles[gridPos.x, gridPos.y].transform.position, Quaternion.identity);
        GameManager.instance.gridManager.DestroGrid(gridPos);
    }

    private void PlaceShapes(List<Vector2Int> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            _objectsShapes[i].SetActive(true);

            if (GameManager.instance.gridManager.tiles.GetLength(0) < tiles[i].x || 
                GameManager.instance.gridManager.tiles.GetLength(1) < tiles[i].y || GameManager.instance.gridManager.tiles[tiles[i].x, tiles[i].y] == null)
            {
                continue;
            }

            _objectsShapes[i].transform.position = GameManager.instance.gridManager.tiles[tiles[i].x, tiles[i].y].transform.position;

            _canBuild = GameManager.instance.gridManager.ControlShapeFit(tiles);

        }

        

        if (_canBuild && !_isOutOfTile)
            PaintSilhouettes(Color.green);
        else
            PaintSilhouettes(Color.red);
    }

    private void PaintSilhouettes(Color color)
    {
        for (int i = 0; i < _objectsShapes.Count; i++)
        {
            _objectsShapes[i].GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void HidePlaceHolders()
    {
        for (int i = 0; i < _objectsShapes.Count; i++)
        {
            _objectsShapes[i].SetActive(false);
        }
    }

    private void CancelBuildSelection()
    {

    }


}
