using UnityEngine;
using System.Collections.Generic;
public class State : MonoBehaviour
{
    [SerializeField] List<Action> m_entryActions;
    [SerializeField] List<Action> m_stayActions;
    [SerializeField] List<Action> m_exitActions;

    [SerializeField] List<Transition> m_Transitions;
    void AddTransition(Transition newTransition)
    {
        if(!m_Transitions.Contains(newTransition))
            m_Transitions.Add(newTransition);
    }

	void AddTransition(Condition condition, State target, List<Action> actions)
	{
        Transition newTransition = new Transition(condition, target, actions);
        AddTransition(newTransition);
	}

	void AddEntryAction(Action entryAction)
    {
        if(!m_entryActions.Contains(entryAction))
            m_entryActions.Add(entryAction);
    }

    void AddExitAction(Action exitAction)
    {
        if(!m_exitActions.Contains(exitAction))
            m_exitActions.Add(exitAction);
    }

    //Stay Actions
    void AddAction(Action action)
    {
        if(!m_stayActions.Contains(action))
            m_stayActions.Add(action);
    }
}
