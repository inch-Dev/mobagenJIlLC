using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : Agent, IGameStatable
{
    public void HandleGameState()
    {
        switch (GameManager.instance.GetGameState())
        {
            case GameState.GAME_START:
                BootStrap();
                break;
            case GameState.CATCHER:
                Search();
                break;
            case GameState.GAME_OVER:
                isActive = false;
                break;
        }
    }
    // Start is called before the first frame update
    //Get position of cat
    //Get path of cat?

    //Place wall in most outside bounds
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
