using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

public abstract class User : MonoBehaviour
{
    [SerializeField] protected CardAnimator cardAnimator;
    [SerializeField] private Transform cardTarget;
    
    protected List<Card> Cards = new List<Card>();
    protected IGameState UserState;
    
    private List<CardConfig> _collectedCards = new List<CardConfig>();

    public void InjectUserState(IGameState state)
    {
        if (UserState != null) return;
        UserState = state;
    }
    
    public void SetCards(List<Card> cards)
    {
        Cards = cards;
    }

    public void CollectCards(List<Card> cards)
    {
        foreach (var card in cards)
        {
            _collectedCards.Add(card.GetConfig());
        }
        
        cardAnimator.MoveCardsToTarget(cards, transform);
    }

    public bool IsHandEmpty()
    {
        return Cards.Count == 0;
    }
    
    public virtual void OnCardPlayed(Card card)
    {
        Cards.Remove(card);
        cardAnimator.AnimateSelectedCard(card, cardTarget, true,() =>
        {
            GameController.Instance.AppendCardsOnTable(card);
            UserState.ExitState();
        });
    }
    
    public abstract void OnTurnStart();
}
