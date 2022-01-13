using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour, IDestroyWithDelay
{
    public static GameManager instance;

    public PlayerData playerData;
    public GridManager gridManager;
    public ConstructionManager constructionManager;

    public GameObject floatingText_Gold;
    public GameObject floatingText_Gem;

    public Canvas canvas;
    [SerializeField] private CanvasGroup[] _buildingCards;

    [HideInInspector] public bool isGameRestarted;

    public GameObject[] allBuildingTypes;

    private void Awake() => instance = this;

    public void SelectBuilding(Building building)
    {
        constructionManager.currentBuild = building;
    }
    public IEnumerator DestroyWithDelay(float time, GameObject gameObject)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    public void CreateFloatText(Vector2 pos, Building building)
    {
        int goldAmount = building.type.goldCost;
        int gemAmount = building.type.gemCost;


        if (goldAmount != 0)
        {
            FloatingText floatingText = Instantiate(floatingText_Gold, pos - Vector2.right + Vector2.up, Quaternion.identity).GetComponent<FloatingText>();
            floatingText.SetResourceText(-goldAmount);
        }

        if (gemAmount != 0)
        {
            FloatingText floatingText = Instantiate(floatingText_Gem, pos + Vector2.right + Vector2.up, Quaternion.identity).GetComponent<FloatingText>();
            floatingText.SetResourceText(-gemAmount);
        }

    }
    public void CreateFloatText(Vector2 pos, int goldGenerationAmount, int gemGenerationAmount)
    {
        int goldAmount = goldGenerationAmount;
        int gemAmount = gemGenerationAmount;

        if (goldAmount != 0)
        {
            FloatingText floatingText = Instantiate(floatingText_Gold, pos - Vector2.right + Vector2.up, Quaternion.identity).GetComponent<FloatingText>();
            floatingText.SetResourceText(goldAmount);
        }
            

        if (gemAmount != 0)
        {
            FloatingText floatingText = Instantiate(floatingText_Gem, pos + Vector2.right + Vector2.up, Quaternion.identity).GetComponent<FloatingText>();
            floatingText.SetResourceText(gemAmount);
        }
    }
    public void RestartGame()
    {
        isGameRestarted = true;

        PlayerPrefs.DeleteAll();

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ControlResourcesForCards()
    {
        for (int i = 0; i < _buildingCards.Length; i++)
        {
            CanvasGroup cGroup = _buildingCards[i].GetComponent<CanvasGroup>();

            if (_buildingCards[i].GetComponent<DragAndDrop>().getBuilding.type.goldCost > playerData.myGold || 
                _buildingCards[i].GetComponent<DragAndDrop>().getBuilding.type.gemCost > playerData.myGem)
            {
                cGroup.alpha = .5f;
                cGroup.blocksRaycasts = false;
            }
            else
            {
                cGroup.alpha = 1;
                cGroup.blocksRaycasts = true;
            }
        }
    }
}
