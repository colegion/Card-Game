using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using Interfaces;
using UnityEngine;

public class CardDistributionState : IGameState
{
    private const float CardAnimationDelay = 0.1f;
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

        DOVirtual.DelayedCall(2.6f, ExitState); 
    }

    private void DistributeTableCards()
    {
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < CardAmount; i++)
        {
            var config = GameController.Instance.GetRandomConfig();
            var card = GameController.Instance.GetCard();
            card.ConfigureSelf(config, i < CardAmount - 1);
            GameController.Instance.RemoveCardFromDeck(config);

            sequence.AppendInterval(CardAnimationDelay)
                .AppendCallback(() => GameController.Instance.AppendCardsOnTable(card));
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
