using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerData playerData;
    public GridManager gridManager;

    private void Awake() => instance = this;
    

    public void RestartGame()
    {

    }
}
