using System.ComponentModel;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

public enum GameState
{
    CREATE,
	PLAY
}


public interface IStateable
{
	public void HandleState(GameState state);
}

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager instance;
	GameState m_curState;

	public void SetState(int state)
	{
		SetState((GameState)state);
	}


	public void SetState(GameState state)
	{
		m_curState = state;

		//Get Stateables
		List<IStateable> stateables = new List<IStateable>();
		
		MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>(); 
		foreach(MonoBehaviour script in allScripts)
		{
			if (script is IStateable && !stateables.Contains(script as IStateable))
				stateables.Add(script as IStateable);
		}
		
		//Change State Behavior
		foreach(IStateable stateable in stateables)
		{
			stateable.HandleState(m_curState);
		}
		
	}

	private void Start()
	{
		if(instance == null)
			instance = this;
		SetState(GameState.CREATE);
	}
}
