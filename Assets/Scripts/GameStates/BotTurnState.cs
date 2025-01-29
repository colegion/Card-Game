using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

public class BotTurnState : TurnStateBase
{
    protected override User GetUser() => GameController.Instance.GetUser(true);
    protected override GameStateTypes GetGameStateType() => GameStateTypes.BotTurn;
}
