using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    GAME_START,
    CAT,
    CATCHER,
    CAT_WIN,
    CATCHER_WIN,

}
    
public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager instance;
    [SerializeField] TextMeshProUGUI catWinsTF;
    [SerializeField] TextMeshProUGUI catcherWinsTF;
    [SerializeField] float stepSeconds;
    GameState _currentGameState;
    public GameState GetGameState(){ return _currentGameState; }

    public void SetGameState(int gameState)
    {
        SetGameState((GameState)gameState);
    }
    public void SetGameState(GameState newState)
    {
        Debug.Log($"Setting game sate to {newState}");

        switch (newState)
        {
            case GameState.CAT_WIN:
                catWinsTF.gameObject.SetActive(true);
                return;
            case GameState.CATCHER_WIN:
                catcherWinsTF.gameObject.SetActive(true);
                return;
        }
            
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

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator StepTimer(GameState newState)
    {
        yield return new WaitForSeconds(stepSeconds);
		_currentGameState = newState;

        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();

		List<IGameStatable> gameStatables = new List<IGameStatable>();

		foreach (var script in allScripts)
        {
            if (script is IGameStatable)
                gameStatables.Add(script as IGameStatable);
        }

        foreach(var script in gameStatables)
        {
            script.HandleGameState();
        }

	}
}
