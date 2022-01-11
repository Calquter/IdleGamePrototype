using UnityEngine;

public class ConstructionManager : MonoBehaviour
{

    public Building currentBuild;

    private void Update()
    {
        RaycastEvent();
    }

    private void RaycastEvent()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider != null)
            {
                if (currentBuild != null)
                {
                    if (hit.collider.tag == "Grid")
                    {
                        Vector2Int coordinates = GameManager.instance.gridManager.GetTileCoordinate(hit.collider.gameObject);

                        FindConstructingPlaces(coordinates);

                    }
                }
            }
        }
    }

    private void FindConstructingPlaces(Vector2Int coordinates)
    {
        GameObject[,] tiles = GameManager.instance.gridManager.tiles;

        ConstructBuilding(coordinates);

        #region Horizontal Dimention

        int horizontalMoveCount = Mathf.Abs(currentBuild.type.shapeOfBuilding.x) + 1;

        for (int i = 0; i < horizontalMoveCount; i++)
        {
            if (currentBuild.type.shapeOfBuilding.x < 0)
                ConstructBuilding(new Vector2Int(coordinates.x - i, coordinates.y));
            else
                ConstructBuilding(new Vector2Int(coordinates.x + i, coordinates.y));
        }

        #endregion

        #region Vertical Dimention

        int verticalMoveCount = Mathf.Abs(currentBuild.type.shapeOfBuilding.y) + 1;

        for (int i = 0; i < verticalMoveCount; i++)
        {
            if (currentBuild.type.shapeOfBuilding.y < 0)
                ConstructBuilding(new Vector2Int(coordinates.x, coordinates.y - i));
            else
                ConstructBuilding(new Vector2Int(coordinates.x, coordinates.y + i));
        }

        #endregion

        #region Depth Dimention

        int depthMoveCount = Mathf.Abs(currentBuild.type.shapeOfBuilding.z) + 1;

        for (int i = 0; i < depthMoveCount; i++)
        {
            if (currentBuild.type.shapeOfBuilding.z < 0)
                ConstructBuilding(new Vector2Int(coordinates.x + i, coordinates.y - i));
            else
                ConstructBuilding(new Vector2Int(coordinates.x + i, coordinates.y + i));
        }

        #endregion
    }

    private void ConstructBuilding(Vector2Int gridPos)
    {
        Instantiate(currentBuild.type.constructionObject, GameManager.instance.gridManager.tiles[gridPos.x, gridPos.y].transform.position, Quaternion.identity);
        print("Constructed");
        GameManager.instance.gridManager.DestroGrid(gridPos);
    }

    private void CancelBuildSelection()
    {

    }


}
