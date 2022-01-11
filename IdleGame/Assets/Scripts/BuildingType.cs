using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Buildings/BuildingType")]
public class BuildingType : ScriptableObject
{
    public string constructionName;
    public Sprite constructionSprite;
    public GameObject constructionObject;
    public float resourceGenerationDuration;
    public int goldGenerationAmount, gemGenerationAmount;
    public int goldCost, gemCost;
    public Vector3Int shapeOfBuilding;
}
