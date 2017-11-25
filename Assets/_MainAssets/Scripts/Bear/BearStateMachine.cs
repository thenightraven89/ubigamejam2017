using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearState : State
{
    public BearController bear;
    public BearStateMachine bsm;
    public BearState(BearStateMachine sm, BearController ber)
    {
        bear = ber;
        bsm = sm;
    }
}

public class StartState : BearState
{
    public StartState(BearStateMachine sm, BearController ber) : base (sm, ber)
    {

    }
    public override void EnterState()
    {
        base.EnterState();
        bear.Reset();
        bsm.SwitchState(bsm.patrolState);
    }
}

public class PatrolState : BearState
{
    public PatrolState(BearStateMachine sm, BearController ber) : base (sm, ber)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        //let 's get a random point to patrol to
        bear.GetPatrolPoint();        
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if(bear.Patrol())
        {
            bsm.SwitchState(bsm.poopGoldState);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}

public class PoopGoldState : BearState
{
    private float stateDuration = 2f;
    private float stateDurationProgress = 0f;
    public PoopGoldState(BearStateMachine sm, BearController ber) : base (sm, ber)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        bear.PoopGold();
        stateDurationProgress = 0f;
        //let 's get a random point to patrol to
    }

    public override void UpdateState()
    {
        base.UpdateState();
        stateDurationProgress += Time.deltaTime;
        if(stateDurationProgress >= stateDuration)
        {
            bsm.SwitchState(bsm.patrolState);
        }
    }
}

public class BearStateMachine : StateMachine
{
    public StartState startState;
    public PatrolState patrolState;
    public PoopGoldState poopGoldState;

    public void InitStateMachine(BearController ber)
    {
        base.InitStateMachine();
        startState = new StartState(this, ber);
        patrolState = new PatrolState(this, ber);
        poopGoldState = new PoopGoldState(this, ber);

        currentState = startState;
        currentState.EnterState();
    }
}
