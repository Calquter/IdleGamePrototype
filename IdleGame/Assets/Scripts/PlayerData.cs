using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour, ISaveLoadData
{
    public int myGold, myGem;

    public TMP_Text myGoldText;
    public TMP_Text myGemText;


    public void SetMyGold(int amount) => myGold += amount;


    public void SetMyGem(int amount) => myGem += amount;


    public void LoadData()
    {
        
    }

    public void SaveData()
    {
        
    }
}
