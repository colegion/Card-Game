using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

public abstract class TurnStateBase : IGameState
{
    protected User User;

    public void EnterState()
    {
        Debug.Log("entered turn state");
        if (User == null)
            User = GetUser();

        if (User.IsHandEmpty())
        {
            ExitImmediately();
            return;
        }

        User.InjectUserState(this);
        User.OnTurnStart();
    }

    public virtual void ExitState()
    {
        var outcomeState = GameController.Instance.GetStateByType(GameStateTypes.Outcome);
        GameController.Instance.SetLastCallerType(GetGameStateType());
        GameController.Instance.ChangeState(outcomeState);
    }

    private void ExitImmediately()
    {
        var distributionState = GameController.Instance.GetStateByType(GameStateTypes.CardDistribution);
        GameController.Instance.SetLastCallerType(GetGameStateType());
        GameController.Instance.ChangeState(distributionState);
    }

    protected abstract User GetUser();
    protected abstract GameStateTypes GetGameStateType();
}



