using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iState
{
    void EnterState();
    void UpdateState();
    void EndState();
}

public class State : iState
{
    public virtual void EndState()
    {
        Debug.Log("Ending State: " + this.GetType().ToString());
    }

    public virtual void EnterState()
    {
        Debug.Log("Entering State: " + this.GetType().ToString());
    }

    public virtual void UpdateState()
    {
        Debug.Log("Updating State: " + this.GetType().ToString());
    }
}

public class StateMachine
{
    protected State currentState;
    protected State previousState;

    public virtual void InitStateMachine()
    {

    }

    public virtual void UpdateState()
    {
        if (currentState == null) return;
        currentState.UpdateState();
    }

    public virtual void SwitchState(State nextState)
    {
        //let us exit current state
        currentState.EndState();
        nextState.EnterState();
        previousState = currentState;
        currentState = nextState;
    }
}
