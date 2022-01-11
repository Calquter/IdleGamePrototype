using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingType type;
    
    private float _resourceGenerationDuration;
    [SerializeField] private float resouceGenerateSpeed;

    private void Awake() => _resourceGenerationDuration = type.resourceGenerationDuration;

    private void Update()
    {
        ResourceGenerationTimer();   
    }

    private void ResourceGenerationTimer() 
    {
        _resourceGenerationDuration -= Time.deltaTime * resouceGenerateSpeed;

        if (_resourceGenerationDuration <= 0)
        {
            GenerateResource();
            _resourceGenerationDuration = type.resourceGenerationDuration;
        }
    }

    private void GenerateResource()
    {
        GameManager.instance.playerData.myGold += type.goldGenerationAmount;
        GameManager.instance.playerData.myGem += type.gemGenerationAmount;
    }
}
