using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using Interfaces;
using UnityEngine;

public class CardDistributionState : IGameState
{
    private const float CardAnimationDelay = 0.25f;
    private const int CardAmount = 4;
    private bool _initialDistributionCompleted;
    
    public void EnterState()
    {
        if (GameController.Instance.CheckIfGameFinished())
        {
            return;
        }
        else
        {
            DistributeCards();
        }
    }
    
    private void DistributeCards()
    {
        if (!_initialDistributionCompleted)
        {
            _initialDistributionCompleted = true;
            DistributeTableCards(() =>
            {
                DistributeUserCards(ExitState);
            });
        }
        else
        {
            DistributeUserCards(ExitState);
        }
    }

    private void DistributeTableCards(Action onComplete)
    {
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < CardAmount; i++)
        {
            var config = GameController.Instance.GetRandomConfig();
            var card = GameController.Instance.GetCard();
            card.ConfigureSelf(config, i < CardAmount - 1);
            GameController.Instance.RemoveCardFromDeck(config);
            sequence.AppendCallback(() =>
            {
                GameController.Instance.AppendCardsOnTable(card);
            });
            
            sequence.AppendInterval(CardAnimationDelay);
        }

        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    private void DistributeUserCards(Action onComplete)
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
            user.ReceiveCards(() =>
            {
                onComplete?.Invoke();
            });
        }
    }

    public void ExitState()
    {
        var playerTurn = GameController.Instance.GetStateByType(GameStateTypes.PlayerTurn);
        GameController.Instance.ChangeState(playerTurn);
    }
}
