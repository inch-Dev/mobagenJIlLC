using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IGameStatable
{
    public void HandleGameState()
    {
        if(GameManager.instance.GetGameState() == role)
        {
            
        }
    }
    GameState role;
    
    void Click()
    {
        //Run Collisions

        switch (role)
        {
            case GameState.CAT:
                break;
            case GameState.CATCHER:
                break;
        }
            
    }
}
