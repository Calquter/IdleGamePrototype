using UnityEngine;
using System.Collections.Generic;

public class ConstructionManager : MonoBehaviour
{

    public Building currentBuild;
    [SerializeField] private GameObject _placeholderBuildingObject;

    [SerializeField] private List<GameObject> _objectsShapes;
    private GameObject _selectedGrid;
    private void Start()
    {
        for (int i = 0; i < 10; i++)
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
                Vector2Int coordinates = GameManager.instance.gridManager.GetTileCoordinate(hit.collider.gameObject);
                
                if (_selectedGrid != null && !_selectedGrid.Equals(hit.collider.gameObject))
                    HidePlaceHolders();

                _selectedGrid = hit.collider.gameObject;

                VisualizeBuildingShape(coordinates);

                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.collider.tag == "Grid")
                    {
                        
                        List<Vector2Int> tiles = FindConstructingPlaces(coordinates);

                        for (int i = 0; i < tiles.Count; i++)
                        {
                            ConstructBuilding(tiles[i]);
                        }
                        
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
        GameObject[,] tiles = GameManager.instance.gridManager.tiles;
        List<Vector2Int> constructableTiles = new List<Vector2Int>();

        #region Horizontal Dimention

        int horizontalMoveCount = Mathf.Abs(currentBuild.type.shapeOfBuilding.x) + 1;

        for (int i = 0; i < horizontalMoveCount; i++)
        {
            if (currentBuild.type.shapeOfBuilding.x < 0)
                constructableTiles.Add(new Vector2Int(coordinates.x - i, coordinates.y));
            else
                constructableTiles.Add(new Vector2Int(coordinates.x + i, coordinates.y));
        }

        #endregion

        #region Vertical Dimention

        int verticalMoveCount = Mathf.Abs(currentBuild.type.shapeOfBuilding.y) + 1;

        for (int i = 0; i < verticalMoveCount; i++)
        {
            if (currentBuild.type.shapeOfBuilding.y < 0)
                constructableTiles.Add(new Vector2Int(coordinates.x, coordinates.y - i));
            else
                constructableTiles.Add(new Vector2Int(coordinates.x, coordinates.y + i));
        }

        #endregion

        #region Depth Dimention

        int depthMoveCount = Mathf.Abs(currentBuild.type.shapeOfBuilding.z) + 1;

        for (int i = 0; i < depthMoveCount; i++)
        {
            if (currentBuild.type.shapeOfBuilding.z < 0)
                constructableTiles.Add(new Vector2Int(coordinates.x + i, coordinates.y - i));
            else
                constructableTiles.Add(new Vector2Int(coordinates.x + i, coordinates.y + i));
        }

        #endregion

        return constructableTiles;
    }

    private bool ControlShapeFit()
    {
        return false;
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
            _objectsShapes[i].transform.position = GameManager.instance.gridManager.tiles[tiles[i].x, tiles[i].y].transform.position;
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
