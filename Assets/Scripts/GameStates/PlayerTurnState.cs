using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

public class PlayerTurnState : TurnStateBase
{
    protected override User GetUser() => GameController.Instance.GetUser(false);
    protected override GameStateTypes GetGameStateType() => GameStateTypes.PlayerTurn;
    
    public override void ExitState()
    {
        (User as Player)?.OnTurnEnd();
        var outcomeState = GameController.Instance.GetStateByType(GameStateTypes.Outcome);
        GameController.Instance.SetLastCallerType(GetGameStateType());
        GameController.Instance.ChangeState(outcomeState);
    }
}
