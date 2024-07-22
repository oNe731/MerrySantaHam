using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : class
{
    private GameObject m_owner;
    private int m_curState = -1;
    private int m_preState = -1;
    private List<State<T>> m_states;

    public GameObject Owner { get => m_owner; }
    public State<T> CurState { get => m_states[m_curState]; }
    public State<T> PreState { get => m_states[m_preState]; }
    public int CurStateIndex { get => m_curState; }
    public int PreStateIndex { get => m_preState; }

    public StateMachine(GameObject owner)
    {
        m_owner = owner;
    }

    public void Initialize_State(List<State<T>> states, int startState)
    {
        m_states = states;
        Change_State(startState);
    }

    public void Update_State()
    {
        if (m_curState == -1)
            return;

        m_states[(int)m_curState].Update_State();
    }

    public void Change_State(int stateIndex)
    {
        if (m_curState != -1)
            m_states[(int)m_curState].Exit_State();

        m_preState = m_curState;
        m_curState = stateIndex;

        m_states[(int)m_curState].Enter_State();
    }

    public void OnDrawGizmos()
    {
        m_states[(int)m_curState].OnDrawGizmos();
    }
}
