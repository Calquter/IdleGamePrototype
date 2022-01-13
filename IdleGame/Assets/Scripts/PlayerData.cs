using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour, ISaveLoadData
{
    public int myGold, myGem;

    public TMP_Text myGoldText;
    public TMP_Text myGemText;


    private void Start()
    {
        LoadData();
        UpdateResources();
    }
    public void SetMyGold(int amount)
    {
        myGold += amount;
        UpdateResources();
    }
    public void SetMyGem(int amount)
    {
        myGem += amount;
        UpdateResources();
    }
    private void UpdateResources()
    {
        myGoldText.text = myGold.ToString();
        myGemText.text = myGem.ToString();
    }
    public void LoadData()
    {
        if (PlayerPrefs.HasKey("myGold"))
        {
            myGold = PlayerPrefs.GetInt("myGold");
            myGem = PlayerPrefs.GetInt("myGem");
        }
    }
    public void SaveData()
    {
        if (!GameManager.instance.isGameRestarted)
        {
            PlayerPrefs.SetInt("myGold", myGold);
            PlayerPrefs.SetInt("myGem", myGem);
        }
    }
    private void OnApplicationQuit() => SaveData();

}
