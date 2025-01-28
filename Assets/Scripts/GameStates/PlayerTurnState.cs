using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

public class PlayerTurnState : IGameState
{
    private Player _player;
    public void EnterState()
    {
        if(_player == null)
            _player = GameController.Instance.GetUser(false) as Player;
        
        _player.InjectUserState(this);
        _player.OnTurnStart();
    }

    public void ExitState()
    {
        _player.OnTurnEnd();
        var outcomeState = GameController.Instance.GetStateByType(GameStateTypes.Outcome);
        GameController.Instance.SetLastCallerType(GameStateTypes.PlayerTurn);
        GameController.Instance.ChangeState(outcomeState);
    }
}
