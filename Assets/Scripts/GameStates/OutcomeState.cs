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
            return;
        }

        var lastCallerType = GameController.Instance.GetLastOutcomeCallerType();
        var user = GameController.Instance.GetUser(lastCallerType == GameStateTypes.BotTurn);
        
        if (cardsOnTable.Count == 2 && AreLastTwoCardsSame(cardsOnTable))
        {
            user.IncrementPistiCount();
            CollectCardsAndExit(user, cardsOnTable, true);
            return;
        }
        
        if (AreLastTwoCardsSame(cardsOnTable) || IsLastCardJack(cardsOnTable[^1]))
        {
            CollectCardsAndExit(user, cardsOnTable, false);
            return;
        }

        ExitState();
    }
    
    private void CollectCardsAndExit(User user, List<Card> cardsOnTable, bool isPisti)
    {
        user.CollectCards(cardsOnTable, isPisti, () =>
        {
            GameController.Instance.ClearOnTableCards();
            ExitState();
        });
    }

    private bool AreLastTwoCardsSame(List<Card> cards)
    {
        var lastCard = cards[^1].GetConfig();
        var secondLastCard = cards[^2].GetConfig();
        Debug.Log($"Last two cards: {lastCard.cardValue} {lastCard.cardSuit} and {secondLastCard.cardValue} {secondLastCard.cardSuit}");
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
