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
        //Check All Transitions 
        foreach(Transition transition in m_currentState.GetTransitions())
        {
            //If Condition Met
            if(transition.GetCondition().Test(agent))
            {
                //Exit State
                foreach(Action exitAction in m_currentState.GetExitActions())
                {
                    exitAction.Execute(agent);
                }

                //Enter New State
                foreach(Action entryAction in transition.GetTargetState().GetEntryActions())
                {
                    entryAction.Execute(agent);
                }
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
