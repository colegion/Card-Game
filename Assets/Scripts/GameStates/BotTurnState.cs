using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

public class BotTurnState : IGameState
{
    public void EnterState()
    {
        var bot = GameController.Instance.GetUser(true);
        bot.InjectUserState(this);
        bot.OnTurnStart();
    }

    public void ExitState()
    {
        var outcomeState = GameController.Instance.GetStateByType(GameStateTypes.Outcome);
        GameController.Instance.SetLastCallerType(GameStateTypes.BotTurn);
        GameController.Instance.ChangeState(outcomeState);
    }
}
