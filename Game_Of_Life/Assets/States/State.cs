using UnityEngine;
using System.Collections.Generic;
public class State : MonoBehaviour
{
    [SerializeField] List<Action> m_entryActions;
    public List<Action> GetEntryActions(){ return m_entryActions; }
    [SerializeField] List<Action> m_stayActions;
    public List<Action> GetStayActions(){ return m_stayActions; }
    [SerializeField] List<Action> m_exitActions;
    public List<Action> GetExitActions(){  return m_exitActions; }

    [SerializeField] List<Transition> m_Transitions;
    public List<Transition> GetTransitions(){ return m_Transitions; }
    void AddTransition(Transition newTransition)
    {
        if(!m_Transitions.Contains(newTransition))
            m_Transitions.Add(newTransition);
    }

	protected void AddTransition(Condition condition, State target, List<Action> actions)
	{
        Transition newTransition = new Transition(condition, target, actions);
        AddTransition(newTransition);
	}

	protected void AddEntryAction(Action entryAction)
    {
        if(!m_entryActions.Contains(entryAction))
            m_entryActions.Add(entryAction);
    }

    protected void AddExitAction(Action exitAction)
    {
        if(!m_exitActions.Contains(exitAction))
            m_exitActions.Add(exitAction);
    }

    //Stay Actions
    protected void AddAction(Action action)
    {
        if(!m_stayActions.Contains(action))
            m_stayActions.Add(action);
    }
}
