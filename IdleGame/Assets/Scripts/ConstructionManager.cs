using UnityEngine;
using System.Collections.Generic;

public class ConstructionManager : MonoBehaviour, ISaveLoadData
{

    public Building currentBuild;
    [SerializeField] private GameObject _placeholderBuildingObject;
    [SerializeField] private GameObject _constructedGridObject;

    [SerializeField] private List<GameObject> _objectsShapes;
    private GameObject _selectedGrid;
    private bool _canBuild, _isOutOfTile, _isSelectedSpace, _isResourceEnough;
    [HideInInspector] public bool isBuildingDragging;

    public List<Vector2Int> constructableTiles;

    [HideInInspector] public List<GameObject> constructedList;

    private int _constructedGridCount;


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
            _isSelectedSpace = false;
            
            if (currentBuild != null)
            {
                Vector2Int coordinate = GameManager.instance.gridManager.GetTileCoordinate(hit.collider.gameObject);

                _isResourceEnough = ControlResource();

                if (_selectedGrid != null && !_selectedGrid.Equals(hit.collider.gameObject))
                    HidePlaceHolders();

                _selectedGrid = hit.collider.gameObject;

                PlaceShapes(FindConstructingPlaces(coordinate));

                if (!isBuildingDragging && _canBuild && !_isOutOfTile && !_isSelectedSpace && _isResourceEnough)
                {
                    if (hit.collider.tag == "Grid")
                    {
                        List<Vector2Int> tiles = FindConstructingPlaces(coordinate);

                        for (int i = 0; i < tiles.Count; i++)
                        {
                            ConstructBuilding(tiles[i]);
                        }

                        
                        Building build = Instantiate(currentBuild.gameObject, GameManager.instance.gridManager.tiles[tiles[tiles.Count / 2].x, tiles[tiles.Count / 2].y].transform.position + Vector3.up * -.5f, Quaternion.identity)
                            .GetComponent<Building>();
                        GameManager.instance.CreateFloatText(build.transform.position, build);
                        GameManager.instance.playerData.SetMyGem(-currentBuild.type.gemCost);
                        GameManager.instance.playerData.SetMyGold(-currentBuild.type.goldCost);
                        GameManager.instance.SelectBuilding(null);
                        HidePlaceHolders();

                        constructedList.Add(build.gameObject);
                    }
                }
                else if(!isBuildingDragging)
                {
                    GameManager.instance.SelectBuilding(null);
                    HidePlaceHolders();
                }
            }
        }
        else
        {
            _isSelectedSpace = true;
            PaintSilhouettes(Color.red);

            if (!isBuildingDragging)
            {
                GameManager.instance.SelectBuilding(null);
                HidePlaceHolders();
            }

        }
            
    }
    private List<Vector2Int> FindConstructingPlaces(Vector2Int coordinates)
    {
        constructableTiles = new List<Vector2Int>();
        _isOutOfTile = false;
        for (int row = 0; row < currentBuild.type.rows.Length; row++)
        {
            int horizontalLeftMove = Mathf.Abs(currentBuild.type.rows[row].x);
            int horizontalRightMove = Mathf.Abs(currentBuild.type.rows[row].y);

            if (horizontalLeftMove != 0)
                for (int x = 1; x < horizontalLeftMove; x++)
                {
                    if (coordinates.x + x < 10 && coordinates.x + x >= 0 && coordinates.y - row < 10 && coordinates.y - row >= 0)
                        constructableTiles.Add(new Vector2Int(coordinates.x - x, coordinates.y - row));
                    else
                        _isOutOfTile = true;
                }
                    
            if (horizontalRightMove != 0)
                for (int y = 1; y < horizontalRightMove; y++)
                {
                    if (coordinates.x + y < 10 && coordinates.x + y >= 0 && coordinates.y - row < 10 && coordinates.y - row >= 0)
                        constructableTiles.Add(new Vector2Int(coordinates.x + y, coordinates.y - row));
                    else
                    {
                        _isOutOfTile = true;
                    }
                        
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

        PlayerPrefs.SetInt($"{_constructedGridCount}:GridX", gridPos.x);
        PlayerPrefs.SetInt($"{_constructedGridCount}:GridY", gridPos.y);

        GameManager.instance.gridManager.DestroyGrid(gridPos);

        _constructedGridCount++;
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
            _canBuild = GameManager.instance.gridManager.ControlWithOtherBuildings(tiles);
        }

        

        if (_canBuild && !_isOutOfTile && !_isSelectedSpace && _isResourceEnough)
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
    public void HidePlaceHolders()
    {
        for (int i = 0; i < _objectsShapes.Count; i++)
        {
            _objectsShapes[i].SetActive(false);
        }
    }
    public bool ControlResource()
    {
        if (GameManager.instance.playerData.myGold >= currentBuild.type.goldCost && GameManager.instance.playerData.myGem >= currentBuild.type.gemCost)
            return true;

        return false;
    }
    public void LoadData()
    {
        _constructedGridCount = PlayerPrefs.GetInt("ConstructedGridCount");

        if (PlayerPrefs.HasKey("ConstructedBuildCount"))
        {
            for (int i = 0; i < PlayerPrefs.GetInt("ConstructedBuildCount"); i++)
                for (int x = 0; x < GameManager.instance.allBuildingTypes.Length; x++)
                {
                    if (GameManager.instance.allBuildingTypes[x].GetComponent<Building>().type.constructionName.Equals(PlayerPrefs.GetString($"{i}:BuildName")))
                    {
                        constructedList.Add(Instantiate(GameManager.instance.allBuildingTypes[x], new Vector2(PlayerPrefs.GetFloat($"{i}:BuildPosX"), PlayerPrefs.GetFloat($"{i}:BuildPosY")), Quaternion.identity));
                        constructedList[i].GetComponent<Building>().resourceGenerationCurrentTime = PlayerPrefs.GetFloat($"{i}:BuildResourceGenerationCurrentTime");
                        break;
                    }
                }

            for (int i = 0; i < _constructedGridCount; i++)
                Instantiate(_constructedGridObject, GameManager.instance.gridManager.tiles[PlayerPrefs.GetInt($"{i}:GridX"), PlayerPrefs.GetInt($"{i}:GridY")].transform.position,
                    Quaternion.identity);
            
        }
    }
    public void SaveData()
    {
        if (!GameManager.instance.isGameRestarted)
        {
            PlayerPrefs.SetInt("ConstructedGridCount", _constructedGridCount);

            for (int i = 0; i < constructedList.Count; i++)
            {
                PlayerPrefs.SetString($"{i}:BuildName", constructedList[i].GetComponent<Building>().type.constructionName);
                PlayerPrefs.SetFloat($"{i}:BuildPosX", constructedList[i].transform.position.x);
                PlayerPrefs.SetFloat($"{i}:BuildPosY", constructedList[i].transform.position.y);
                PlayerPrefs.SetFloat($"{i}:BuildResourceGenerationCurrentTime", constructedList[i].GetComponent<Building>().resourceGenerationCurrentTime);
            }

            PlayerPrefs.SetInt("ConstructedBuildCount", constructedList.Count);
        }
    }
    private void OnApplicationQuit() => SaveData();

}
