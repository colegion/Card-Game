using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

public class CardDistributionState : IGameState
{
    private const int CardAmount = 4;
    private bool _initialDistributionCompleted;
    
    public void EnterState()
    {
        DistributeCards();
    }
    
    private void DistributeCards()
    {
        if (!_initialDistributionCompleted)
        {
            DistributeTableCards();
            DistributeUserCards();
            _initialDistributionCompleted = true;
        }
        else
        {
            DistributeUserCards();
        }

        ExitState();
    }

    private void DistributeTableCards()
    {
        for (int i = 0; i < CardAmount; i++)
        {
            var config = GameController.Instance.GetRandomConfig();
            var card = GameController.Instance.GetCard();
            card.ConfigureSelf(config, i < CardAmount - 1);
            GameController.Instance.RemoveCardFromDeck(config);
            GameController.Instance.AppendCardsOnTable(card);
        }
    }

    private void DistributeUserCards()
    {
        var users = GameController.Instance.GetAllUsers();

        foreach (var user in users)
        {
            var faceDown = user is not Player;
            var cards = new List<Card>();
            for (int i = 0; i < CardAmount; i++) 
            {
                var config = GameController.Instance.GetRandomConfig();
                var card = GameController.Instance.GetCard();
                card.ConfigureSelf(config, faceDown);
                GameController.Instance.RemoveCardFromDeck(config);
                cards.Add(card);
            }
            
            user.SetCards(cards);
        }
    }

    public void ExitState()
    {
        var playerTurn = GameController.Instance.GetStateByType(GameStateTypes.PlayerTurn);
        GameController.Instance.ChangeState(playerTurn);
    }
}
