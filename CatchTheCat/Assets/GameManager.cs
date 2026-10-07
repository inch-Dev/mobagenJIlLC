using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GAME_START,
    CAT,
    CATCHER,
    GAME_OVER
}
    
public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager instance;
    [SerializeField] float stepSeconds;
    GameState _currentGameState;
    public GameState GetGameState(){ return _currentGameState; }

    public void SetGameState(int gameState)
    {
        SetGameState((GameState)gameState);
    }
    public void SetGameState(GameState newState)
    { 
        StartCoroutine(StepTimer(newState));
    }

    void Start()
    {
        if (instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StepTimer(GameState newState)
    {
        yield return new WaitForSeconds(stepSeconds);
		_currentGameState = newState;
	}
}
