using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public BuildingType type;
    
    private float _resourceGenerationCurrenTime;
    [SerializeField] private float resouceGenerateSpeed;
    [SerializeField] private TMP_Text _buildingName;
    [SerializeField] private TMP_Text _resourceRemainTime;
    [SerializeField] private Slider _proccesSlider;


    private void Awake() => _resourceGenerationCurrenTime = type.resourceGenerationDuration;

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
        _resourceGenerationCurrenTime -= Time.deltaTime * resouceGenerateSpeed;

        _resourceRemainTime.text = _resourceGenerationCurrenTime.ToString("F0") + "s";

        _proccesSlider.maxValue = type.resourceGenerationDuration;
        _proccesSlider.value = _resourceGenerationCurrenTime % type.resourceGenerationDuration;

        if (_resourceGenerationCurrenTime <= 0)
        {
            GenerateResource();
            _resourceGenerationCurrenTime = type.resourceGenerationDuration;
        }
    }

    private void GenerateResource()
    {
        GameManager.instance.playerData.myGold += type.goldGenerationAmount;
        GameManager.instance.playerData.myGem += type.gemGenerationAmount;

        GameManager.instance.CreateFloatText(transform.position, type.goldGenerationAmount, type.gemGenerationAmount);

    }
}
