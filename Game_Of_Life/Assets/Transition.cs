using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct Transition
{
    [SerializeField] Condition m_Condition;
    [SerializeField] State m_Target;
    [SerializeField] List<Action> m_Actions;

    public Transition(Condition condition, State target, List<Action> actions)
    {
        m_Condition = condition;
        m_Target = target;
        m_Actions = actions;
    }

}
