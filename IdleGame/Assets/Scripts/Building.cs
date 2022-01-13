using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public BuildingType type;
    
    [HideInInspector] public float resourceGenerationCurrentTime;
    [SerializeField] private float resouceGenerateSpeed;
    [SerializeField] private TMP_Text _buildingName;
    [SerializeField] private TMP_Text _resourceRemainTime;
    [SerializeField] private Slider _proccesSlider;


    private void Awake() => resourceGenerationCurrentTime = type.resourceGenerationDuration;

    private void Start()
    {
        _buildingName.text = type.constructionName;
    }

    private void Update()
    {
        ResourceGenerationTimer();   
    }

    private void ResourceGenerationTimer() 
    {
        resourceGenerationCurrentTime -= Time.deltaTime * resouceGenerateSpeed;

        _resourceRemainTime.text = resourceGenerationCurrentTime.ToString("F0") + "s";

        _proccesSlider.maxValue = type.resourceGenerationDuration;
        _proccesSlider.value = resourceGenerationCurrentTime % type.resourceGenerationDuration;

        if (resourceGenerationCurrentTime <= 0)
        {
            GenerateResource();
            resourceGenerationCurrentTime = type.resourceGenerationDuration;
        }
    }

    private void GenerateResource()
    {
        GameManager.instance.playerData.SetMyGold(type.goldGenerationAmount);
        GameManager.instance.playerData.SetMyGem(type.gemGenerationAmount);

        GameManager.instance.CreateFloatText(transform.position, type.goldGenerationAmount, type.gemGenerationAmount);

    }
}
