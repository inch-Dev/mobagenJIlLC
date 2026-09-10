using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class StateMachine : MonoBehaviour
{
    State m_currentState;
    public State GetCurrentState(){ return m_currentState; }
    public void SetCurrentState(State newState){ m_currentState = newState; }
    public bool UpdateState(Agent agent)
    {
        Debug.Log("Updating states");
        //Check All Transitions 
        foreach(Transition transition in m_currentState.GetTransitions())
        {
            Debug.Log("Going through transitions");
            //If Condition Met
            if(transition.GetCondition().Test(agent))
            {
                //Exit State
                foreach(Action exitAction in m_currentState.GetExitActions())
                {
                    exitAction.Execute(agent);
                    Debug.Log("Executing");
                }

                //Enter New State
                foreach(Action entryAction in transition.GetTargetState().GetEntryActions())
                {
                    entryAction.Execute(agent);
                }
            }

            else
            {
                //Debug.Log($"{transition.GetCondition()} was {transition.GetCondition().Test(agent)}");
            }

            m_currentState = transition.GetTargetState();
            return true;
        }

        foreach(Action stayAction in m_currentState.GetStayActions())
        {
            stayAction.Execute(agent);
        }
        
        return false;
    }
}
