using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour, ISaveLoadData
{
    public int myGold, myGem;

    public TMP_Text myGoldText;
    public TMP_Text myGemText;


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
        
    }

    public void SaveData()
    {
        
    }
}
