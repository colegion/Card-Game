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
    private int _roundIndex = 0;
    public static event Action<int, Action> OnRoundDistributed; 
    
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
        Debug.Log("Distributing table cards...");

        for (int i = 0; i < CardAmount; i++)
        {
            var config = GameController.Instance.GetRandomConfig();
            if (config.cardValue == CardValue.Null)
            {
                Debug.LogError("Deck is empty while distributing table cards!");
                break;
            }

            var card = GameController.Instance.GetCard();
            card.ConfigureSelf(config, i < CardAmount - 1);
            GameController.Instance.RemoveCardFromDeck(config);
            Debug.Log($"Added {config.cardValue} to table.");

            sequence.AppendCallback(() =>
            {
                GameController.Instance.AppendCardsOnTable(card);
            });

            sequence.AppendInterval(CardAnimationDelay);
        }

        sequence.OnComplete(() =>
        {
            Debug.Log("Table cards distributed.");
            onComplete?.Invoke();
        });
    }


    private void DistributeUserCards(Action onComplete)
    {
        var users = GameController.Instance.GetAllUsers();
        RoutineHelper.Instance.StartRoutine(DistributeUserCardsCoroutine(users, CardAmount, onComplete));
    }

    private IEnumerator DistributeUserCardsCoroutine(List<User> users, int cardAmount, Action onComplete)
    {
        Debug.Log($"Starting user card distribution. Expected cards per user: {cardAmount}");
    
        for (int i = 0; i < cardAmount; i++)
        {
            for (int j = 0; j < users.Count; j++)
            {
                var user = users[j];
                var faceDown = user is not Player;

                var config = GameController.Instance.GetRandomConfig();
                if (config.cardValue == CardValue.Null)
                {
                    Debug.LogError("Deck is out of cards!");
                    yield break;
                }

                var card = GameController.Instance.GetCard();
                card.ConfigureSelf(config, faceDown);
                GameController.Instance.RemoveCardFromDeck(config);
                user.AddCardToHand(card);

                Debug.Log($"Distributed card {config.cardValue} to {user.GetType()}");

                bool animationCompleted = false;
                user.ReceiveSingleCard(card, () => animationCompleted = true);

                yield return new WaitUntil(() => animationCompleted);
                yield return new WaitForSeconds(0.1f);
            }
        }

        Debug.Log("User card distribution complete.");
        onComplete?.Invoke();
    }



    public void ExitState()
    {
        _roundIndex++;
        GameController.Instance.CheckRemainingCardAmount();
        OnRoundDistributed?.Invoke(_roundIndex, () =>
        {
            var playerTurn = GameController.Instance.GetStateByType(GameStateTypes.PlayerTurn);
            GameController.Instance.ChangeState(playerTurn);
        });
        
    }

    public void ResetAttributes()
    {
        _initialDistributionCompleted = false;
        _roundIndex = 0;
    }
}
