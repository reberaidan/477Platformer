using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderKeywordFilter;
using UnityEditorInternal;
using UnityEngine;
using State = EnemyState;

public enum EnemyState { 
    IDLE,
    UP,
    DOWN,
}

public class FlyingGoomba : MonoBehaviour
{
    public float move = 0.1f;
    public float distance;

    private Dictionary<State, Action> stateEnterMeths;
    private Dictionary<State, Action> stateExitMeths;
    private Dictionary<State, Action> stateStayMeths;
    private float xTarget;
    private Vector3 m_Velocity = Vector3.zero;
    private float m_MovementSmoothing = 0.05f;

    public State State { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        stateEnterMeths = new Dictionary<State, Action>()
        {
            { State.IDLE, StateEnterIdle },
            { State.UP, StateEnterUp },
            { State.DOWN, StateEnterDown },

        };
        stateExitMeths = new Dictionary<State, Action>()
        {
            { State.IDLE, StateExitIdle },
            { State.UP, StateExitUp },
            { State.DOWN, StateExitDown },

        };
        stateStayMeths = new Dictionary<State, Action>()
        {
            { State.IDLE, StateStayIdle },
            { State.UP, StateStayUp },
            { State.DOWN, StateStayDown },

        };
        State = State.IDLE;

    }
    void FixedUpdate()
    {
        stateStayMeths[State].Invoke();
    }
    private void ChangeState(State newState)
    {
        if(State != newState)
        {
            stateExitMeths[State].Invoke();
            State = newState;
            stateEnterMeths[State].Invoke();
        }
    }

    #region state meths
    #region StayStates
    private void StateStayDown()
    {

        // And then smoothing it out and applying it to the character
        if (transform.position.y >= xTarget)
        {
            Vector3 targetVelocity = new Vector2(transform.position.x,transform.position.y-move);
            transform.position = Vector3.SmoothDamp(transform.position, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }
        else {
            ChangeState(State.UP);
        }
    }


    private void StateStayUp()
    {
        if(transform.position.y <= xTarget) {
            Vector3 targetVelocity = new Vector2(transform.position.x,transform.position.y+move);
            transform.position = Vector3.SmoothDamp(transform.position, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }
        else
        {
            ChangeState(State.DOWN);
        }
    }

    private void StateStayIdle()
    {
        ChangeState(State.UP);
    }
    #endregion

    #region ExitStates
    private void StateExitDown()
    {
        
    }

    private void StateExitUp()
    {
    }

    private void StateExitIdle()
    {
    }
    #endregion

    #region EnterStates
    private void StateEnterDown()
    {
        xTarget = transform.position.y - distance;
    }

    private void StateEnterUp()
    {
        xTarget = transform.position.y + distance;
    }

    private void StateEnterIdle()
    {

        
    }
    #endregion
    #endregion
    // Update is called once per frame
}
