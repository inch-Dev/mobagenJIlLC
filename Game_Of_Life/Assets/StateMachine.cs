using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class StateMachine : MonoBehaviour
{
    State m_currentState;
    State m_snapShotState;
    public State GetSnapshotState(){ return m_snapShotState; }
    public void SetSnapshotState(State state){ m_snapShotState = state; }
    public State GetCurrentState(){ return m_currentState; }
    public void SetCurrentState(State newState){ m_currentState = newState; }
    public bool UpdateState(Agent agent)
    {
        //Debug.Log("Updating states");
        //Check All Transitions 
        foreach(Transition transition in m_currentState.GetTransitions())
        {
            Debug.Log("Going through transitions");
            //If Condition Met
            if(transition.GetCondition().Test(agent))
            {
                Debug.Log($"{transition.GetCondition()} is true!");
                //Exit State
                foreach(Action exitAction in m_currentState.GetExitActions())
                {
                    exitAction.Execute(agent);
                }

                //Debug.Log($"Actions in transition {transition.GetActions().Count}");

                //Transition Actions
                foreach(Action transitionAction in transition.GetActions())
                {
                    transitionAction.Execute(agent);
					Debug.Log("Executing");
				}

                //Enter New State
                foreach(Action entryAction in transition.GetTargetState().GetEntryActions())
                {
                    entryAction.Execute(agent);
                }

				m_currentState = transition.GetTargetState();
				return true;
			}
        }

        foreach(Action stayAction in m_currentState.GetStayActions())
        {
            stayAction.Execute(agent);
        }
        
        return false;
    }

	private void Start()
	{
        m_snapShotState = m_currentState;
	}
}
