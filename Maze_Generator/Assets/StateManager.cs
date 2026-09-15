using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public enum State
{
    PAUSED,
    PLAY
}



public class StateManager : MonoBehaviour
{
    [HideInInspector] public static StateManager instance;
    State currentState;
    public void SetState(int state) { SetState((State)state); }
    public void SetState(State state)
    {
        UpdateStateables();
        currentState = state;

        foreach(IStateable stateable in stateables)
        {
            stateable.HandleState(currentState);
        }

    }
    public State GetState() { return currentState; }

    List<IStateable> stateables = new List<IStateable>();
    void UpdateStateables()
    {
        stateables.Clear();

        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();

        foreach(MonoBehaviour script in allScripts)
        {
            if(script is IStateable)
            {
                stateables.Add(script as IStateable);
            }
        }
    }
    
    void Start()
    {
        SetState(State.PLAY);
    }
}
