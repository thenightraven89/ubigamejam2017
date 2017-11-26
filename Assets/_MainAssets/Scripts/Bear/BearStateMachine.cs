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
    public StartState(BearStateMachine sm, BearController ber) : base(sm, ber)
    {

    }
    public override void EnterState()
    {
        base.EnterState();
        bear.Reset();
        bsm.SwitchState(bsm.patrolState);
    }
}

public class TravelToPooPlaceState : BearState
{
    public TravelToPooPlaceState(BearStateMachine sm, BearController ber) : base(sm, ber)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        //let 's get a random point to patrol to
        bear.GetPooPoint();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (bear.GoToPooDestination())
        {
            bsm.SwitchState(bsm.poopGoldState);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}

public class PatrolState : BearState
{
    public PatrolState(BearStateMachine sm, BearController ber) : base(sm, ber)
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
        if (bear.Patrol())
        {
            bsm.SwitchState(bsm.lookConcernedState);
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
    public PoopGoldState(BearStateMachine sm, BearController ber) : base(sm, ber)
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
        if (stateDurationProgress >= stateDuration)
        {
            bsm.SwitchState(bsm.patrolState);
        }
    }
}

public class LookConcernedState : BearState
{
    private float stateDuration = 3f;
    private float stateDurationProgress = 0f;
    public LookConcernedState(BearStateMachine sm, BearController ber) : base(sm, ber)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        bear.LookConcerned();
        stateDurationProgress = 0f;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        stateDurationProgress += Time.deltaTime;
        if (stateDurationProgress >= stateDuration)
        {
            if (!bear.ShouldPoo())
                bsm.SwitchState(bsm.patrolState);
            else
                bsm.SwitchState(bsm.travelToPooPlaceState);
        }
    }
}

public class ChaseState : BearState
{
    public ChaseState(BearStateMachine sm, BearController ber) : base(sm, ber)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        bear.StartChase();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if(bear.Chase()) // if player was caught
        {
            bsm.SwitchState(bsm.catchState);
        }
    }
}

public class CatchState : BearState
{
    public CatchState(BearStateMachine sm, BearController ber) : base(sm, ber)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        int val = bear.EvaluateCatch(); //0 caught, 1 feigned death but seen, 2 - feigned death but not seen
        switch (val)
        {
            case 0:
                bsm.SwitchState(bsm.molestState);
                break;
            case 1:
                bsm.SwitchState(bsm.molestState);
                break;
            case 2:
                bsm.SwitchState(bsm.patrolState);
                break;
            default:
                break;
        }
    }
}

public class MolestState : BearState
{
    private float stateDuration = 3f;
    private float stateDurationProgress = 0f;

    public MolestState(BearStateMachine sm, BearController ber) : base(sm, ber)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        bear.MolestPlayer();
        stateDurationProgress = 0f;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        stateDurationProgress += Time.deltaTime;
        if (stateDurationProgress >= stateDuration)
        {
            if (!bear.ShouldPoo())
                bsm.SwitchState(bsm.patrolState);
            else
                bsm.SwitchState(bsm.travelToPooPlaceState);
        }
    }
}

public class BearStateMachine : StateMachine
{
    //interrupts
    public static string PLAYER_SPOTTED = "playerSpotted";

    //states
    public StartState startState;
    public PatrolState patrolState;
    public TravelToPooPlaceState travelToPooPlaceState;
    public PoopGoldState poopGoldState;
    public LookConcernedState lookConcernedState;
    public ChaseState chaseState;
    public CatchState catchState;
    public MolestState molestState;

    public void InitStateMachine(BearController ber)
    {
        base.InitStateMachine();
        startState = new StartState(this, ber);
        patrolState = new PatrolState(this, ber);
        travelToPooPlaceState = new TravelToPooPlaceState(this, ber);
        poopGoldState = new PoopGoldState(this, ber);
        lookConcernedState = new LookConcernedState(this, ber);
        chaseState = new ChaseState(this, ber);
        catchState = new CatchState(this, ber);
        molestState = new MolestState(this, ber);

        currentState = startState;
        currentState.EnterState();
    }

    public void SendInterrupt(string interruptName)
    {
        if (interruptName == PLAYER_SPOTTED)
            SwitchState(chaseState); // hacked for now
    }
}
