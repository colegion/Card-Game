using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using Interfaces;
using UnityEngine;

public class OutcomeState : IGameState
{
    public void EnterState()
    {
        DecideOutcomeIfPossible();
    }

    private void DecideOutcomeIfPossible()
    {
        var cardsOnTable = GameController.Instance.GetCardsOnTable();

        if (cardsOnTable.Count <= 1)
        {
            ExitState();
        }
        else
        {
            if (AreLastTwoCardsSame(cardsOnTable) || IsLastCardJack(cardsOnTable[^1]))
            {
                var user = GameController.Instance.GetUser(GameController.Instance.GetLastOutcomeCallerType() ==
                                                           GameStateTypes.BotTurn);
                user.CollectCards(cardsOnTable);
                GameController.Instance.ClearOnTableCards();
            }
            
            DOVirtual.DelayedCall(1.3f, ExitState);
        }
    }

    private bool AreLastTwoCardsSame(List<Card> cards)
    {
        var lastCard = cards[^1].GetConfig();
        var secondLastCard = cards[^2].GetConfig();
        return lastCard.Equals(secondLastCard);
    }

    private bool IsLastCardJack(Card card)
    {
        return card.IsJackCard();
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
}
