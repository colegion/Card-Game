using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using Interfaces;
using UnityEngine;

public class OutcomeState : IGameState
{
    private GameRules _gameRules;
    public void EnterState()
    {
        if (_gameRules == null)
            _gameRules = new GameRules();
        DecideOutcomeIfPossible();
    }

    private void DecideOutcomeIfPossible()
    {
        var cardsOnTable = GameController.Instance.GetCardsOnTable();
        if (cardsOnTable.Count <= 1)
        {
            ExitState();
            return;
        }

        var lastCallerType = GameController.Instance.GetLastOutcomeCallerType();
        var user = GameController.Instance.GetUser(lastCallerType == GameStateTypes.BotTurn);

        if (_gameRules.IsMoveCollectable(cardsOnTable, out CollectType type))
        {
            if(type == CollectType.Pisti) user.IncrementPistiCount();
            CollectCardsAndExit(user, cardsOnTable, type);
        }
        else
        {
            ExitState();
        }
    }
    
    private void CollectCardsAndExit(User user, List<Card> cardsOnTable, CollectType type)
    {
        user.CollectCards(cardsOnTable, type, () =>
        {
            GameController.Instance.ClearOnTableCards();
            ExitState();
        });
    }

    public void ExitState()
    {
        var lastCaller = GameController.Instance.GetLastOutcomeCallerType();
        GameStateTypes newCaller = lastCaller;
        if (lastCaller == GameStateTypes.BotTurn)
        {
            newCaller = GameStateTypes.PlayerTurn;
        }
        
        else if (lastCaller == GameStateTypes.PlayerTurn)
        {
            newCaller = GameStateTypes.BotTurn;
        }
        
        GameController.Instance.ChangeState(GameController.Instance.GetStateByType(newCaller));
    }

    public void ResetAttributes()
    {
        
    }
}
